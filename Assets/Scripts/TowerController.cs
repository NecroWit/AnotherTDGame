using System;
using System.Collections;
using System.Collections.Generic;
using AnotherDTGame;
using UnityEditor;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    
    private TowerSettings _currentSettings;
    private int _currentLevel = 0;

    private EnemyController _targetEnemy;
    public float waitDelay = 0.1f;


    public bool isWorking = false;
    private bool _maxLevelReached = false;

    public GameObject updateObj;

    private void Start()
    {
        GetCurrentSettings();
        GameMaster.Instance.onGameStart.AddListener(StartWorking);
        GameMaster.Instance.onGameOver.AddListener(StopWorking);
        GameMaster.Instance.onGoldAmountChanged.AddListener(CheckIsReadyToUpdate);
    }

    IEnumerator Searching()
    {
        while (isWorking)
        {
            if (TryGetNearestEnemy())
            {
                StopAllCoroutines();
                StartCoroutine(Firing());
            }
            yield return new WaitForSeconds(waitDelay);
        }
    }

    IEnumerator Firing()
    {
        while (_targetEnemy != null)
        {
            if (Vector3.Distance(_targetEnemy.transform.localPosition, transform.localPosition) <
                _currentSettings.radius)
            {
                bool isDead = _targetEnemy.Hit(_currentSettings.damage);

#if UNITY_EDITOR
                Debug.DrawLine(transform.localPosition, _targetEnemy.GetLocalPos(), Color.magenta);
#endif
                if (isDead)
                    _targetEnemy = null;
            }
            else
                _targetEnemy = null;

            yield return new WaitForSeconds(60f / _currentSettings.bulletsPerMinute);
        }
        StopAllCoroutines();
        StartCoroutine(Searching());
    }


    public bool TryGetNearestEnemy()
    {
        _targetEnemy = BoardMaster.Instance.GetNearestEnemy(transform.localPosition, _currentSettings.radius);
        if (_targetEnemy != null)
            return true;
        return false;
    }

    public void StartWorking()
    {
        ChangeIsWorking(true);
        StartCoroutine(Searching());
    }

    public void StopWorking()
    {
        StopAllCoroutines();
        ChangeIsWorking(false);
    }

    public void ChangeIsWorking(bool newWorkingValue)
    {
        isWorking = newWorkingValue;
    }

    private void GetCurrentSettings()
    {
        _currentSettings = BoardMaster.Instance.GetTowerSettings(_currentLevel, out _maxLevelReached);
    }

    public void CheckIsReadyToUpdate()
    {
        if (GameMaster.Instance.GetGold() >= _currentSettings.updatePrice &&
            !_maxLevelReached)
        {
            updateObj.SetActive(true);
        }
        else
        {
            updateObj.SetActive(false);
        }
    }

    public void UpdateTower()
    {
        GameMaster.Instance.ChangeGold(-_currentSettings.updatePrice);
        _currentLevel++;
        GetCurrentSettings();
        CheckIsReadyToUpdate();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.localPosition// +(transform.forward *Range) // position
            , transform.up                       // normal
            , _currentSettings.radius);        
    }
#endif
}

using System;
using System.Collections;
using System.Collections.Generic;
using AnotherDTGame;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    
    private TowerSettings _currentSettings;
    private int currentLevel = 0;

    private EnemyController targetEnemy;


    public bool isWorking = false;


    private void Start()
    {
        GetCurrentSettings();
        GameMaster.Instance.onGameStart.AddListener(StartWorking);
        GameMaster.Instance.onGameOver.AddListener(StopWorking);
    }
    
    //Implement thru coroutine if to often
    private void FixedUpdate()
    {
        if (isWorking)
        {
            if (targetEnemy != null)
            {
                targetEnemy.Hit(_currentSettings.damage);
            }
            else
            {
                GetNearestEnemy();
            }
        }
    }

    public void GetNearestEnemy()
    {
        targetEnemy = BoardMaster.Instance.GetNearestEnemy(transform.localPosition);
        if (targetEnemy == null)
            isWorking = true;
    }

    public void StartWorking()
    {
        ChangeIsWorking(true);
    }

    public void StopWorking()
    {
        ChangeIsWorking(false);
    }

    public void ChangeIsWorking(bool newWorkingValue)
    {
        isWorking = newWorkingValue;
    }

    private void GetCurrentSettings()
    {
        _currentSettings = BoardMaster.Instance.GetTowerSettings(currentLevel, out bool maxLevelReached);

        if (maxLevelReached)
            MaxLevelReached();
    }

    public void MaxLevelReached()
    {
        
    }
}

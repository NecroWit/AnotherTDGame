﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnotherDTGame
{
    [System.Serializable]
    public class EnemySettings : System.ICloneable
    {
        public string name;
        public float speed;
        public int attack;
        public int attackVariation;
        public int health;
        public int healthVariation;
        public int goldAward;
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    
    [System.Serializable]
    public class TowerSettings
    {
        public string name;
        public int damage;
        public int bulletsPerMinute;
        public float radius;
        public int updatePrice;
    }

    public class BoardMaster : SingletonBehaviour<BoardMaster>
    {
        [SerializeField] private Transform spawnPos = null;

        [SerializeField] private Transform endPos = null;

        [SerializeField] private GameObject pathContainer;

        private List<Vector3> _pathPositions = new List<Vector3>();

        public int enemiesLevel;
        [SerializeField] float spawnDelay = 4f;

        [SerializeField] private int maxEnemiesRandomRange = 4;

        [SerializeField] private int currentLevelEnemiesAmount;

        //[SerializeField] private List<EnemySettings> enemySettings;

        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject enemyContainer;
        

        private List<EnemyController> _enemyList = new List<EnemyController>();

        [SerializeField] private EnemySettings baseEnemySettings;

        private EnemySettings _currentEnemySettings = new EnemySettings();

        [SerializeField] private List<TowerSettings> towerSettingsList = new List<TowerSettings>();

        private void Awake()
        {
            Transform[] pathObjs = pathContainer.GetComponentsInChildren<Transform>();
            foreach (var pathObj in pathObjs)
            {
                if (pathObj.localPosition != Vector3.zero)
                    _pathPositions.Add(pathObj.localPosition);
            }
            Init();
            _currentEnemySettings = (EnemySettings)baseEnemySettings.Clone();
        }

        public void StartGame()
        {
            StartSpawnEnemies();
        }

        public void RestartGame()
        {
            Clear();
            StartGame();
        }

        public void Clear()
        {
            foreach (var enemy in _enemyList)
            {
                enemy.Die();
            }
            _enemyList.Clear();
            _currentEnemySettings = (EnemySettings)baseEnemySettings.Clone();
        }

        public Vector3 GetPositionOnPath(int n)
        {
            if (n < _pathPositions.Count)
                return _pathPositions[n];
            else
                return Vector3.zero;
        }

        public bool IsInFinalPoint(EnemyController checkEnemy)
        {
            if (endPos == null)
                return false;
            else
            {
                if (checkEnemy.transform.localPosition == endPos.localPosition)
                {
                    RemoveEnemy(checkEnemy);
                    HitBase(checkEnemy);
                    return true;
                }
            }
            return false;
        }

        public void HitBase(EnemyController enemy)
        {
            GameMaster.Instance.DecreaseLives(enemy.GetCurrentAttack());
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            _enemyList.Remove(enemy);
            Invoke(nameof(CheckIsLevelEnd), 0.1f);
        }
        
        public void CheckIsLevelEnd()
        {
            if (_enemyList.Count == 0)
            {
                NextLevel();
            }
        }

        public void NextLevel()
        {
            GameMaster.Instance.LevelUp();
            VariateEnemy();
            StartSpawnEnemies();
        }

        public void VariateEnemy()
        {
            if (Random.value > 0.5f)
            {
                _currentEnemySettings.attack += Random.Range(0, _currentEnemySettings.attackVariation);
            }
            if (Random.value > 0.5f)
            {
                _currentEnemySettings.health += Random.Range(0, _currentEnemySettings.healthVariation);
            }
        }

        public void AddEnemy(EnemyController enemy)
        {
            _enemyList.Add(enemy);
        }

        public void StartSpawnEnemies()
        {
            var currentLevel = GameMaster.Instance.GetLevel();
            currentLevelEnemiesAmount = Random.Range(currentLevel,  
                currentLevel + maxEnemiesRandomRange);
            StartCoroutine(SpawnEnemy());
        }

        IEnumerator SpawnEnemy()
        {
            while (currentLevelEnemiesAmount > 0)
            {
                currentLevelEnemiesAmount--;
                GameObject enemyGo = Instantiate(enemyPrefab, enemyContainer.transform);
                enemyGo.name = _currentEnemySettings.name + currentLevelEnemiesAmount;

                var enemyCont = enemyGo.GetComponent<EnemyController>();

                if (enemyCont != null)
                {
                    AddEnemy(enemyCont);
                    enemyCont.StartMove(spawnPos.localPosition, _currentEnemySettings);
                }

                yield return new WaitForSeconds(spawnDelay);
            }

            StopAllCoroutines();
        }


        public TowerSettings GetTowerSettings(int towerLevel, out bool maxLevelReached)
        {
            maxLevelReached = (towerLevel + 1) == towerSettingsList.Count;
            return towerSettingsList[towerLevel];
        }

        public EnemyController GetNearestEnemy(Vector3 position, float radius)
        {
            if (_enemyList.Count == 0)
                return null;

            EnemyController resEnemy = null;

            foreach (EnemyController currentEnemy in _enemyList)
            {
                if (resEnemy == null &&
                    Vector3.Distance(currentEnemy.GetLocalPos(), position) < radius)
                {
                    resEnemy = currentEnemy;
                }
                else if ((Vector3.Distance(currentEnemy.GetLocalPos(), position) < radius) &&
                         (Vector3.Distance(currentEnemy.GetLocalPos(), position) <
                          Vector3.Distance(resEnemy.GetLocalPos(), position)))
                {
                    resEnemy = currentEnemy;
                }
            }

            return resEnemy;
        }
    }
}

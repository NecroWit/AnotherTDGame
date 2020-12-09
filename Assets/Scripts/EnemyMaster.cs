using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySettings
{
    public string name;
    public float speed;
    public int attack;
    public int health;
    public int goldAward;
}

public class EnemyMaster : MonoBehaviour
{

    public static EnemyMaster Instance;

    private void OnDestroy()
    {
        Instance = null;
    }

    [SerializeField]
    private Transform spawnPos = null;

    [SerializeField] 
    private Transform endPos = null;

    [SerializeField]
    private GameObject pathContainer;
    
    public List<Vector3> _pathPositions = new List<Vector3>();

    public int enemiesLevel;
    [SerializeField]
    float spawnDelay = 4f;

    [SerializeField]
    private int maxEnemies = 10;
    [SerializeField]
    private int maxEnemiesRandomRange = 4;

    [SerializeField]
    private int currentLevelEnemiesAmount;

    [SerializeField]
    private List<EnemySettings> enemySettings;

    [SerializeField]
    private GameObject enemyPrefab;
    
    private List<EnemyController> _enemyList = new List<EnemyController>();

    [SerializeField]
    private EnemySettings baseEnemySettings;

    private void Awake()
    {
        Transform[] pathObjs = pathContainer.GetComponentsInChildren<Transform>();
        foreach(var pathObj in pathObjs)
        {
            if (pathObj.localPosition != Vector3.zero)
                _pathPositions.Add(pathObj.localPosition);
        }

        Instance = this;

        StartSpawnEnemies();
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
                return true;
            }
        }
        return false;
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemyList.Remove(enemy);
    }

    public void AddEnemy(EnemyController enemy)
    {
        _enemyList.Add(enemy);
    }

    public void StartSpawnEnemies()
    {
        currentLevelEnemiesAmount = maxEnemies + Random.Range(0, maxEnemiesRandomRange);
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (currentLevelEnemiesAmount > 0)
        {
            currentLevelEnemiesAmount--;
            GameObject enemyGo = Instantiate(enemyPrefab, transform);
            enemyGo.name = baseEnemySettings.name + currentLevelEnemiesAmount;

            var enemyCont = enemyGo.GetComponent<EnemyController>();

            if (enemyCont != null)
            {
                AddEnemy(enemyCont);
                enemyCont.StartMove(spawnPos.localPosition, baseEnemySettings);
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        StopAllCoroutines();
    }
}

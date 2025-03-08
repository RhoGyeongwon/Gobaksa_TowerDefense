using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform[] wayPoints;

    void Awake()
    {
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject enemyObject = Instantiate(enemyPrefab);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.Setup(wayPoints);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}

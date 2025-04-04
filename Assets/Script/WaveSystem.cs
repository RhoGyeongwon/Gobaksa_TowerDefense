using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public float spawnRate;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
public class WaveSystem : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;

    public int CurrentWave => currentWaveIndex + 1; //단순한 값을 읽기 위해서라면, 여기서 처리하는 건 좋지않은거같다.
    public int MaxWave => waves.Length;
    
    public void StartWave()
    {
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}


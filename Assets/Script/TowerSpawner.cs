using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int towerBuildGold = 50;
    [SerializeField] private PlayerGold playerGold;

    public void SpawnTower(Transform tileTransform)
    {
        if (towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }
        
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower)
        {
            return;
        }
        
        tile.IsBuildTower = true;
        playerGold.CurrentGold -= towerBuildGold;
        Vector3 position = tileTransform.position + Vector3.back; //왜 타일보다 z축 -1 위치에 배치했을까? 아 타워가 더 앞에 있다!
        GameObject towerObject = Instantiate(towerPrefab, position, Quaternion.identity);
        towerObject.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}

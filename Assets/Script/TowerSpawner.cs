using System.Collections;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private TowerTemplate towerTemplate;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private PlayerGold playerGold;
    [SerializeField] private SystemTextViewer systemTextViewer;
    private bool IsOnTowerButton = false;
    private GameObject followTowerClone = null;

    public void ReadyToSpawnTower()
    {
        if (IsOnTowerButton == true)
        {
            return;
        }
        if (towerTemplate.weapons[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        IsOnTowerButton = true;
        followTowerClone = Instantiate(towerTemplate.followTowerPrefab);
        StartCoroutine("OnTowerCancelSystem");
    }
    
    public void SpawnTower(Transform tileTransform)
    {
        if (IsOnTowerButton == false)
        {
            return;
        }
        
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true) // if(tile.IsBuildTower)랑 같다.
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        IsOnTowerButton = false;
        tile.IsBuildTower = true;
        playerGold.CurrentGold -= towerTemplate.weapons[0].cost;
        Vector3 position = tileTransform.position + Vector3.back; //왜 타일보다 z축 -1 위치에 배치했을까? 아 타워가 더 앞에 있다!
        GameObject towerObject = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity);
        towerObject.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);
        
        Destroy(followTowerClone);
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                IsOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }
}

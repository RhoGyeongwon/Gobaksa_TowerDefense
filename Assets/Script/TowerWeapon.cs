using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum WeaponState
{
    SearchTarget = 0,
    AttackToTarget
}

public class TowerWeapon : MonoBehaviour
{
    [SerializeField] private TowerTemplate towerTemplate;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    
    private int level = 0;
    private WeaponState weaponState = WeaponState.SearchTarget;

    private Transform attackTarget = null;
    private SpriteRenderer spriteRenderer;
    private EnemySpawner enemySpawner;
    private PlayerGold playerGold;
    private Tile ownerTile;
    public Sprite TowerSprite => towerTemplate.weapons[level].sprite;
    public float Damage => towerTemplate.weapons[level].damage;
    public float Rate => towerTemplate.weapons[level].rate;
    public float Range => towerTemplate.weapons[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapons.Length;
    // Start is called before the first frame update

    public void Setup(EnemySpawner _enemySpawner, PlayerGold _playerGold, Tile _ownerTile) //어쩔 때 Initialize같은 함수를 쓰는 걸까?, 외부에서 받은 정보를 설정하는 경우에 적합.
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.enemySpawner = _enemySpawner;
        this.playerGold = _playerGold;
        this.ownerTile = _ownerTile;
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if (attackTarget != null)
        {
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            float closetDistSqr = Mathf.Infinity;
            
            for (int i = 0; i < enemySpawner.EnemyList.Count - 1; i++)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

                if (distance <= towerTemplate.weapons[level].range && distance <= closetDistSqr)
                {
                    closetDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }

            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            
            float distance = Vector3.Distance(attackTarget.position, transform.position);

            if (distance > towerTemplate.weapons[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            
            yield return new WaitForSeconds(towerTemplate.weapons[level].rate);

            SpawnProjectile();
        }
    }

    private void SpawnProjectile() // 게임 오브젝트를 특정 위치에 생성하고 배치하는 경우에 적합
    {
        GameObject projectileObject = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectileObject.GetComponent<ProjectTile>().Setup(attackTarget, towerTemplate.weapons[level].damage);
    }

    public bool Upgrade()
    {
        if (playerGold.CurrentGold < towerTemplate.weapons[level + 1].cost)
        {
            return false;
        }
        
        level++;
        spriteRenderer.sprite = towerTemplate.weapons[level].sprite;
        playerGold.CurrentGold -= towerTemplate.weapons[level].cost;
        return true;
    }

    public void Sell()
    {
        playerGold.CurrentGold += towerTemplate.weapons[level].sell;
        ownerTile.IsBuildTower = false;
        Destroy(gameObject);
    }
}

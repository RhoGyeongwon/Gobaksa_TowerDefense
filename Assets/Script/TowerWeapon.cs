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
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float attakeRate = 0.5f; // 공격 주기
    [SerializeField] private float attakeRange = 2.0f; //공격범위
    [SerializeField] private int attackDamage = 1;
    private WeaponState weaponState = WeaponState.SearchTarget;

    private Transform attackTarget = null;
    private EnemySpawner enemySpawner;
    // Start is called before the first frame update

    public void Setup(EnemySpawner _enemySpawner) //어쩔 때 Initialize같은 함수를 쓰는 걸까?, 외부에서 받은 정보를 설정하는 경우에 적합.
    {
        this.enemySpawner = _enemySpawner;
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

                if (distance <= attakeRange && distance <= closetDistSqr)
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

            if (distance > attakeRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            
            yield return new WaitForSeconds(attakeRate);

            SpawnProjectile();
        }
    }

    private void SpawnProjectile() // 게임 오브젝트를 특정 위치에 생성하고 배치하는 경우에 적합
    {
        Debug.Log(attackDamage);
        GameObject projectileObject = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectileObject.GetComponent<ProjectTile>().Setup(attackTarget, attackDamage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType
{
    Kill = 0,
    Arrive
}
public class Enemy : MonoBehaviour
{
    [SerializeField] private int gold = 10;
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner;
    private EnemyDestroyType enemyDestroyType = EnemyDestroyType.Arrive;
    public void Setup(EnemySpawner _enemySpawner, Transform[] _wayPoints)
    {
        enemySpawner = _enemySpawner;
        movement2D = GetComponent<Movement2D>();
        wayPoints = _wayPoints;
        transform.position = wayPoints[currentIndex].position;
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward * 10);

            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02 * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        if (currentIndex < wayPoints.Length - 1)
        {
            ++currentIndex;
            Vector3 direction = wayPoints[currentIndex].position - transform.position;
            movement2D.MoveTo(direction.normalized);
        }
        else
        {
            gold = 0;
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        //EnemySpawner가 리스트로 적 정볼르 관리하기 때문에 직접 Destroy()를 하지 않는다.
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}

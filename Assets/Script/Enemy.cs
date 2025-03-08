using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;

    public void Setup(Transform[] _wayPoints)
    {
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
    }
}

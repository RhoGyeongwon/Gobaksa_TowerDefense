using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    public void OnAttackRange(Vector3 position, float range)
    {
        Debug.Log("켜졌나?");
        gameObject.SetActive(true);
        float diameter = range * 2.0f; //attackRange는 단일 방향이므로 지름으로 설정
        transform.localScale = Vector3.one * diameter;
        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}

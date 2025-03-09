using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class ProjectTile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    
    public void Setup(Transform _target, float _damage)
    {
        movement2D = GetComponent<Movement2D>();
        target = _target;
        damage = _damage;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            movement2D.MoveTo(direction);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.transform == target)
        {
            collision.GetComponent<EnemyHP>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

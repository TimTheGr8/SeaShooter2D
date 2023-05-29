using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.5f;
    [SerializeField]
    private float _targetRadius = 5.0f;
    [SerializeField]
    private float _rotationModifier = 0;

    // Update is called once per frame
    void Update()
    {
        CheckForTarget();
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.x > 11)
            Destroy(this.gameObject);
    }

    private void CheckForTarget()
    {
        GameObject closestTarget = null;
        float shortestDist = Mathf.Infinity;

        GameObject[] target = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in target)
        {
            if (enemy.tag == "Enemy")
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < shortestDist)
                {
                    shortestDist = distance;
                    closestTarget = enemy.gameObject;
                }
            }
        }
        if(closestTarget != null)
        {
            TargetPowerup(closestTarget);
        }
    }

    private void TargetPowerup(GameObject target)
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, (vectorToTarget.x)) * Mathf.Rad2Deg - _rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DestoryMe();
                Destroy(this.gameObject);
            }
        }
    }
}

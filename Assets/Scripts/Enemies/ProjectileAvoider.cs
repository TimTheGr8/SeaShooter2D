using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAvoider : MonoBehaviour
{
    [SerializeField]
    private float _checkRadius = 5.0f;
    [SerializeField]
    private float _rotationModifier = 0;
    [SerializeField]
    private float _turnSpeed = 2.5f;

    private bool _turnStarted = false;

    // Update is called once per frame
    void Update()
    {
        CheckforCannonball();   
    }

    private void CheckforCannonball()
    {
        Collider2D[] other = Physics2D.OverlapCircleAll(transform.position, _checkRadius);
        foreach (var hitObject in other)
        {
            if (hitObject.tag == "Cannon Ball")
            {
                CannonBall shot = hitObject.GetComponent<CannonBall>();
                if(shot != null && !shot.IsEnemyCannonball() && !_turnStarted)
                {
                    StartCoroutine(AvoidShot(hitObject.transform.position));
                }
            }
        }
    }

    private void TurnShip(Vector3 target)
    {
        Vector3 vectorToTarget = (target * -1) - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _turnSpeed);
    }

    IEnumerator AvoidShot(Vector3 target)
    {
        TurnShip(target);
        yield return new WaitForSeconds(0.5f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * _turnSpeed);
        _turnStarted = true;
        yield return new WaitForSeconds(0.5f);
        _turnStarted = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;


    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.x >= 11)
            Destroy(gameObject);
    }
}

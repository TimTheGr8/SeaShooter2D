using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move left at 4 meters per second
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // if left side of screen 
        // respawn on right side at random y location
        if(transform.position.x <= -9)
        {
            transform.position = new Vector3(11, Random.Range(-3f, 5.5f), 0);
        }
    }
}

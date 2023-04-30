using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _scoreValue = 5;

    private Player _player;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("There is no Player script.");
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.x <= -11)
        {
            transform.position = new Vector3(11, Random.Range(-3f, 5.5f), 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.DamagePlayer();
            Destroy(this.gameObject);
        }

        if (other.tag == "Cannon Ball")
        {
            _player.AddScore(_scoreValue);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}

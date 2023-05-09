using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _scoreValue = 5;
    [SerializeField]
    private AudioClip _explosionClip;

    private Player _player;
    private Animator _anim;
    private Collider2D _collider;
    private AudioSource _audioSource;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("There is no Player script.");
        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("There is no Animator on the enemy object.");
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
            Debug.LogError("There is no collider on the enemy.");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("There is no Audio Source on the Enemy.");
        else
            _audioSource.clip = _explosionClip;
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.x <= -11)
        {
            transform.position = new Vector3(11, Random.Range(-3f, 5.5f), 0);
        }
    }

    private void DestoryMe()
    {
        _speed = 0;
        _audioSource.Play();
        _anim.SetTrigger("OnEmenyDeath");
        _collider.enabled = false;
        Destroy(this.gameObject, 0.4f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.DamagePlayer();
            DestoryMe();
        }

        if (other.tag == "Cannon Ball")
        {
            _player.AddScore(_scoreValue);
            Destroy(other.gameObject);
            DestoryMe();
        }
    }

}

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
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private float _shieldProbability = 45;

    private Player _player;
    private Animator _anim;
    private Collider2D _collider;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private bool _hasShield;

    private void Start()
    {
        AssignComponents();
        ActivateShield();

        if (_hasShield)
            _shieldPrefab.SetActive(true);
        else
            _shieldPrefab.SetActive(false);
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.x <= -11)
        {
            transform.position = new Vector3(11, Random.Range(-3f, 5.5f), 0);
        }
    }

    private void AssignComponents()
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
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("There is no Spawn Manager.");
            //TODO: fix cannon sprite clean up
    }

    private void ActivateShield()
    {
        int probability = Random.Range(0, 101);

        _hasShield =  (probability <= _shieldProbability) ? true : false;
    }

    public void DestoryMe()
    {
        if(_hasShield)
        {
            _shieldPrefab.SetActive(false);
            _hasShield = false;
            return;
        }
        _speed = 0;
        _audioSource.Play();

        _anim.SetTrigger("OnEmenyDeath");
        _collider.enabled = false;
        _player.AddScore(_scoreValue);
        _spawnManager.RemoveEnemy();
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
            bool enemyShot = other.GetComponent<CannonBall>().IsEnemyCannonball();
            if (enemyShot == false)
            {
                Destroy(other.gameObject);
                DestoryMe();
            }
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}

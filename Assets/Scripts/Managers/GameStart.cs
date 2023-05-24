using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart: MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _explosionClip;

    private SpriteRenderer _renderer;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
            Debug.LogError("There is no Sprite Renderer on the Ghost Ship.");
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("Ghost ship did not find Spawn Manager.");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("There is no Audio Source on the Game Start.");
        else
            _audioSource.clip = _explosionClip;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Cannon Ball")
        {
            _audioSource.Play();
            _renderer.enabled = false;
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject,0.5f);
        }

        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.DamagePlayer();
            }
        }
    }
}

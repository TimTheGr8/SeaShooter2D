using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart: MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpriteRenderer _renderer;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
            Debug.LogError("There is no Sprite Renderer on the Ghost Ship.");
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("Ghost ship did not find Spawn Manager.");
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
            _renderer.enabled = false;
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
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

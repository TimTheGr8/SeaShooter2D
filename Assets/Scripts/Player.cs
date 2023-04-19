using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _projectilePrefab;
    [SerializeField]
    private Transform _projectileStartLoc;
    [SerializeField]
    private float _projectileCooldown = 0.5f;
    private bool _canShoot = true;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-7, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.Log("There is no Spawn Manager!!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ShootCannon();
        // Use this for drooping mines in the water.
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right mouse pressed.");
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // Flip movement directions so that the player moves the way intended
        Vector3 direction = new Vector3(-verticalInput, horizontalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.0f, 0.0f), transform.position.y, 0);

        if (transform.position.y >= 7.5f)
        {
            transform.position = new Vector3(transform.position.x, -5.5f, 0);
        }
        else if (transform.position.y <= -5.5f)
        {
            transform.position = new Vector3(transform.position.x, 7.5f, 0);
        }
    }

    void ShootCannon()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && _canShoot)
        {
            _canShoot = false;
            Instantiate(_projectilePrefab, _projectileStartLoc.position, transform.rotation);
            StartCoroutine(LaserCoolDown());
        }
    }

    public void DamagePlayer ()
    {
        _lives--;
        if(_lives <= 0 )
        {
            _spawnManager.OnPLayerDeath();
            Destroy(this.gameObject);
        }
    }

    IEnumerator LaserCoolDown ()
    {
        yield return new WaitForSeconds(_projectileCooldown);
        _canShoot = true;
    }
}

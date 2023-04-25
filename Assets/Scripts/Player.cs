using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _singleShotPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _tripleShotCannons;
    [SerializeField]
    private float _shotCooldown = 0.5f;
    [SerializeField]
    private float _tripleShotCoolDown = 5.0f;
    [SerializeField]
    private int _lives = 3;

    private bool _canShoot = true;
    private float _horizontalInput, _verticalInput;
    private SpawnManager _spawnManager;
    private SpriteRenderer _spriteRenderer;
    private bool _tripleShotEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-7, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
            Debug.Log("There is no Spawn Manager!!!!");

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.Log("There is no Sprite Renderer.");
        _tripleShotEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && _canShoot)
            ShootCannon();
    }

    private void MovePlayer()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        RotateShip();
        // Flip movement directions so that the player moves the way intended
        Vector3 direction = new Vector3(-_verticalInput, _horizontalInput, 0);
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

    private void RotateShip()
    {
        float zRotation = 0;
        if (_verticalInput >= 0.5f)
            zRotation = -75;
        else if (_verticalInput <= -0.5f)
            zRotation = -115;
        else
            zRotation = -90;

        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    void ShootCannon()
    {
        _canShoot = false;
        GameObject newCannonball;
        if (_tripleShotEnabled)
        {
            newCannonball = Instantiate(_tripleShotPrefab, transform.position, _spriteRenderer.transform.rotation);
        }
        else
        {
            newCannonball = Instantiate(_singleShotPrefab, new Vector3(transform.position.x + 1.3F, transform.position.y, 0), _spriteRenderer.transform.rotation);
        }
        
        _spawnManager.SetCannonballParent(newCannonball);
        StartCoroutine(CannonCoolDown());
    }

    public void DamagePlayer ()
    {
        _lives--;
        if(_lives <= 0 )
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _tripleShotEnabled = true;
        _tripleShotCannons.SetActive(true);
        StartCoroutine(TripleShotCountDown());
    }

    IEnumerator CannonCoolDown ()
    {
        yield return new WaitForSeconds(_shotCooldown);
        _canShoot = true;
    }

    IEnumerator TripleShotCountDown()
    {
        yield return new WaitForSeconds(_tripleShotCoolDown);
        _tripleShotEnabled = false;
        _tripleShotCannons.SetActive(false);
    }
}

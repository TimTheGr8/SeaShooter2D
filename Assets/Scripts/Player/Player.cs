using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _boostSpeed = 8.5f;
    [SerializeField]
    private float _windSpeed = 5.5f;
    [SerializeField]
    private float _windSpeedDepletion = 30.0f;
    [SerializeField]
    private Vector3 _startingPosition = new Vector3(-7, 0, 0);
    [SerializeField]
    private GameObject _singleShotPrefab;
    [SerializeField]
    private GameObject _bombPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _torpedoPrefab;
    [SerializeField]
    private GameObject _singleCannon;
    [SerializeField]
    private GameObject _tripleShotCannons;
    [SerializeField]
    private GameObject _bombCannon;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private float _shotCooldown = 0.5f;
    [SerializeField]
    private int _maxAmmoCount = 15;
    [SerializeField]
    private float _tripleShotCoolDown = 5.0f;
    [SerializeField]
    private float _speedBoostCoolDown = 2.5f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private Sprite _threeLivesRemaining, _twoLivesRemaining, _oneLifeRemaining;
    [SerializeField]
    private AudioClip _cannonFireClip;
    [SerializeField]
    private GameObject _explosionPrefab;

    private int _score = 0;
    private int _currentAmmo;
    private bool _canShoot = true;
    private float _horizontalInput, _verticalInput;
    private SpawnManager _spawnManager;
    private SpriteRenderer _spriteRenderer;
    private UIManager _uiManager;
    private bool _tripleShotEnabled = false;
    private bool _shieldActive = false;
    private SpriteRenderer _shieldRenderer;
    private int _shieldHits = 0;
    private float _currentSpeed;
    private AudioSource _audioSource;
    private GameObject _currentProjectile;
    private float _windSpeedTimer = 100f;
    private CameraShake _camShake;
    private GameManager _gm;

    void Start()
    {
        transform.position = _startingPosition;
        _currentSpeed = _speed;
        _currentAmmo = _maxAmmoCount;
        _currentProjectile = _singleShotPrefab;
        AssignComponents();
    }

    void Update()
    {
        if(!_gm.IsGameOver())
        {
            MovePlayer();
            if ((Input.GetKeyDown(KeyCode.Space)) && _canShoot)
                ShootCannon();
            // Turbo
            if (Input.GetKey(KeyCode.LeftShift) && _windSpeedTimer > 0)
            {
                WindSpeedBoost();
            }
            else
            {
                _currentSpeed = _speed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _currentSpeed = _speed;
                StartCoroutine(WindSpeedRecover());
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                AttractPowerups();
            }
        }
        
    }

    private void AssignComponents()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("There is no Spawn Manager!!!!");
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.LogError("There is no Sprite Renderer.");
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("There is no UI Manager");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("There is no Audio Source on the Player.");
        else
            _audioSource.clip = _cannonFireClip;
        _shieldRenderer = _playerShield.GetComponent<SpriteRenderer>();
        if (_shieldRenderer == null)
            Debug.LogError("There is no shield sprite renderer.");
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)
            Debug.LogError("There is no Camera Shake script.");
        _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gm == null)
            Debug.LogError("The player could not find the Game Manager.");
    }

    private void MovePlayer()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        RotateShip();
        // Flip movement directions so that the player moves the way intended
        Vector3 direction = new Vector3(-_verticalInput, _horizontalInput, 0);
        transform.Translate(direction * _currentSpeed * Time.deltaTime);
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
        if (_currentAmmo > 0)
        {
            _currentAmmo--;
            _canShoot = false;
            GameObject newCannonball;
            _audioSource.Play();
            if (_tripleShotEnabled)
            {
                newCannonball = Instantiate(_tripleShotPrefab, transform.position, _spriteRenderer.transform.rotation);
            }
            else
            {
                newCannonball = Instantiate(_currentProjectile, new Vector3(transform.position.x + 1.3F, transform.position.y, 0), _spriteRenderer.transform.rotation);
            }

            _spawnManager.SetCannonballParent(newCannonball);
            StartCoroutine(CannonCoolDown());
            _uiManager.UpdateAmmo(_currentAmmo, _maxAmmoCount);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        _uiManager.UpdateScore(_score);
    }

    public void DamagePlayer ()
    {
        if (_shieldActive && _shieldHits < 3)
        {
            _shieldHits++;
            UpdateShield();
            return;
        }

        _lives--;
        _camShake.ShakeCamera();
        _uiManager.RemoveLives(_lives);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        UpdatePlayerSprite();
        // Move player to starting position, blink, make it not take damage again until blink is done
    }

    public void AddHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.AddLives(_lives);
        }
        UpdatePlayerSprite();
    }

    private void UpdatePlayerSprite()
    {
        switch (_lives)
        {
            case 3:
                _spriteRenderer.sprite = _threeLivesRemaining;
                break;
            case 2:
                _spriteRenderer.sprite = _twoLivesRemaining;
                break;
            case 1:
                _spriteRenderer.sprite = _oneLifeRemaining;
                break;
            case 0:
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }

    public void ActivateTripleShot()
    {
        _tripleShotEnabled = true;
        _tripleShotCannons.SetActive(true);
        StartCoroutine(TripleShotCountDown());
    }

    public void ActivateBombs()
    {
        _singleCannon.SetActive(false);
        _bombCannon.SetActive(true);
        _currentProjectile = _bombPrefab;
        StartCoroutine(BombCountDown());
    }

    public void ActivateSpeedBoost()
    {
        _currentSpeed = _boostSpeed;
        StartCoroutine(SpeedBoostCoolDown());
    }

    public void ActivateShield()
    {
        _shieldRenderer.color = Color.green;
        _shieldHits = 0;
        _shieldActive = true;
        _playerShield.SetActive(true);
    }

    private void UpdateShield()
    {
        switch (_shieldHits)
        {
            case 1:
                _shieldRenderer.color = Color.yellow;
                break;
            case 2:
                _shieldRenderer.color = Color.red;
                break;
            case 3:
                DeactivateShields();
                break;
            default:
                Debug.LogError("The shield has taken too many hits.");
                break;
        }
    }
    private void DeactivateShields()
    {
        _shieldActive = false;
        _playerShield.SetActive(false);
    }

    public void ActivateTorpedo()
    {
        _currentProjectile = _torpedoPrefab;
        StartCoroutine(TorpedoCountDown());
    }

    private void AttractPowerups()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Powerup"))
        {
            Powerup powerup = item.GetComponent<Powerup>();
            if(powerup != null)
            {
                powerup.MoveToTarget(transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Cannon Ball" && !_gm.IsGameOver())
        {
            Destroy(other);
            DamagePlayer();
        }
    }

    public void AddAmmo()
    {
        _currentAmmo = _maxAmmoCount;
        _uiManager.UpdateAmmo(_currentAmmo, _maxAmmoCount);
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

    private void WindSpeedBoost()
    {
        _currentSpeed = _windSpeed;
        _windSpeedTimer -= _windSpeedDepletion * Time.deltaTime;
        _uiManager.UpdateWindSpeedGauge(_windSpeedTimer / 100);
    }

    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(_speedBoostCoolDown);
        _currentSpeed = _speed;
    }

    IEnumerator BombCountDown ()
    {
        yield return new WaitForSeconds(5);
        _bombCannon.SetActive(false);
        _singleCannon.SetActive(true);
        _currentProjectile = _singleShotPrefab;
    }

    IEnumerator WindSpeedRecover()
    {
        while (_currentSpeed != _windSpeed)
        {
            _windSpeedTimer += 10 * Time.deltaTime;
            if(_windSpeedTimer >= 100f)
            {
                _windSpeedTimer = 100f;
                break;
            }
            _uiManager.UpdateWindSpeedGauge(_windSpeedTimer / 100);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator TorpedoCountDown()
    {
        yield return new WaitForSeconds(2f);
        _currentProjectile = _singleShotPrefab;
    }
}

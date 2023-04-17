using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private Transform _laserStartLoc;
    [SerializeField]
    private float _laserCooldown = 0.5f;
    private bool _canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        // Take current position and assign it a start position
        transform.position = new Vector3(-7, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ShootLaser();
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.0f, -3.0f), transform.position.y, 0);

        if (transform.position.y >= 7.5f)
        {
            transform.position = new Vector3(transform.position.x, -5.5f, 0);
        }
        else if (transform.position.y <= -5.5f)
        {
            transform.position = new Vector3(transform.position.x, 7.5f, 0);
        }
    }

    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canShoot)
        {
            _canShoot = false;
            Instantiate(_laserPrefab, _laserStartLoc.position, Quaternion.identity);
            StartCoroutine(LaserCoolDown());
        }
    }

    IEnumerator LaserCoolDown ()
    {
        yield return new WaitForSeconds(_laserCooldown);
        _canShoot = true;
    }
}

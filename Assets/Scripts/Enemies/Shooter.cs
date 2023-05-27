using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject _canonnSprite;
    [SerializeField]
    private float _shotTimeMin = 1.0f;
    [SerializeField]
    private float _shotTimeMax = 1.5f;
    [SerializeField]
    private GameObject _cannonballPrefab;
    [SerializeField]
    private bool _shootPowerups = false;
    [SerializeField]
    private float _powerupCheckRadius = 2.5f;
    [SerializeField]
    private float _rotationModifier = 0;

    private SpawnManager _spawnManager;
    private bool _shootingPowerup = false;
    private float _currentShotMin, _currentShotMax;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("There is no Spawn Manager.");

        DoesShootPowerup();
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= -11)
        {
            StopAllCoroutines();
            StartCoroutine(Shoot());
        }

        CheckForPowerups();
    }

    private void CheckForPowerups()
    {
        GameObject closestTarget = null;
        float shortestDist = Mathf.Infinity;

        Collider2D[] other = Physics2D.OverlapCircleAll(transform.position, _powerupCheckRadius);
        foreach (var hitObject in other)
        {
            if (hitObject.tag == "Powerup" && hitObject.transform.position.x < transform.position.x)
            {
                float distance = Vector3.Distance(hitObject.transform.position, transform.position);
                if (distance < shortestDist)
                {
                    shortestDist = distance;
                    closestTarget = hitObject.gameObject;
                }

            }
        }
        if (closestTarget != null)
        {
            _currentShotMin = 0.5f;
            _currentShotMax = 0.75f;
            TargetPowerup(closestTarget);
        }
        else
        {
            _currentShotMin = _shotTimeMin;
            _currentShotMax = _shotTimeMax;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private void DoesShootPowerup()
    {
        int randNumber = Random.Range(1, 10);
        _shootPowerups = (randNumber <= 5) ? true : false;
    }

    private void TargetPowerup(GameObject target)
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2.5f);
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_currentShotMin, _currentShotMax));
            GameObject cannonball = Instantiate(_cannonballPrefab, new Vector3(transform.position.x - 0.25f, transform.position.y, transform.position.z), transform.rotation);
            _spawnManager.SetCannonballParent(cannonball);
        }
    }
}

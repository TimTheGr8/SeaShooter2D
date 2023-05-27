using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _cannonSprite;
    [SerializeField]
    private float _shotTimeMin = 1.0f;
    [SerializeField]
    private float _shotTimeMax = 1.5f;
    [SerializeField]
    private GameObject _cannonballPrefab;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("There is no Spawn Manager.");
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
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_shotTimeMin, _shotTimeMax));
            GameObject cannonball = Instantiate(_cannonballPrefab, new Vector3(transform.position.x - 1.03f, transform.position.y, transform.position.z), transform.rotation);
            _spawnManager.SetCannonballParent(cannonball);
        }
    }
}

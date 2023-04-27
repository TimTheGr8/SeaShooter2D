using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyPrefab = new List<GameObject>();
    [SerializeField]
    private float _spawnTimer = 3.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _cannonballContainer;
    [SerializeField]
    private GameObject _powerupPrefab;

    private bool _spawn = true;


    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    public void OnPlayerDeath()
    {
        _spawn = false;
    }

    public void SetCannonballParent(GameObject cannonball)
    {
        cannonball.transform.parent = _cannonballContainer.transform;
    }

    IEnumerator SpawnEnemies()
    {
        while (_spawn)
        {
            int randomEnemy = Random.Range(0, _enemyPrefab.Count);
            GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], new Vector3(11, Random.Range(-3f, 5.5f), 0), _enemyPrefab[randomEnemy].transform.rotation);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    IEnumerator SpawnPowerups()
    {
        while (_spawn)
        {
            int randomTime = Random.Range(3, 8);
            GameObject newPowerup = Instantiate(_powerupPrefab, new Vector3(11, Random.Range(-3f, 5.5f), 0), Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
    }
}

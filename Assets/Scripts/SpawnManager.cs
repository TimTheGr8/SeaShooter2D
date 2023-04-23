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
    private GameObject _EnemyContainer;
    [SerializeField]
    private GameObject _cannonballContainer;
    private bool _spawnEneimes = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void OnPlayerDeath()
    {
        _spawnEneimes = false;
    }

    public void SetCannonballParent(GameObject cannonball)
    {
        cannonball.transform.parent = _cannonballContainer.transform;
    }

    IEnumerator SpawnEnemies()
    {
        while (_spawnEneimes)
        {
            int randomEnemy = Random.Range(0, _enemyPrefab.Count);
            GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], new Vector3(11, Random.Range(-3f, 5.5f), 0), _enemyPrefab[randomEnemy].transform.rotation);
            newEnemy.transform.parent = _EnemyContainer.transform;
            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}

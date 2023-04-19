using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private float _spawnTimer = 3.0f;
    [SerializeField]
    private int _enemyCount = 0;
    [SerializeField]
    private GameObject _EnemyContainer;
    private bool _spawnEneimes = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void OnPLayerDeath()
    {
        _spawnEneimes = false;
    }

    IEnumerator SpawnEnemies()
    {
        while (_spawnEneimes)
        {
            GameObject newEnemy =  Instantiate(_enemyPrefab, new Vector3(11, Random.Range(-3f, 5.5f), 0), _enemyPrefab.transform.rotation);
            newEnemy.transform.SetParent(_EnemyContainer.transform);
            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}

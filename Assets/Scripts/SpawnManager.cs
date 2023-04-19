using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private float _spwanTimer = 3.0f;
    [SerializeField]
    private int _enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        int _enemiesSpawned = 0;
        while (true)
        {
            Instantiate(_enemyPrefab, new Vector3(11, Random.Range(-3f, 5.5f), 0), _enemyPrefab.transform.rotation);
            yield return new WaitForSeconds(_spwanTimer);
        }
    }
}

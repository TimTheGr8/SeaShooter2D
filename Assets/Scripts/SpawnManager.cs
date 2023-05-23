using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyPrefab = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _powerupPrefab = new List<GameObject>();
    [NamedArrayAttribute(new string[] { "Ammo", "TNT", "Wind", "Bomb", "Triple Shot", "Shield", "Health" })]
    [SerializeField]
    private int[] _dropTable =
    {
        55, // Ammo
        13, // TnT
        10, // Wind
        8, // Bomb
        7, // Triple
        5, // Shield
        2, // Health
    };
    [SerializeField]
    private float _spawnTimer = 3.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _cannonballContainer;
    

    private bool _spawn = true;
    private int _enemyCount = 0;
    private int _enemiesToSpawn;
    private int _dropTableTotal;

    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (gm == null)
            Debug.LogError("There is no Game Manager");
        TotalDropTable();
        _spawn = true;
    }

    public void OnPlayerDeath()
    {
        _spawn = false;
        StopAllCoroutines();
    }

    public void SetCannonballParent(GameObject cannonball)
    {
        cannonball.transform.parent = _cannonballContainer.transform;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerups());
    }

    public void RemoveEnemy()
    {
        _enemyCount--;
        if (_enemyCount == 0 && _enemiesToSpawn == gm.WaveEnemyCount())
        {
            StopCoroutine(SpawnEnemies());
            gm.NextWave();
            _enemiesToSpawn = 0;
            StartCoroutine(SpawnEnemies());
        }
    }

    private void TotalDropTable()
    {
        foreach (var item in _dropTable)
        {
            _dropTableTotal += item;
        }

        if (_dropTableTotal > 100)
            Debug.LogError("The Drop Table total is greater than 100.");
        else if (_dropTableTotal < 100)
        {
            Debug.LogError("The Drop Table total is less than 100.");
        }
    }

    private int RandomDrop()
    {
        int randomDrop = Random.Range(0, _dropTableTotal);
        int itemToDrop = 9;
        Debug.Log($"Random Number inital: {randomDrop}");
        for (int i = 0; i < _dropTable.Length; i++)
        {
            if (randomDrop <= _dropTable[i])
            {
                Debug.Log($"Drop Table Index {i} : {_dropTable[i]}");
                itemToDrop = i;
                break;
            }
            else
            {
                randomDrop -= _dropTable[i];
            }
        }
        Debug.Log($"Random Number: {randomDrop}\nDropping element: {itemToDrop}");
        return itemToDrop;
    }


    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(1.5f);
        while (_spawn && _enemiesToSpawn < gm.WaveEnemyCount())
        {
            int randomEnemy = Random.Range(0, _enemyPrefab.Count);
            GameObject newEnemy = Instantiate(_enemyPrefab[randomEnemy], new Vector3(11, Random.Range(-3f, 5.5f), 0), _enemyPrefab[randomEnemy].transform.rotation);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesToSpawn++;
            _enemyCount++;
            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    IEnumerator SpawnPowerups()
    {
        yield return new WaitForSeconds(3.0f);
        while (_spawn)
        {
            int randomTime = Random.Range(3, 8);
            GameObject newPowerup = Instantiate(_powerupPrefab[RandomDrop()], new Vector3(11, Random.Range(-3f, 5.5f), 0), Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
    }
}

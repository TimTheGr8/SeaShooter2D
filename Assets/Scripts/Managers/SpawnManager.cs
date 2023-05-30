using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyPrefab = new List<GameObject>();

    [SerializeField]
    private List<Wave> _waves = new List<Wave>();

    [SerializeField]
    private List<GameObject> _powerupPrefab = new List<GameObject>();
    [NamedArrayAttribute(new string[] { "Ammo", "TNT", "Wind", "Bomb", "Triple Shot", "Shield", "Health", "Torpedo" })]
    [SerializeField]
    private int[] _dropTable =
    {
        55, // Ammo
        13, // TnT
        10, // Wind
        8, // Bomb
        7, // Triple
        4, // Shield
        2, // Health
        1  // Torpedo
    };
    [SerializeField]
    private float _spawnTimer = 3.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _cannonballContainer;
    

    private bool _spawnEnemies = true;
    private bool _spawnPowerups = true;
    private int _enemyCount = 0;
    private int _dropTableTotal;
    private int _currentWave;

    private GameManager _gm;

    void Start()
    {
        _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gm == null)
            Debug.LogError("There is no Game Manager");
        TotalDropTable();
        _spawnEnemies = true;
    }

    public void OnPlayerDeath()
    {
        _spawnEnemies = false;
        _spawnPowerups = false;
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
        if (_enemyCount == 0 && !_spawnEnemies)
        {
            StopCoroutine(SpawnEnemies());
            _gm.NextWave();
            _spawnEnemies = true;
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
        for (int i = 0; i < _dropTable.Length; i++)
        {
            if (randomDrop <= _dropTable[i])
            {
                itemToDrop = i;
                break;
            }
            else
            {
                randomDrop -= _dropTable[i];
            }
        }
        return itemToDrop;
    }


    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(1.5f);
        while(_spawnEnemies)
        {
            var currentWave = _waves[(_gm.WaveCount() - 1)].GetEnemies();
            foreach (var obj in currentWave)
            {
                GameObject newEnemy = Instantiate(obj, new Vector3(11, Random.Range(-3f, 5.5f), 0), obj.transform.rotation);
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemyCount++;
                yield return new WaitForSeconds(_spawnTimer);
            }
            _spawnEnemies = false;
        }
    }

    IEnumerator SpawnPowerups()
    {
        yield return new WaitForSeconds(3.0f);
        while (_spawnPowerups)
        {
            int randomTime = Random.Range(3, 8);
            GameObject newPowerup = Instantiate(_powerupPrefab[RandomDrop()], new Vector3(11, Random.Range(-3f, 5.5f), 0), Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
    }
}

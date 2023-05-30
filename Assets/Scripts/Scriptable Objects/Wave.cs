using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWave.asset", menuName = "Scriptable Objects/New Wave")]
public class Wave : ScriptableObject
{
    [SerializeField]
    private List<GameObject> _enemies;

    public List<GameObject> GetEnemies()
    {
        return _enemies;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    [SerializeField]
    private float _ramSpeed = 8;
    [SerializeField]
    private float _ramDistance = 2.5f;
    [SerializeField]
    private float _rotationModifier = 0;
    [SerializeField]
    private float _turnSpeed = 2.5f;

    private Player _player;
    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("AggressiveEnemy cannot find the Player");
        _enemy = GetComponent<Enemy>();
        if (_enemy == null)
            Debug.LogError("AggressiveEnemy cannot find Enemy.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= _ramDistance)
        {
            RamPlayer();
        }
    }

    private void RamPlayer()
    {
        Vector3 vectorToTarget = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _turnSpeed);

        _enemy.SetSpeed(_ramSpeed);
    }

}

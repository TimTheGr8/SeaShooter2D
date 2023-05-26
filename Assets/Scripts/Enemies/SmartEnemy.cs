using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _dropable;

    private GameObject _player;
    private bool _bombDropped = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
            Debug.LogError("The Smart Enemy could not find the player.");
    }

    // Update is called once per frame
    void Update()
    {
        if(IsBehindPlayer() && !_bombDropped)
        {
            DropBomb();
        }

        if (!IsBehindPlayer())
            _bombDropped = false;
    }

    private bool IsBehindPlayer()
    {
        bool behindPlayer = _player.transform.position.x > transform.position.x ? true : false;

        return behindPlayer;
    }

    private void DropBomb()
    {
        Instantiate(_dropable, transform.position, Quaternion.identity);
        _bombDropped = true;
    }
}

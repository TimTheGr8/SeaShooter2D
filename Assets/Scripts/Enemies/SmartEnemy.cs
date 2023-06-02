using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _dropable;

    private GameObject _player;
    private bool _bombDropped = false;
    private GameManager _gm;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
            Debug.LogError("The Smart Enemy could not find the player.");
        _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gm == null)
            Debug.LogError("The Smart Enemy cannot find the Game Manager.");
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gm.IsGameOver() && IsBehindPlayer() && !_bombDropped)
        {
            DropBomb();
        }

        if (!_gm.IsGameOver() && !IsBehindPlayer())
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

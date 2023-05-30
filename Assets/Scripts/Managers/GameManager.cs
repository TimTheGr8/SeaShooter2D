using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _startingWaveCount = 5;
    [SerializeField]
    private float _waveCountMultiplyer = 1.5f;

    private bool _isGameOver = false;
    private int _wave = 1;
    private int _waveEnemyCount;

    private void Awake()
    {
        _isGameOver = false;
        _waveEnemyCount = _startingWaveCount;
    }

    private void Update()
    {
        if( _isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void NextWave()
    {
        _wave++;
        if(_wave == 5)
        {
            GameOver();
        }
    }

    public int WaveCount()
    {
        return _wave;
    }
}

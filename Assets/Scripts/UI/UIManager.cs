using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _ammoText;
    [SerializeField]
    private List<Image> _lives = new List<Image>();
    [SerializeField]
    private Sprite _lifeSprite;
    [SerializeField]
    private Sprite _deathSprite;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("There is no Game MAnager in the scene.");

        _scoreText.text = $"Score: 0";
        for (int i = 0; i < _lives.Count; i++)
        {
            _lives[i].sprite = _lifeSprite;
        }
        _gameOverText.SetActive(false);
        _restartText.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = $"Ammo: {ammo}";
    }

    public void UpdateLives(int livesRemaining)
    {
        _lives[livesRemaining].sprite = _deathSprite;
        if (livesRemaining == 0)
        {
            StartCoroutine(Flicker());
            _restartText.SetActive(true);
            _gameManager.GameOver(); 
        }
    }

   IEnumerator Flicker()
    {
        while(true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}

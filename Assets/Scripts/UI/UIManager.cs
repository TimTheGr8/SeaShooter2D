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
    [SerializeField]
    private Image _windSpeedGauge;
    [SerializeField]
    private Image _bossHealthBar;
    [SerializeField]
    private Sprite _bossLifefull, _bossLifeMed, _bossLifeLow;
    [SerializeField]
    private GameObject _bossDefeatedMessage;

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

    public void UpdateAmmo(int ammo, int ammoTotal)
    {
        _ammoText.text = $"Ammo: {ammo}/{ammoTotal}";
    }

    public void AddLives(int livesRemaining)
    {
        for (int i = 0; i < livesRemaining; i++)
        {
            _lives[livesRemaining - 1].sprite = _lifeSprite;
        }
    }

    public void RemoveLives(int livesRemaining)
    {
        _lives[livesRemaining].sprite = _deathSprite;
        if (livesRemaining == 0)
        {
            StartCoroutine(Flicker());
            _restartText.SetActive(true);
            _gameManager.GameOver();
        }
    }

    public void UpdateWindSpeedGauge(float timeRemaining)
    {
        _windSpeedGauge.fillAmount = timeRemaining;
    }

    public void EnableBossLife()
    {
        _bossHealthBar.transform.parent.gameObject.SetActive(true);
    }

    public void EnableBossDefeatedMessage()
    {
        StartCoroutine(Flicker());
        _bossDefeatedMessage.SetActive(true);
        _restartText.SetActive(true);
        _gameManager.GameOver();
    }

    public void UpdateBossHealth(float healthRemaining)
    {
        _bossHealthBar.fillAmount = healthRemaining;
        switch (healthRemaining)
        {
            case <= 0.3f:
                _bossHealthBar.sprite = _bossLifeLow;
                break;
            case <= 0.6f:
                _bossHealthBar.sprite = _bossLifeMed;
                break;
            default:
                break;
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

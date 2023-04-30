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
    private List<Image> _lives = new List<Image>();
    [SerializeField]
    private Sprite _lifeSprite;
    [SerializeField]
    private Sprite _deathSprite;

    void Start()
    {
        _scoreText.text = $"Score: 0";
        for (int i = 0; i < _lives.Count; i++)
        {
            _lives[i].sprite = _lifeSprite;
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void UpdateLives(int livesRemaining)
    {
        _lives[livesRemaining].sprite = _deathSprite;
    }
}

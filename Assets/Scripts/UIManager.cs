using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;

    void Start()
    {
        
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel,levelCompletePanel;
    [SerializeField] private GamePlaySO gamePlaySo;
    [SerializeField] private TMP_Text leftScoreText,nextLevelText;
    [SerializeField] private Image fillImage;
    
    private void OnEnable()
    {
        gamePlaySo.OnGameOver += OnGameOver;
        gamePlaySo.OnUpdateUI += UpdateUI;
        gamePlaySo.OnNewLevel += OnNewLevel;
        gamePlaySo.OnRestart += OnRestart;
    }
    
    private void OnDisable()
    {
        gamePlaySo.OnGameOver -= OnGameOver;
        gamePlaySo.OnUpdateUI -= UpdateUI;
        gamePlaySo.OnNewLevel -= OnNewLevel;
        gamePlaySo.OnRestart -= OnRestart;
    }

    private void OnLevelComplete()
    {
        levelCompletePanel.SetActive(true);
    }

    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    private void UpdateUI()
    {
        var targetScore = ScoreManager.Instance.targetScore;
        var currentScore = ScoreManager.Instance.currentScore;
        var leftScore = targetScore - currentScore;
        if (leftScore < 0) leftScore = 0;
        var fillAmount = (float)currentScore / targetScore;
        
        leftScoreText.text = $"Score left : {leftScore} ";
        fillImage.fillAmount = fillAmount;
        nextLevelText.text = $"{gamePlaySo.currentLevel + 1}";
        
    }
    
    private void OnNewLevel()
    {
        UpdateUI();
    }
    
    private void OnRestart()
    {
        UpdateUI();
    }
}

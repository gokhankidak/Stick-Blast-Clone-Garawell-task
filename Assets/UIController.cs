using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GamePlaySO gamePlaySo;
    
    private void OnEnable()
    {
        gamePlaySo.OnGameOver += OnGameOver;
    }
    
    private void OnDisable()
    {
        gamePlaySo.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    private void OnNewLevel()
    {
        
    }
    
    private void OnRestart()
    {
        
    }
}

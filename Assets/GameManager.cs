using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GamePlaySO gamePlaySO;

    private void Awake()
    {
        ServiceLocator.Register(this);
        gamePlaySO.currentLevel = 1;
    }

    public void RestartGame()
    {
        gamePlaySO.OnRestart?.Invoke();
    }

    public void NextLevel()
    {
        gamePlaySO.currentLevel++;
        gamePlaySO.OnNewLevel?.Invoke();
    }
    
}

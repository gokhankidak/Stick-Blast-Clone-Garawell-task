using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GamePlaySO gamePlaySO;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    public void RestartGame()
    {
        gamePlaySO.OnRestart?.Invoke();
    }
    
}

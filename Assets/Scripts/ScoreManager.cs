using System;
using System.Threading.Tasks;
using DamageNumbersPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    [HideInInspector] public int currentScore,targetScore;

    [SerializeField] private GamePlaySO gamePlaySO;
    [SerializeField] private DamageNumberMesh damageNumberPrefab,comboPrefab;
    
    private int comboCount = 0;
    private int comboProgressCount = 0;
    
    
    private void OnEnable()
    {
        gamePlaySO.OnNewLevel += ResetValues;
        gamePlaySO.OnRestart += ResetValues;
        gamePlaySO.OnPiecePlaced += CalculateComboProgress;
    }
    
    private void OnDisable()
    {
        gamePlaySO.OnNewLevel -= ResetValues;
        gamePlaySO.OnRestart -= ResetValues;
        gamePlaySO.OnPiecePlaced -= CalculateComboProgress;
    }
    
    private void CalculateComboProgress()
    {
        Debug.Log("Combo Progress Count: " + comboProgressCount);
        comboProgressCount--;
        if (comboProgressCount <= 0)
        {
            comboCount = 0;
            comboProgressCount = 0;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }
    
    private void Start()
    {
        ResetValues();
        gamePlaySO.OnUpdateUI?.Invoke();
    }
    
    private void ResetValues()
    {
        comboCount = 0;
        comboProgressCount = 0;
        currentScore = 0;
        targetScore = 500;
    }
    
    public async void ShowScoreNumber(Vector3 position, float amount,bool isCombo = false)
    {
        if (comboCount > 1) amount += comboCount * 10;
        
        damageNumberPrefab.Spawn(position, amount);
        currentScore += (int)amount;
        await Task.Delay(500);
        gamePlaySO.OnUpdateUI?.Invoke();
        
        if (currentScore >= targetScore)
        {
            gamePlaySO.OnLevelComplete?.Invoke();
        }

        if (!isCombo) return;
        comboProgressCount = 3;
        comboCount++;
        
        if (comboCount > 1)
        {
            comboPrefab.Spawn(Vector3.zero, comboCount);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Square : MonoBehaviour
{
    [HideInInspector] public bool isActive;
    [SerializeField] private List<Stick> surroundingSticks;
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration;
    [SerializeField] private GamePlaySO gamePlaySo;
    [SerializeField] private ParticleSystem particleSystem;

    SquareManager squareManager;
    
    private void OnEnable()
    {
        gamePlaySo.OnPiecePlaced += CheckForSquares;
        gamePlaySo.OnRestart += ClearSquare;
        
    }
    
    private void OnDisable()
    {
        gamePlaySo.OnPiecePlaced -= CheckForSquares;
        gamePlaySo.OnRestart -= ClearSquare;
    }
    
    private void CheckForSquares()
    {
        if (isActive) return;
        foreach (var stick in surroundingSticks)
        {
            if (!stick.isOccupied)
            {
                return;
            }
        }
        SetActive();
    }
    
    public bool CheckForHighlight()
    {
        bool hasAtLeastOneNotOccupied = false;
        if (isActive) return true;
        foreach (var stick in surroundingSticks)
        {
            if (!stick.isOccupied) hasAtLeastOneNotOccupied = true;
            
            if (!stick.isScoreHighlighted && !stick.isOccupied)
            {
                return false;
            }
        }
        if (!hasAtLeastOneNotOccupied) return false;
        
        foreach (var stick in surroundingSticks)
        {
            stick.ScoreHighlight(true);
        }
        
        return true;
    }

    private void Start()
    {
        SetInactive();
        transform.localScale = Vector3.zero;
        squareManager = ServiceLocator.Get<SquareManager>();
    }

    public void SetInactive()
    {
        isActive = false;
    }

    private void ClearSquare()
    {
        isActive = false;
        transform.localScale = Vector3.zero;
    }

    public List<Stick> GetSurroundingSticks()
    {
        return surroundingSticks;
    }
    
    public void OnScored()
    {
        particleSystem.Play();
        transform.DOScale(Vector3.zero, duration).SetEase(easeType);
        SetInactive();
    }

    public void SetActive()
    {
        isActive = true;
        transform.localScale = Vector3.zero;
        ScoreManager.Instance.ShowScoreNumber( transform.position, 10,true);
        transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }
}

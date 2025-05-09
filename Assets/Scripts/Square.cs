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

    SquareController squareController;
    
    private void OnEnable()
    {
        gamePlaySo.OnPiecePlaced += CheckForSquares;
    }
    
    private void OnDisable()
    {
        gamePlaySo.OnPiecePlaced -= CheckForSquares;
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

    private void Start()
    {
        SetInactive();
        transform.localScale = Vector3.zero;
        squareController = ServiceLocator.Get<SquareController>();
    }

    public void SetInactive()
    {
        isActive = false;
    }

    public void ClearSquare()
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
        transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }
}

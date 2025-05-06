using System;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    [SerializeField] private GamePlaySO gamePlaySO;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }
    
    public void OnPieceSelected(Piece piece)
    {
        gamePlaySO.selectedPiece = piece;
        gamePlaySO.OnPieceSelected?.Invoke();
    }
    public void OnPieceDropped()
    {
        gamePlaySO.OnPieceDroped?.Invoke();
        gamePlaySO.selectedPiece = null;
    }   
}

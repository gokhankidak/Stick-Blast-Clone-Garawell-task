using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GamePlaySO", menuName = "ScriptableObjects/GamePlaySO")]
public class GamePlaySO : ScriptableObject
{
    [HideInInspector] public Piece selectedPiece;
    public Action OnPieceSelected;
    public Action OnPieceDroped;
    public Action OnPiecePlaced;
}

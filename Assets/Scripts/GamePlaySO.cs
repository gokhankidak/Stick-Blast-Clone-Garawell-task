using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GamePlaySO", menuName = "ScriptableObjects/GamePlaySO")]
public class GamePlaySO : ScriptableObject
{
    public int currentLevel = 0;
    
    [HideInInspector] public Piece selectedPiece;
    public Action OnPieceSelected;
    public Action OnPieceDroped;
    public Action OnPiecePlaced;
    public Action OnScored;
    public Action OnNewLevel;
    public Action OnGameOver;
    public Action OnRestart;
    public Action OnUpdateUI;
    public Action OnLevelComplete;

}

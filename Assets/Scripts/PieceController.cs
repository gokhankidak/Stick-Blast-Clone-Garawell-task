using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    [SerializeField] private GamePlaySO gamePlaySO;
    [SerializeField] private List<Piece> pieces;
    [SerializeField] private Transform[] spawnPos = new Transform[3];
    
    private List<Piece> spawnedPieces = new List<Piece>();

    private void Start()
    {
        SpawnSticks();
    }

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
    
    public void OnPieceDestroyed(Piece piece)
    {
        spawnedPieces.Remove(piece);
        if(spawnedPieces.Count == 0) SpawnSticks();
    }
    
    private void SpawnSticks()
    {
        for (int i = 0; i < 3; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, pieces.Count);
            var stick = Instantiate(pieces[randomIndex], spawnPos[i].position,Quaternion.identity,transform);
            spawnedPieces.Add(stick);
        }
    }
}

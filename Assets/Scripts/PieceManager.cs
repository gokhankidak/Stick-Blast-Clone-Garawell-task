using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private GamePlaySO gamePlaySO;
    [SerializeField] private List<Piece> pieces;
    [SerializeField] private Transform[] spawnPos = new Transform[3];
    
    private List<Piece> spawnedPieces = new List<Piece>();
    private GridController gridController;

    private void OnEnable()
    {
        gamePlaySO.OnPiecePlaced += CheckForFail;
        gamePlaySO.OnRestart += RestartGame;
    }
    
    private void OnDisable()
    {
        gamePlaySO.OnPiecePlaced -= CheckForFail;
        gamePlaySO.OnRestart -= RestartGame;
    }

    private void Start()
    {
        SpawnSticks();
        gridController = ServiceLocator.Get<GridController>();
    }

    private void Awake()
    {
        ServiceLocator.Register(this);
    }
    
    private void RestartGame()
    {
        for (int i = 0; i < spawnedPieces.Count; i++)
        {
            Destroy(spawnedPieces[i].gameObject);
        }
        spawnedPieces.Clear();
        SpawnSticks();
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
    
    private async void CheckForFail()
    {
        await Task.Delay(1000);
        var canPlace = false;
        foreach (var piece in spawnedPieces)
        {
            var can = gridController.CanPlace(piece);
            canPlace = can || canPlace;
        }

        if (!canPlace)
        {
            Debug.Log("Game Over");
            gamePlaySO.OnGameOver?.Invoke();
        }
    }
}

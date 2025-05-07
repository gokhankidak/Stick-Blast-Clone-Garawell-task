using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SquareController : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private List<SquareArray> squares = new List<SquareArray>();
    [SerializeField] private GamePlaySO gamePlaySO;
    private HashSet<Square> scoredSquares = new HashSet<Square>();

    private void OnEnable()
    {
        gamePlaySO.OnPiecePlaced += CheckForMatch;
    }
    
    private void OnDisable()
    {
        gamePlaySO.OnPiecePlaced -= CheckForMatch;
    }

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private async void CheckForMatch()
    {
        await Task.Delay(300);
        var scoredRow = new List<Square>();
        var scoredColumn = new List<Square>();
        for (int i = 0; i < squares.Count; i++)
        {
            for (int j = 0; j < squares[i].squares.Length; j++)
            {
                if (squares[i].squares[j].isActive)
                {
                    scoredRow.Add(squares[i].squares[j]);
                }
                else
                {
                    scoredRow = new List<Square>();
                    break;
                }
                if(scoredRow.Count == squares[i].squares.Length)
                    scoredSquares.AddRange(scoredRow);
            }
        }

        for (int j = 0; j < squares[0].squares.Length; j++)
        {
            for (int i = 0; i < squares.Count; i++)
            {
                if (squares[i].squares[j].isActive)
                {
                    scoredColumn.Add(squares[i].squares[j]);
                }
                else
                {
                    scoredColumn = new List<Square>();
                    break;
                }
                if(scoredColumn.Count == squares[i].squares.Length)
                    scoredSquares.AddRange(scoredColumn);
            }
        }

        if(scoredSquares.Count == 0) return;
        var surroundingSticks = new HashSet<Stick>();
        foreach (var scoredSquare in scoredSquares)
        { 
            await Task.Delay(100);
            scoredSquare.OnScored();
            surroundingSticks.AddRange(scoredSquare.GetSurroundingSticks());
        }
        
        foreach (var surroundingStick in surroundingSticks)
        {
            surroundingStick.OnScored();
        }
        
        scoredSquares.Clear();
    }
}

[Serializable]
public struct SquareArray
{
    public Square[] squares;
}
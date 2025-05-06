using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridController : MonoBehaviour
{
    [SerializeField] private SticksRow[] gridRowSticks;
    [SerializeField] private Color highlightColor, occupiedColor, emptyColor;
    [SerializeField] private GamePlaySO gamePlaySO;
    [SerializeField] private float snapDistance = 0.25f;

    private Piece selectedPiece;
    private Transform referencePoint;
    private Vector2Int snapIndex;
    private bool isReferenceVertical;
    private bool[,] gridOccupied = new bool[9, 5];

    private void Awake()
    {
        ServiceLocator.Register(this);
    }
    

    private void Start()
    {
        for (int i = 0; i < gridOccupied.GetLength(0); i++)
        {
            for (int j = 0; j < gridOccupied.GetLength(1); j++)
            {
                gridOccupied[i, j] = false;
            }
        }
        ReDrawGrid();
    }

    private void ReDrawGrid()
    {
        for (int i = 0; i < gridRowSticks.Length; i++)
        {
            for (int j = 0; j < gridRowSticks[i].sticks.Count; j++)
            {
                Stick stick = gridRowSticks[i].sticks[j];
                if (gridOccupied[i, j])
                {
                    stick.SetColor(occupiedColor);
                }
                else
                {
                    stick.SetColor(emptyColor);
                }
            }
        }
    }

    private void PlacePiece()
    {
        if(snapIndex.x == -1 || snapIndex.y == -1) return;
        
        for (int x = 0; x < selectedPiece.gridRows.Count; x++)
        {
            for (int y = 0; y < selectedPiece.gridRows[x].sticks.Count; y++)
            {
                gridOccupied[snapIndex.x + x, snapIndex.y + y] = true;
                gridRowSticks[snapIndex.x + x].sticks[snapIndex.y + y].SetColor(occupiedColor);
            }
        }
        selectedPiece.DestroyPiece();
    }
    
    private void OnEnable()
    {
        gamePlaySO.OnPieceSelected += OnStickSelected;
        gamePlaySO.OnPieceDroped += OnStickDroped;
    }

    private void OnDisable()
    {
        gamePlaySO.OnPieceSelected -= OnStickSelected;
        gamePlaySO.OnPieceDroped -= OnStickDroped;
    }

    private void OnStickSelected()
    {
        selectedPiece = gamePlaySO.selectedPiece;
        referencePoint = selectedPiece.transform;
    }

    private void CheckForSnap()
    {
        Vector2Int closestIndex = new Vector2Int(-1, -1);
        float closestDistance = Mathf.Infinity;
        var startIndex = selectedPiece.isReferenceVertical ? 1 : 0;
        for (int i = startIndex; i < gridRowSticks.Length; i += 2)
        {
            for (int j = 0; j < gridRowSticks[i].sticks.Count; j++)
            {
                Stick stick = gridRowSticks[i].sticks[j];
                var distance = Vector3.Distance(stick.transform.position, referencePoint.position);

                if (distance > snapDistance) continue;
                ReDrawGrid();
                if (CanSnap(new Vector2Int(i, j)))
                {
                    if(closestDistance > distance) 
                    {
                        closestDistance = distance;
                        closestIndex = new Vector2Int(i, j);
                        snapIndex = closestIndex;
                    }
                }
            }
        }
        if (closestIndex.x == -1 || closestIndex.y == -1)
        {
            snapIndex = new Vector2Int(-1, -1);
            ReDrawGrid();
            return;
        }
        HighlightSnap(closestIndex.x, closestIndex.y);
    }
    private void HighlightSnap(int i, int j)
    {
        for (int x = 0; x < selectedPiece.gridRows.Count; x++)
        {
            for (int y = 0; y < selectedPiece.gridRows[x].sticks.Count; y++)
            {
                gridRowSticks[i + x].sticks[j + y].SetColor(highlightColor);
            }
        }
    }

    private bool CanSnap(Vector2Int index)
    {
        for (int i = 0; i < selectedPiece.gridRows.Count; i++)
        {
            for (int j = 0; j < selectedPiece.gridRows[i].sticks.Count; j++)
            {
                if (index.x + i >= gridOccupied.GetLength(0) || index.y + j >= gridOccupied.GetLength(1)
                                                             || index.x + selectedPiece.maxIndex.x >
                                                             gridRowSticks.Length || j + index.y >
                                                             gridRowSticks[selectedPiece.maxIndex.x + i].sticks.Count)
                    return false;
                if (gridOccupied[index.x + i, index.y + j] && selectedPiece.gridRows[i].sticks[j] != null)
                    return false;
            }
        }

        Debug.Log("CanSnap");
        return true;
    }

    private void OnStickDroped()
    {
        PlacePiece();
        selectedPiece = null;
    }

    private void Update()
    {
        if (selectedPiece == null) return;
        CheckForSnap();
    }
}

[Serializable]
public struct SticksRow
{
    public List<Stick> sticks;
}
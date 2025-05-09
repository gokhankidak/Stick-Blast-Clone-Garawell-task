using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GridController : MonoBehaviour
{
    [SerializeField] private SticksRow[] gridRowSticks;
    [SerializeField] private GamePlaySO gamePlaySO;
    [SerializeField] private float snapDistance = 0.25f;

    private Piece selectedPiece;
    private Transform referencePoint;
    private Vector2Int snapIndex;
    private bool isReferenceVertical;
    private Vector3 pieceOffset = Vector3.up * 1.5f;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnEnable()
    {
        gamePlaySO.OnPieceSelected += OnStickSelected;
        gamePlaySO.OnPieceDroped += OnStickDroped;
        gamePlaySO.OnRestart += RestartGame;
    }

    private void OnDisable()
    {
        gamePlaySO.OnPieceSelected -= OnStickSelected;
        gamePlaySO.OnPieceDroped -= OnStickDroped;
        gamePlaySO.OnRestart -= RestartGame;
    }

    private void Start()
    {
        for (int i = 0; i < gridRowSticks.Length; i++)
        {
            for (int j = 0; j < gridRowSticks[i].sticks.Count; j++)
            {
                gridRowSticks[i].sticks[j].RemoveHighlight();
            }
        }
        
        ReDrawGrid();
        StartCoroutine(CustomUpdate());
    }

    private void RestartGame()
    {
        for (int i = 0; i < gridRowSticks.Length; i++)
        {
            for (int j = 0; j < gridRowSticks[i].sticks.Count; j++)
            {
                gridRowSticks[i].sticks[j].Clear();
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
                if (gridRowSticks[i].sticks[j].isOccupied)
                {
                    stick.OnOccupied();
                }
                else
                {
                    stick.RemoveHighlight();
                }
            }
        }
    }

    private void RemoveAllHighlights()
    {
        for (int i = 0; i < gridRowSticks.Length; i++)
        {
            for (int j = 0; j < gridRowSticks[i].sticks.Count; j++)
            {
                gridRowSticks[i].sticks[j].RemoveHighlight();
            }
        }
    }

    private void PlacePiece()
    {
        if (snapIndex.x == -1 || snapIndex.y == -1) return;

        for (int x = 0; x < selectedPiece.gridRows.Count; x++)
        {
            for (int y = 0; y < selectedPiece.gridRows[x].sticks.Count; y++)
            {
                if (selectedPiece.gridRows[x].sticks[y] == null) continue;
                gridRowSticks[snapIndex.x + x].sticks[snapIndex.y + y].OnOccupied();
            }
        }

        selectedPiece.DestroyPiece();
        gamePlaySO.OnPiecePlaced?.Invoke();
    }


    private void OnStickSelected()
    {
        selectedPiece = gamePlaySO.selectedPiece;
        referencePoint = selectedPiece.referencePointStick.transform;
    }

    private void CheckForSnap()
    {
        Vector2Int closestIndex = new Vector2Int(-1, -1);
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < gridRowSticks.Length; i++)
        {
            for (int j = 0; j < gridRowSticks[i].sticks.Count; j++)
            {
                if ((selectedPiece.isReferenceVertical && i % 2 == 0) ||
                    (!selectedPiece.isReferenceVertical && i % 2 == 1)) continue;

                Stick stick = gridRowSticks[i].sticks[j];
                var distance = Vector3.Distance(stick.transform.position, referencePoint.position + pieceOffset);

                if (distance > snapDistance) continue;
                RemoveAllHighlights();
                if (CanSnap(new Vector2Int(i, j)))
                {
                    if (closestDistance > distance)
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
            RemoveAllHighlights();
            return;
        }

        HighlightSnap(closestIndex.x, closestIndex.y);
    }


    public bool CanPlace(Piece piece)
    {
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < gridRowSticks.Length; i++)
        {
            for (int j = 0; j < gridRowSticks[i].sticks.Count; j++)
            {
                if (CanSnap(new Vector2Int(i, j), piece))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void HighlightSnap(int i, int j)
    {
        for (int x = 0; x < selectedPiece.gridRows.Count; x++)
        {
            for (int y = 0; y < selectedPiece.gridRows[x].sticks.Count; y++)
            {
                if (selectedPiece.gridRows[x].sticks[y] == null) continue;
                gridRowSticks[i + x].sticks[j + y].Highlight();
            }
        }
    }

    private bool CanSnap(Vector2Int snapIndex, Piece piece = null)
    {
        if (piece == null)
            piece = selectedPiece;
        for (int i = 0; i < piece.gridRows.Count; i++)
        {
            for (int j = 0; j < piece.gridRows[i].sticks.Count; j++)
            {
                if (snapIndex.x + i >= gridRowSticks.Length ||
                    snapIndex.y + j >= gridRowSticks[snapIndex.x + i].sticks.Count)
                    return false;
                if (gridRowSticks[snapIndex.x + i].sticks[snapIndex.y + j].isOccupied &&
                    piece.gridRows[i].sticks[j] != null)
                    return false;
            }
        }

        return true;
    }

    private void OnStickDroped()
    {
        PlacePiece();
        ReDrawGrid();
        selectedPiece = null;
    }

    private IEnumerator CustomUpdate()
    {
        float tickTime = (float)1/gamePlaySO.customUpdateCount;
        while (true)
        {
            
            yield return new WaitForSeconds(tickTime);
            if (selectedPiece != null)
                CheckForSnap();
        }
    }
}

[Serializable]
public struct SticksRow
{
    public List<Stick> sticks;
}
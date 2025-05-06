using System;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Stick referencePointStick;
    public bool isReferenceVertical;
    public List<SticksRow> gridRows;
    [HideInInspector] public Vector2Int maxIndex = Vector2Int.zero;
    
    private Vector3 originalScale;
    private Vector3 downScale;
    private Vector3 mouseOffset;
    private Vector3 startPosition;
    
    private PieceController pieceController;

    private void Start()
    {
        pieceController = ServiceLocator.Get<PieceController>();
        CalculateMaxIndexes();
        
        startPosition = transform.position;
        originalScale = transform.localScale;
        downScale = originalScale * 0.7f;
        transform.localScale = downScale;
    }

    private void CalculateMaxIndexes()
    {
        maxIndex.x = gridRows.Count;
        for (int i = 0; i < gridRows.Count; i++)
        {
            if(maxIndex.y < gridRows[i].sticks.Count)
                maxIndex.y = gridRows[i].sticks.Count;
        }
    }

    private void OnMouseEnter()
    {
        transform.localScale = originalScale;
    }
    
    private void OnMouseDown()
    {
        mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pieceController.OnPieceSelected(this);
        
    }
    
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;
    }

    private void OnMouseUp()
    {
        transform.position = startPosition;
        pieceController.OnPieceDropped();
    }

    public void DestroyPiece()
    {
        Destroy(gameObject);
    }

    private void OnMouseExit()
    {
        transform.localScale = downScale;
    }
}

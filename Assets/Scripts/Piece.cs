using System;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Stick referencePointStick;
    public bool isReferenceVertical;
    public List<SticksRow> gridRows;
    
    private Vector3 originalScale;
    private Vector3 downScale;
    private Vector3 mouseOffset;
    private Vector3 startPosition;
    
    private PieceManager pieceManager;

    private void Start()
    {
        pieceManager = ServiceLocator.Get<PieceManager>();
        
        startPosition = transform.position;
        originalScale = transform.localScale;
        downScale = originalScale * 0.7f;
        transform.localScale = downScale;
    }
    

    private void OnMouseEnter()
    {
        transform.localScale = originalScale;
    }
    
    private void OnMouseDown()
    {
        mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pieceManager.OnPieceSelected(this);
        
    }
    
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;
    }

    private void OnMouseUp()
    {
        transform.position = startPosition;
        pieceManager.OnPieceDropped();
    }

    public void DestroyPiece()
    {
        pieceManager.OnPieceDestroyed(this);
        Destroy(gameObject);
    }

    private void OnMouseExit()
    {
        transform.localScale = downScale;
    }
}

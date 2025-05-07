using System;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [HideInInspector] public bool isOccupied;
    [SerializeField] List<Block> adjacentBlocks;
    [SerializeField] private Color occupiedColor, emptyColor, highlightColor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnOccupied()
    {
        spriteRenderer.color = occupiedColor;
        foreach (var block in adjacentBlocks)
        {
            block.OnOccupied();
        }
        isOccupied = true;
    }
    
    public void OnScored()
    {
        spriteRenderer.color = emptyColor;
        foreach (var block in adjacentBlocks)
        {
            block.OnRemoved();
        }
        isOccupied = false;
    }

    public void Highlight()
    {
        spriteRenderer.color = highlightColor;
        foreach (var block in adjacentBlocks)
        {
            block.Highlight();
        }
    }
    
    public void RemoveHighlight()
    {
        spriteRenderer.color = isOccupied? occupiedColor : emptyColor;
        foreach (var block in adjacentBlocks)
        {
            block.RemoveHighlight();
        }
    }
}

using System;
using System.Collections.Generic;
using SpriteGlow;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public bool isOccupied;
    public bool isScoreHighlighted;
    [SerializeField] List<Block> adjacentBlocks;
    [SerializeField] private Color occupiedColor, emptyColor, highlightColor;
    private SpriteRenderer spriteRenderer;
    private SpriteGlowEffect spriteGlowEffect;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteGlowEffect = GetComponent<SpriteGlowEffect>();
    }
    
    public void OnOccupied()
    {
        ScoreHighlight(false);
        spriteRenderer.color = occupiedColor;
        foreach (var block in adjacentBlocks)
        {
            block.Occupy();
        }
        isOccupied = true;
    }
    
    public void OnScored()
    {
        ScoreHighlight(false);
        spriteRenderer.color = emptyColor;
        foreach (var block in adjacentBlocks)
        {
            block.RemoveOccupy();
        }
        isOccupied = false;
    }

    public void Highlight()
    {
        isScoreHighlighted = true;
        spriteRenderer.color = highlightColor;
        foreach (var block in adjacentBlocks)
        {
            block.Highlight();
        }
    }
    
    public void ScoreHighlight(bool isActive)
    {
        isScoreHighlighted = isActive;
        spriteGlowEffect.enabled = isActive;
        foreach (var block in adjacentBlocks)
        {
            block.ScoreHighlight(isActive);
        }
    }

    public void Clear()
    {
        ScoreHighlight(false);
        isOccupied = false;
        spriteRenderer.color = emptyColor;
        foreach (var block in adjacentBlocks)
        { 
            block.ResetBlock();
        }
    }
    
    public void RemoveHighlight()
    {
        ScoreHighlight(false);
        spriteRenderer.color = isOccupied? occupiedColor : emptyColor;
        foreach (var block in adjacentBlocks)
        {
            block.RemoveHighlight();
        }
    }
}

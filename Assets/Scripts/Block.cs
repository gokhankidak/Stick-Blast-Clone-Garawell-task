using System;
using SpriteGlow;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Color occupiedColor, emptyColor, highlightColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GamePlaySO gamePlaySO;

    private SpriteGlowEffect spriteGlowEffect;
    private int occupiedCount = 0;
    
    private void Awake()
    {
        spriteGlowEffect = GetComponent<SpriteGlowEffect>();
    }

    public void ResetBlock()
    {
        occupiedCount = 0;
        spriteRenderer.color = emptyColor;
    }

    public void Occupy()
    {
        spriteRenderer.color = occupiedColor;
        occupiedCount++;
    }

    public void RemoveOccupy()
    {
        occupiedCount--;
        spriteRenderer.color = occupiedCount <= 0 ? emptyColor : occupiedColor;
    }

    public void RemoveHighlight()
    {
        spriteRenderer.color = occupiedCount > 0 ? occupiedColor : emptyColor;
    }
    
    public void ScoreHighlight(bool isActive)

    {
        spriteGlowEffect.enabled = isActive;
    }

    public void Highlight()
    {
        spriteRenderer.color = highlightColor;
    }
}
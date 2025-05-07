using System;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [HideInInspector] public bool isOccupied;
    [SerializeField] List<GameObject> adjacentBlocks;
    [SerializeField] private Color occupiedColor, emptyColor, highlightColor;
    [SerializeField] private GameObject highlightEffect;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnOccupied()
    {
        spriteRenderer.color = occupiedColor;
        isOccupied = true;
    }
    
    public void OnScored()
    {
        spriteRenderer.color = emptyColor;
        isOccupied = false;
    }

    public void OnHighlighted()
    {
        spriteRenderer.color = highlightColor;
        highlightEffect.SetActive(true);
    }
    
    public void OnRemoved()
    {
        spriteRenderer.color = emptyColor;
        highlightEffect.SetActive(false);
        isOccupied = false;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] List<GameObject> adjacentBlocks;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}

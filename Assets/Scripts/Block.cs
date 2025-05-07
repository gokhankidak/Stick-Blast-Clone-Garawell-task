using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Color occupiedColor, emptyColor, highlightColor;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int occupiedCount = 0;

    public void OnOccupied()
    {
        spriteRenderer.color = occupiedColor;
        occupiedCount++;
    }

    public void OnRemoved()
    {
        occupiedCount--;
        spriteRenderer.color = occupiedCount <= 0 ? emptyColor : occupiedColor;
    }

    public void RemoveHighlight()
    {
        spriteRenderer.color = occupiedCount > 0 ? occupiedColor : emptyColor;
    }

    public void Highlight()
    {
        spriteRenderer.color = highlightColor;
    }
}
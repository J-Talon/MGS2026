// Authors: [Jacky, Jeremy, Mark]
// This script manages a single cell in the grid system.
// It holds properties related to the cell's state.

using UnityEngine;

public class GridCell : MonoBehaviour
{
    // Cell configuration and state
    public int cellSizeX = 1;
    public int cellSizeY = 1;
    public bool isOccupied = false;
    public bool isFriendly = false;

    // Plain colors for different states of the cell. Mainly for testing purposes. Can be removed later when we have proper textures.
    private Color friendlyColor = Color.green;
    private Color friendlyOffsetColor = new Color(0f, 0.5f, 0f, 1f);
    private Color enemyColor = Color.brown;
    private Color enemyOffsetColor = new Color(0.36f, 0.18f, 0.07f, 1f);

    // References to textures and sprite renderer for visual representation
    public Texture2D friendlyTexture;
    public Texture2D enemyTexture;

    // Reference to the SpriteRenderer component for updating the cell's appearance
    public SpriteRenderer spriteRenderer;

    public void UpdateTexture(bool isOffset)
    {
        if (isFriendly)
            spriteRenderer.color = isOffset ? friendlyOffsetColor : friendlyColor;
        else
            spriteRenderer.color = isOffset ? enemyOffsetColor : enemyColor;
    }
}

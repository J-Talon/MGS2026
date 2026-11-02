// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// </summary>
/// This script manages a single cell in the grid system.
/// It holds properties related to the cell's state.
/// </summary>
public class GridCell : MonoBehaviour
{
    public EntityUnit occupiedUnit { get; private set; }

    public bool isOccupied = false;
    public bool isFriendly = false;
    private Vector2Int cellPosition;

    // Plain colors for different states of the cell. Mainly for testing purposes. Can be removed later when we have proper textures.
    private Color friendlyColor = Color.green;
    private Color friendlyOffsetColor = new Color(0f, 0.5f, 0f, 1f);
    private Color enemyColor = Color.brown;
    private Color enemyOffsetColor = new Color(0.36f, 0.18f, 0.07f, 1f);
    public Color GetFriendlyColor() { return friendlyColor; }
    public void SetFriendlyColor(Color value) { friendlyColor = value; }
    public Color GetEnemyColor() { return enemyColor; }
    public void SetEnemyColor(Color value) { enemyColor = value; }

    // References to textures and sprite renderer for visual representation
    public Texture2D friendlyTexture;
    public Texture2D enemyTexture;

    // Reference to the SpriteRenderer component for updating the cell's appearance
    public SpriteRenderer spriteRenderer;

    // Setter/Getters
    public bool GetIsOccupied() { return isOccupied; }
    public bool GetIsFriendly() { return isFriendly; }
    public Vector2Int GetCellPosition() { return cellPosition; }
    public void SetCellPosition(Vector2Int value) { cellPosition = value; }

    /// <summary>
    /// Updates the texture of the cell based on its state (friendly or enemy) and whether it is offset.
    /// </summary>
    /// <param name="isOffset">Indicates whether the cell is offset.</param>
    public void UpdateTexture(bool isOffset)
    {
        if (isFriendly)
            spriteRenderer.color = isOffset ? friendlyOffsetColor : friendlyColor;
        else
            spriteRenderer.color = isOffset ? enemyOffsetColor : enemyColor;
    }

    /// <summary>
    /// Updates the texture of the cell based on its state (friendly or enemy) and whether it is offset.
    /// </summary>
    /// <param name="isOffset">Indicates whether the cell is offset.</param>
    public void EntityUnitOccupy(EntityUnit unit)
    {
        occupiedUnit = unit;
        isOccupied = true;
    }

    /// <summary>
    /// Updates the texture of the cell based on its state (friendly or enemy) and whether it is offset.
    /// </summary>
    /// <param name="isOffset">Indicates whether the cell is offset.</param>
    public void EntityUnitLeave()
    {
        occupiedUnit = null;
        isOccupied = false;
    }
}

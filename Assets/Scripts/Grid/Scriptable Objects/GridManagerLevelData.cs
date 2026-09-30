using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Grid/GridManagerLevelData", order = 1)]
public class GridManagerLevelData : ScriptableObject
{
    // Cell configuration and state
    public Vector2Int gridSize;

    /// <summary>
    /// initial ratio of column territory per player
    /// </summary>
    public int startingFriendlyColumns = 4;

    // Bounding box for grid cells to explicitly define the grid area.
    // This is useful for future considerations, such as extending the grid on the negative x-axis.
    public int xBoundingBoxMin = 0;
    public int xBoundingBoxMax = 8;
    public int yBoundingBoxMin = 0;
    public int yBoundingBoxMax = 4;
}
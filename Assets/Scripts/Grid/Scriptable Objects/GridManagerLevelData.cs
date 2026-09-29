using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Grid/GridLevelData", order = 1)]
public class GridLevelData : ScriptableObject
{
    // Cell configuration and state
    public Vector2Int gridSize;

    /// <summary>
    /// initial ratio of column territory per player
    /// </summary>
    public int startingFreindlyColumns = 1; 

}
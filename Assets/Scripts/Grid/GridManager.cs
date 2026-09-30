// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This script manages the grid system of the game.
/// It handles the generation of grid cells and updating the state of the grid based on player actions and game events.
/// </summary>
public class GridManager : MonoBehaviour, IGridSystemControl
{
    public static GridManager Instance { get { return instance; } }
    private static GridManager instance = null;

    /// <summary>
    /// grid specific level data
    /// </summary>
    public GridManagerLevelData gridLevelData;

    // References
    public GridCell gridCellPrefab;
    public Transform cameraTransform;

    /// <summary>
    /// For organizing grid cells
    /// </summary>
    public GameObject gridCellContainer {private set; get;}

    /// <summary>
    /// auto create debugger script if enabled
    /// </summary>
    public bool debuggerEnable;

    // Runtime state
    private Dictionary<Vector2, GridCell> gridCells;

    
    /// <summary>
    /// The bordering column of control (W.R.T player)
    /// </summary>
    private int border;
    
    // Generate the grid based on the specified size and instantiate grid cells
    void GenerateGrid()
    {
        border = gridLevelData.startingFriendlyColumns-1; //-1 cause of 0 indexing

        // Iterate through grid cells
        for (int x = 0; x < gridLevelData.gridSize.x; x++) //row setup
        {
            for (int y = 0; y < gridLevelData.gridSize.y; y++) //col setup
            {
                // Instantiate a new grid cell at the specified position
                GridCell newCell = Instantiate(gridCellPrefab, new Vector3(x, y, 0), Quaternion.identity, gridCellContainer.transform);
                // Name the cell for easier identification in the hierarchy
                newCell.name = $"GridCell_{x}_{y}";
                // Set if cell is friendly or enemy based on position. For now, we can assume the first half of the grid is friendly and the second half is enemy.
                newCell.isFriendly = x < gridLevelData.startingFriendlyColumns;
                // Store the cell in the grid cells dictionary for easy access later
                gridCells[new Vector2(x, y)] = newCell;
            }
        }
    }

    // Camera repositioning to center the grid in the scene view
    void CenterCamera()
    {
        cameraTransform.position = new Vector3((gridLevelData.gridSize.x / 2f) - 0.5f, (gridLevelData.gridSize.y / 2f) - 0.5f, -10f);
    }

    // Function to update the grid textures. Separate from GenerateGrid in case we want to update this in the future.
    void UpdateGridTextures()
    {
        // Iterate through grid cells
        for (int x = 0; x < gridLevelData.gridSize.x; x++)
        {
            for (int y = 0; y < gridLevelData.gridSize.y; y++)
            {
                GridCell cell = gridCells[new Vector2(x, y)];
                // Update the texture based on whether the cell is friendly or enemy and if it's offset
                bool isOffset = (x + y) % 2 == 1; // Simple checkerboard pattern for offset
                cell.UpdateTexture(isOffset);
            }
        }
    }

    
    /// <summary>
    /// Change control of a single 
    /// 
    /// by default, increases "freindly" player territory by 1 col
    /// </summary>
    /// <param name="freindly"></param>
    public void SingleColControlUpdate(bool freindly = true)
    {   
        if (freindly)
        {
            if (border < gridLevelData.gridSize.x-1) //-1 cause of 0 indexing
            { 
                border += 1;
                for (int i = 0; i < gridLevelData.gridSize.y; i++)
                {
                    gridCells[new Vector2(border, i)].isFriendly = true; 
                }
                
            }
            else
            {
                print ("cannot increase freindly border control any more");
            }
        }
        else
        {
            if (border > -1) //-1 cause of 0 indexing
            { 
                for (int i = 0; i < gridLevelData.gridSize.y; i++)
                {
                    gridCells[new Vector2(border, i)].isFriendly = false; 
                } 
                border -= 1;   
            }
            else
            {
                print ("cannot decrease freindly border control any more");
            }
        }

        
        UpdateGridTextures();
    }

    /// <summary>
    /// Function to get a grid cell at a specific position. Returns null if the position is out of bounds.
    /// </summary>
    /// <param name="position">XY coordinate of GridCell</param>
    /// <returns>GridCell object</returns>
    public GridCell GetTileAtPosition(Vector2 position)
    {
        // Check if the position is within the grid bounds
        if (gridCells.TryGetValue(position, out var gridCell))
        {
            // Return the grid cell if found
            return gridCell;
        }
        return null; // Return null if the position is out of bounds
    }

    // Public function to extend the grid by adding a new column either in the left or the right direction.
    public void AddGridColumn()
    {
        // Implementation for adding a new column to the grid
    }

    // Public function to extend the grid by adding a new row either in the top or the bottom direction.
    public void AddGridRow()
    {
        // Implementation for adding a new row to the grid
    }

    /// <summary>
    /// Class setup
    /// </summary>
    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        // Initialize grid cell container
        gridCellContainer = new GameObject("Grid Cell Container");

        new GridEventHandler(this);

        if (debuggerEnable == true)
        {
            gameObject.AddComponent<GridManagerDebugger>();
        }
    }

    /// <summary>
    /// Class startup behaviour
    /// </summary>
    void Start()
    {
        // Initialize the grid cells
        gridCells = new Dictionary<Vector2, GridCell>();
        GenerateGrid();
        CenterCamera();
        UpdateGridTextures();
    }

}

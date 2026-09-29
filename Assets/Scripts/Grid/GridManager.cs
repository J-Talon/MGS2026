// Authors: [Jacky, Jeremy, Mark]
// This script manages the grid system of the game.
// It handles the generation of grid cells and updating the state of the grid based on player actions and game events.
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Singleton instance for easy access to the GridManager from other scripts
    public static GridManager Instance { get; private set; }

    // Grid configuration
    public int gridSizeX = 8;
    public int gridSizeY = 4;

    // References
    public GridCell gridCellPrefab;
    public Transform cameraTransform;

    // Runtime state
    public GridCell[,] cells;

    // Generate the grid based on the specified size and instantiate grid cells
    void GenerateGrid()
    {
        // Iterate through grid cells
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Instantiate a new grid cell at the specified position
                GridCell newCell = Instantiate(gridCellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                // Name the cell for easier identification in the hierarchy
                newCell.name = $"GridCell_{x}_{y}";
                // Set if cell is friendly or enemy based on position. For now, we can assume the first half of the grid is friendly and the second half is enemy.
                newCell.isFriendly = x < (gridSizeX / 2);
                // Store the cell in the 2D array
                cells[x, y] = newCell;
            }
        }
    }

    // Camera repositioning to center the grid in the scene view
    void CenterCamera()
    {
        cameraTransform.position = new Vector3((gridSizeX / 2f) - 0.5f, (gridSizeY / 2f) - 0.5f, -10f);
    }

    // Function to update the grid textures. Separate from GenerateGrid in case we want to update this in the future.
    void UpdateGridTextures()
    {
        // Iterate through grid cells
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                GridCell cell = cells[x, y];
                // Update the texture based on whether the cell is friendly or enemy and if it's offset
                bool isOffset = (x + y) % 2 == 1; // Simple checkerboard pattern for offset
                cell.UpdateTexture(isOffset);
            }
        }
    }

    void Start()
    {
        // Initialize the grid cells
        cells = new GridCell[gridSizeX, gridSizeY];
        GenerateGrid();
        CenterCamera();
        UpdateGridTextures();
    }

}

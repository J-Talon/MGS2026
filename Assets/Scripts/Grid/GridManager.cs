// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using System.Collections.Generic;
using Event;

/// <summary>
/// This script manages the grid system of the game.
/// It handles the generation of grid cells and updating the state of the grid based on player actions and game events.
/// </summary>
public class GridManager : MonoBehaviour, IGridSystemControl, IGridSystemView
{
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
    public GameObject gridCellContainer { private set; get; }

    /// <summary>
    /// auto create debugger script if enabled
    /// </summary>
    public bool debuggerEnable;

    // Runtime state
    private Dictionary<Vector2, GridCell> gridCells;

    /// <summary>
    /// Runtime bounds of the grid (inclusive). Copied from the level data on startup so that
    /// adding/removing columns and rows at runtime never modifies the ScriptableObject asset.
    /// </summary>
    private int xMin, xMax, yMin, yMax;

    /// <summary>
    /// The bordering column of control (W.R.T player).
    /// Column x is friendly if x <= border. border == xMin - 1 means no friendly columns.
    /// </summary>
    private int border;

    /// <summary>
    /// The rightmost column the player is allowed to own.
    /// Columns past this are permanently enemy territory and can never be captured.
    /// </summary>
    private int CaptureLimit => xMax - gridLevelData.uncapturableColumns;

    // Generate the grid based on the specified size and instantiate grid cells
    void GenerateGrid()
    {
        xMin = gridLevelData.xBoundingBoxMin;
        xMax = xMin + gridLevelData.gridSize.x - 1; //-1 cause of 0 indexing
        yMin = gridLevelData.yBoundingBoxMin;
        yMax = yMin + gridLevelData.gridSize.y - 1;

        border = ClampBorder(xMin + gridLevelData.startingFriendlyColumns - 1); //-1 cause of 0 indexing

        for (int x = xMin; x <= xMax; x++)
        {
            CreateColumn(x);
        }
    }

    // Instantiate a single grid cell at the given coordinate. Ownership follows the border.
    void CreateCell(int x, int y)
    {
        // Instantiate a new grid cell at the specified position
        GridCell newCell = Instantiate(gridCellPrefab, new Vector3(x, y, 0), Quaternion.identity, gridCellContainer.transform);
        // Name the cell for easier identification in the hierarchy
        newCell.name = $"GridCell_{x}_{y}";
        newCell.isFriendly = x <= border;
        // Store the cell in the grid cells dictionary for easy access later
        gridCells[new Vector2(x, y)] = newCell;
    }

    // Destroy a single grid cell at the given coordinate, if it exists
    void DestroyCell(int x, int y)
    {
        Vector2 coordinate = new Vector2(x, y);
        if (gridCells.TryGetValue(coordinate, out var cell))
        {
            Destroy(cell.gameObject);
            gridCells.Remove(coordinate);
        }
    }

    // Instantiate a full column of grid cells at the given x coordinate
    void CreateColumn(int x)
    {
        for (int y = yMin; y <= yMax; y++)
            CreateCell(x, y);
    }

    // Destroy a full column of grid cells at the given x coordinate
    void DestroyColumn(int x)
    {
        for (int y = yMin; y <= yMax; y++)
            DestroyCell(x, y);
    }

    // Instantiate a full row of grid cells at the given y coordinate
    void CreateRow(int y)
    {
        for (int x = xMin; x <= xMax; x++)
            CreateCell(x, y);
    }

    // Destroy a full row of grid cells at the given y coordinate
    void DestroyRow(int y)
    {
        for (int x = xMin; x <= xMax; x++)
            DestroyCell(x, y);
    }

    // Set the ownership of every cell in a column
    void SetColumnOwnership(int x, bool isFriendly)
    {
        for (int y = yMin; y <= yMax; y++)
            gridCells[new Vector2(x, y)].isFriendly = isFriendly;
    }

    // Keep a border value between "no friendly columns" and the capture limit
    int ClampBorder(int value)
    {
        return Mathf.Max(xMin - 1, Mathf.Min(value, CaptureLimit));
    }

    // Pull the border back inside the allowed range after the grid changes shape,
    // giving any columns that fall outside it back to the enemy
    void EnforceBorderLimits()
    {
        int clamped = ClampBorder(border);
        for (int x = clamped + 1; x <= border; x++)
            SetColumnOwnership(x, false);
        border = clamped;
    }

    // Camera repositioning to center the grid in the scene view
    void CenterCamera()
    {
        cameraTransform.position = new Vector3((xMin + xMax) / 2f, (yMin + yMax) / 2f, -10f);
    }

    // Function to update the grid textures. Separate from GenerateGrid in case we want to update this in the future.
    void UpdateGridTextures()
    {
        // Iterate through grid cells
        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                GridCell cell = gridCells[new Vector2(x, y)];
                // Update the texture based on whether the cell is friendly or enemy and if it's offset
                bool isOffset = ((x + y) & 1) == 1; // Simple checkerboard pattern for offset (& instead of % so negative coordinates work)
                cell.UpdateTexture(isOffset);
            }
        }
    }

    /// <summary>
    /// Change control of a single column
    ///
    /// by default, increases "friendly" player territory by 1 col
    /// </summary>
    /// <param name="friendly"></param>
    public void SingleColControlUpdate(bool friendly = true)
    {
        if (friendly)
        {
            if (border < CaptureLimit)
            {
                border += 1;
                SetColumnOwnership(border, true);
            }
            else
            {
                print("cannot increase friendly border control any more");
            }
        }
        else
        {
            if (border >= xMin)
            {
                SetColumnOwnership(border, false);
                border -= 1;
            }
            else
            {
                print("cannot decrease friendly border control any more");
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

    /// <summary>
    /// Extend the grid by one column on the given side.
    /// A column added on the right is always enemy (it becomes part of the uncapturable columns).
    /// A column added on the left is friendly if the player owns the current leftmost column,
    /// so friendly territory always stays one contiguous block on the left.
    /// </summary>
    /// <param name="side">Edge of the grid to add the column to</param>
    public void AddGridColumn(GridSide side)
    {
        if (side == GridSide.Right)
        {
            xMax += 1;
        }
        else
        {
            bool isFriendly = border >= xMin; // player owns the current leftmost column
            xMin -= 1;
            if (!isFriendly)
                border = xMin - 1;
        }

        CreateColumn(side == GridSide.Right ? xMax : xMin);
        CenterCamera();
        UpdateGridTextures();
    }

    /// <summary>
    /// Shrink the grid by one column on the given side. The grid always keeps at least one column.
    /// </summary>
    /// <param name="side">Edge of the grid to remove the column from</param>
    public void RemoveGridColumn(GridSide side)
    {
        if (xMin == xMax)
        {
            print("cannot remove the last column of the grid");
            return;
        }

        if (side == GridSide.Right)
        {
            DestroyColumn(xMax);
            xMax -= 1;
        }
        else
        {
            DestroyColumn(xMin);
            xMin += 1;
        }

        // Removing on the right shifts the uncapturable columns left, removing on the left can empty
        // the player's territory; either way the border may need pulling back into range
        EnforceBorderLimits();
        CenterCamera();
        UpdateGridTextures();
    }

    /// <summary>
    /// Extend the grid by one row on the given side. Ownership of the new cells follows the current border.
    /// </summary>
    /// <param name="side">Edge of the grid to add the row to</param>
    public void AddGridRow(GridRowSide side)
    {
        if (side == GridRowSide.Top)
        {
            yMax += 1;
            CreateRow(yMax);
        }
        else
        {
            yMin -= 1;
            CreateRow(yMin);
        }

        CenterCamera();
        UpdateGridTextures();
    }

    /// <summary>
    /// Shrink the grid by one row on the given side. The grid always keeps at least one row.
    /// </summary>
    /// <param name="side">Edge of the grid to remove the row from</param>
    public void RemoveGridRow(GridRowSide side)
    {
        if (yMin == yMax)
        {
            print("cannot remove the last row of the grid");
            return;
        }

        if (side == GridRowSide.Top)
        {
            DestroyRow(yMax);
            yMax -= 1;
        }
        else
        {
            DestroyRow(yMin);
            yMin += 1;
        }

        CenterCamera();
        UpdateGridTextures();
    }

    /// <summary>
    /// Class setup
    /// </summary>
    void Awake()
    {
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

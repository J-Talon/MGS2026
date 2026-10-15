// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// This script manages the grid system of the game.
/// It sets the grid up, answers queries about it and renders it.
/// Every change to the grid after setup is made by its GridManipulator, reachable through Controls.
/// </summary>
public class GridManager : MonoBehaviour, IGridSystemView
{
    /// <summary>
    /// The grid manager in the current scene
    /// </summary>
    public static GridManager instance { get; private set; }

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

    /// <summary>
    /// Grid manipulation methods (territory control, adding/removing columns and rows)
    /// </summary>
    public IGridSystemControl Controls => manipulator;

    // Runtime state
    private GridState state;
    private GridManipulator manipulator;
    private GridEventHandler eventHandler;

    /// <summary>
    /// Function to get a grid cell at a specific position. Returns null if the position is out of bounds.
    /// </summary>
    /// <param name="position">XY coordinate of GridCell</param>
    /// <returns>GridCell object</returns>
    public GridCell GetTileAtPosition(Vector2Int position)
    {
        // Check if the position is within the grid bounds
        if (state.Cells.TryGetValue(position, out var gridCell))
        {
            // Return the grid cell if found
            return gridCell;
        }
        return null; // Return null if the position is out of bounds
    }

    /// <summary>
    /// Function to get a entity at the given grid cell
    /// It is a wrapper function using GetTileAtPosition
    /// </summary>
    /// <returns>Entity if one exists there (using a snappy movement type), null otherwise</returns>
    public EntityUnit GetEntityAtTilePosition(Vector2Int position)
    {
        GridCell tile = GetTileAtPosition(position);
        if (tile != null)
        {
            EntityUnit unit = tile.occupiedUnit;
            if (unit != null)
            {
                return unit;
            }
        }

        return null; //return null if either no units on cell or cell itself doesn't exist at the given position
    }

    // Re-centre the camera and recolour every cell after the grid changes
    void Redraw()
    {
        CenterCamera();
        UpdateGridTextures();
    }

    // Camera repositioning to center the grid in the scene view
    void CenterCamera()
    {
        cameraTransform.position = new Vector3((state.XMin + state.XMax) / 2f, (state.YMin + state.YMax) / 2f, -10f);
    }

    // Function to update the grid textures
    void UpdateGridTextures()
    {
        // Iterate through grid cells
        for (int x = state.XMin; x <= state.XMax; x++)
        {
            for (int y = state.YMin; y <= state.YMax; y++)
            {
                GridCell cell = state.Cells[new Vector2Int(x, y)];
                // Update the texture based on whether the cell is friendly or enemy and if it's offset
                bool isOffset = ((x + y) & 1) == 1; // Simple checkerboard pattern for offset (& instead of % so negative coordinates work)
                cell.UpdateTexture(isOffset);
            }
        }
    }

    /// <summary>
    /// Class setup. The grid is built here rather than in Start so that other scripts
    /// can query it from their own Start.
    /// </summary>
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("more than one GridManager in the scene; destroying the extra one");
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Initialize grid cell container
        gridCellContainer = new GameObject("Grid Cell Container");

        state = new GridState();
        manipulator = new GridManipulator(state, gridCellPrefab, gridCellContainer.transform);
        manipulator.Changed += Redraw;
        manipulator.Build(gridLevelData);

        eventHandler = new GridEventHandler(manipulator);

        if (debuggerEnable == true)
        {
            gameObject.AddComponent<GridManagerDebugger>();
        }
    }

    /// <summary>
    /// Class cleanup
    /// </summary>
    void OnDestroy()
    {
        if (instance != this)
            return;

        eventHandler?.Unsubscribe();
        instance = null;
    }
}

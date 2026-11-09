// Authors: [Jacky, Jeremy, Mark]
// Added viewing the grid boundaries: -Wilson Nov 1 2026
// Basic OnDrawGizmos implementation: -Jophiel Nov 8th 2026
using UnityEngine;
using System.Collections.Generic;
using System;
using Entities.Unit;
using Event;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

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

    
    public List<(Vector2Int, EntityUnit)> FindAllEntities(Func<EntityUnit, bool> criteria)
    {
        var units_with = new List<(Vector2Int, EntityUnit)>();
        
        for (int x = state.XMin; x < state.XMax+1; x++) {
            for (int y = state.YMin; y < state.YMax+1; y++) {
                var pos = new Vector2Int(x, y);
                var cell = state.Cells[pos];

                if (cell.occupiedUnit != null && criteria.Invoke(cell.occupiedUnit))
                {
                    units_with.Add((pos, cell.occupiedUnit));
                }
            }
        }

        return units_with;
    }

    /// <returns>If coordinate exists within grid bounds</returns>
    public bool WithinGrid(Vector2Int position) {
        return state.XMin <= position.x && position.x <= state.XMax && state.YMin <= position.y && position.y <= state.YMax;
    }

    /// <returns>If coordinate exists within grid bounds</returns>
    public bool WithinGridSmooth(Vector2 position) {
        return state.XMin-0.5 < position.x && position.x < state.XMax + 0.5 && state.YMin-0.5 < position.y && position.y < state.YMax+0.5;
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
    

    /// <summary>
    /// Function to get the bounds of the grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    public (Vector2Int min, Vector2Int max) GetGridBounds()
    {
        return (new Vector2Int(state.XMin, state.YMin), new Vector2Int(state.XMax, state.YMax));
    }

    /// Function to get the bounds of the friendly grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    public (Vector2Int min, Vector2Int max) GetFriendlyGridBounds()
    {
        return (new Vector2Int(state.XMin, state.YMin), new Vector2Int(state.Border, state.YMax));
    }

    /// Function to get the bounds of the enemy grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    public (Vector2Int min, Vector2Int max) GetEnemyGridBounds()
    {
        return (new Vector2Int(state.Border + 1, state.YMin), new Vector2Int(state.XMax, state.YMax));
    }

    // Re-centre the camera and recolour every cell after the grid changes
    public void Redraw()
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

    /// OnDrawGizmos - Renders the grid in-editor by taking 'gridLevelData' parameters and displaying them with cubes.
    /// Notes:  doesn't update to Grid-scaling once adjusted in 'Game';
    ///         work on implementation to update grid bounds in 'Scene' while GridManipulator builds. 
    ///         Also, improve system for showing icons at special squares (e.g. player, enemy, flora).
    void OnDrawGizmos()
    {
        // take grid bounds from gridLevelData object 
        int gridBoundX = gridLevelData.gridSize.x;
        int gridBoundY = gridLevelData.gridSize.y;

        // surpassing this value means you've entered a uncapturable grid section
        int gridOOB = gridBoundX - gridLevelData.uncapturableColumns; // OOB means "out of bounds"

        // to render the x and y components, draw rows then draw next column 
        for (int x = 0; x < gridBoundX; x++) {
            for (int y = 0; y < gridBoundY; y++) {

                Vector3 positionCell = new Vector3(x, y, 0); // localized position vector

                // since enemy field begins rendering at a halfway point,
                // start recording red after reaching halfway across the x-bound

                if (x == 0 && y == 0) { // check for player tile (usually first tile created)
                    Gizmos.color = Color.grey;
                    Gizmos.DrawIcon(new Vector3(x, y, -1), "TestPlayerModel.png"); // function currently can't pull image
                } 
                else if (x < (gridBoundX/2)) {    // check for player area; needs modification depending on 'Game' actions
                    Gizmos.color = Color.green;
                } 
                else if (x >= gridOOB) {          // check for uncaptureable tiles
                    Gizmos.color = Color.darkRed;
                } 
                else {                            // assume you are in enemy territory
                    Gizmos.color = Color.red; 
                }

                // draw a SQUARE to show the grid 
                Gizmos.DrawCube(positionCell, new Vector3(0.9f, 0.9f, 0));
                Gizmos.DrawWireCube(positionCell, new Vector3(1, 1, 0)); // aesthetical frame
            } // build next column by incrementing X
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

/*  author: Kian Reilly 
    github: kareilly
    discrd: kuushi

    The system is composed of 3 primary sets of data (as of now):
        -   An array describing column ownership (player/enemy)
        -   A dictionary describing tile occupation
        -   A dictionary mapping tile coordinates to world position

    This means tile ownership and avaliablity are separated and can be 
    applied individually as needed. The obvious advantage is that 
    checking columns is much faster than tiles. The system could also
    be expanded in this file or by a parallel system (i.e. a warning/
    damage system for tile/row/column-covering attacks). I can also
    try to design a framework for easy addition of such systems if
    that would be desireable.

    Entities using the grid system are responsible for listening for 
    events and keeping their local data up-to-date. 

    Potential issues & notes:
        -   Conflicts could be caused by tracking tile occupation with
            booleans rather than object references, may need to address
        -   Dictionary checks could be much more efficient if a
            CollectionsMarshal was used, but it would hurt readability */

using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region --- Variables & Initialization ---

    // instantiate as static singleton
    public static GridManager Instance {get; private set;}

    // inspector-settable grid data
    [SerializeField] private Vector2Int gridDimensions = new Vector2Int(8,5);
    [SerializeField] private Vector2 tileDimensions = new Vector2(1f,1f);
    [SerializeField] private Vector2 gridOrigin = new Vector2(0f,0f);
    [SerializeField] private int columnsOwned = 4;


    // array of ownership value for each column
    private bool[] ownedColumns;

    // dict of coordinates to if they are occupied
    private Dictionary<Vector2Int, bool> availableTiles; 

    // dict of coordinates to relative positions
    private Dictionary<Vector2Int, Vector2> gridPositions;

    /* 
    This is just a temporary solution for needing to attach the script 
    to a game object so that the serialized fields are configurable in the editor
    */
    private void Awake()
    {
        // destroys itself if it is attached to a game object on awake
        // enforces singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // make persistent across scenes
    }

    private void Start()
    {
        // initialize owned columns
        InitOwnedColumns();

        // initialize grid availability
        InitAvailableTiles();

        // initialize grid positions
        InitGridPositions();
    }
    
    // call when grid length is changed, or on init
    private void InitOwnedColumns()
    {
        ownedColumns = new bool[gridDimensions.x];
        SetOwnedColumns(columnsOwned, true, true);
    }

    // call when grid shape is changed, or on init
    private void InitAvailableTiles()
    {
        availableTiles = new Dictionary<Vector2Int, bool>{};

        // 2d iteration on x and y, 
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                availableTiles.Add(new Vector2Int(x,y), true);
            }
        }
    }

    // should be called when tile size or grid bounds are updated, or on init
    private void InitGridPositions()
    {
        gridPositions = new Dictionary<Vector2Int, Vector2>{};

        // 2d iteration on x and y, 
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector2Int iPos = new Vector2Int(x,y);
                Vector2 fPos = CoordToPos(iPos);

                gridPositions.Add(iPos, fPos);
            }
        }
    }
    #endregion

    #region --- Misc Getters & Setters -------
    
    // grid dimensions
    public Vector2Int GetGridDimensions()
    {
        return gridDimensions;
    }
    public void SetGridDimensions(Vector2Int newDimensions)
    {
        gridDimensions = newDimensions;
        Start();
    }

    // grid origin
    public Vector2 GetGridOrigin()
    {
        return gridOrigin;
    }

    // tile size
    public Vector2 GetTileDimensions()
    {
        return tileDimensions;
    }
    #endregion

    #region --- Column Ownership Functions ---

    // check if the player owns a tile
    public bool IsTileOwned(Vector2Int targetTile)
    {
        if (!(targetTile.x >= 0 && targetTile.x < gridDimensions.x))
        {
            // log when out of bounds
            Debug.LogWarning($"Coordinate {targetTile} is outside of grid bounds");
            return false;
        }
        return ownedColumns[targetTile.x];
    }

    // set a single column or sweep from left to right up to a column
    public void SetOwnedColumns(int column, bool newState = true, bool shouldSweep = false)
    {
        // avoid out of bounds index
        Mathf.Clamp(column, 0, ownedColumns.Length);

        // set a single column as owned
        if (!shouldSweep)
        {
            ownedColumns.SetValue(newState, column);
            return;
        }

        // set all columns left of target (inclusive)
        for (int i = 0; i < column; i++)
        {
            if (ownedColumns[i] == newState) // skip if already set
            {
                continue;
            }
            ownedColumns.SetValue(newState, i);
        }
    }
    #endregion

    #region --- Tile Availability Functions --

    // checks if a tile is marked as available
    public bool IsTileAvailable(Vector2Int targetTile)
    {
        // ensure that the tile exists and get the value if so
        bool isFound = availableTiles.TryGetValue(targetTile, out bool isAvailable);

        if (!isFound) // log when tile isnt found
        {
            Debug.LogWarning($"Coordinate {targetTile} not found in available tiles dict");
            return false;
        }

        return isAvailable;
    }

    // simply sets a tile as available/occupied
    public void SetTileAvailable(Vector2Int targetTile, bool isAvailable = true)
    {
        // check if tile exists
        bool isFound = availableTiles.TryGetValue(targetTile, out bool wasAvailable);

        if (!isFound)
        {
            Debug.LogWarning($"Coordinate {targetTile} not found in available tiles dict");
            return;
        }

        availableTiles[targetTile] = isAvailable;
    }
    #endregion

    #region --- Tile Position Functions ------
    public Vector2 GetTilePosition(Vector2Int targetTile)
    {
        bool isFound = gridPositions.TryGetValue(targetTile, out Vector2 tilePosition);

        if (!isFound)
        {
            Debug.LogWarning($"Coordinate {targetTile} not found in available tiles dict");
            return new Vector2(-1f,-1f); // temporary error return
        }

        return tilePosition;
    }

    public Vector2Int PosToCoord(Vector2 position)
    {
        int xConverted = Mathf.FloorToInt((position.x - gridOrigin.x) / tileDimensions.x);
        int yConverted = Mathf.FloorToInt((position.y - gridOrigin.y) / tileDimensions.y);
        return new Vector2Int(xConverted,yConverted);
    }

    public Vector2 CoordToPos(Vector2Int coord)
    {
        return coord * tileDimensions + gridOrigin;
    }
    #endregion
}
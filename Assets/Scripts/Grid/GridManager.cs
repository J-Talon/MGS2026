// Authors: [Jacky, Jeremy, Mark]
// Added viewing the grid boundaries: -Wilson Nov 1 2026
// Basic OnDrawGizmos implementation: -Jophiel Nov 8th 2026
//edited by Jordan - Nov 22, 2026 -> chagned enemy colours cause im colour blind
//Talon Nov 25th - for terraform system

using System;
using System.Collections.Generic;
using System.Data;
using Entities.Unit;
using JetBrains.Annotations;
using UnityEngine;

namespace Grid
{
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

        // Runtime state
        private GridState _state;
        private GridManipulator _manipulator;

        /// <summary>
        /// Function to get a grid cell at a specific position. Returns null if the position is out of bounds.
        /// </summary>
        /// <param name="position">XY coordinate of GridCell</param>
        /// <returns>GridCell object</returns>
        [CanBeNull]
        public GridCell GetTileAtPosition(Vector2Int position)
        {
            return _state.Cells.GetValueOrDefault(position,null);
        }


        public IGridSystemControl GetManipulator() {
            return _manipulator;
        }


        //todo replace with a call to entity manager instead as that is probably more robust than grid
        public List<(Vector2Int, EntityUnit)> FindAllEntities(Func<EntityUnit, bool> criteria) {
            
            var unitsWith = new List<(Vector2Int, EntityUnit)>();
        
            for (int x = _state.xMin; x < _state.xMax+1; x++) {
                for (int y = _state.yMin; y < _state.yMax+1; y++) {
                    var pos = new Vector2Int(x, y);
                    var cell = _state.Cells[pos];

                    if (cell.occupiedUnit != null && criteria.Invoke(cell.occupiedUnit))
                    {
                        unitsWith.Add((pos, cell.occupiedUnit));
                    }
                }
            }

            return unitsWith;
        }

        /// <returns>If coordinate exists within grid bounds</returns>
        public bool WithinGrid(Vector2Int position) {
            return _state.xMin <= position.x && position.x <= _state.xMax && _state.yMin <= position.y && position.y <= _state.yMax;
        }

        /// <returns>If coordinate exists within grid bounds</returns>
        public bool WithinGridSmooth(Vector2 position) {
            return _state.xMin-0.5 < position.x && position.x < _state.xMax + 0.5 && _state.yMin-0.5 < position.y && position.y < _state.yMax+0.5;
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
            return (new Vector2Int(_state.xMin, _state.yMin), new Vector2Int(_state.xMax, _state.yMax));
        }

        /// Function to get the bounds of the friendly grid
        /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
        public (Vector2Int min, Vector2Int max) GetFriendlyGridBounds()
        {
            return (new Vector2Int(_state.xMin, _state.yMin), new Vector2Int(_state.meridian, _state.yMax));
        }

        /// Function to get the bounds of the enemy grid
        /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
        public (Vector2Int min, Vector2Int max) GetEnemyGridBounds()
        {
            return (new Vector2Int(_state.meridian + 1, _state.yMin), new Vector2Int(_state.xMax, _state.yMax));
        }
    

        // Camera repositioning to center the grid in the scene view
        void CenterCamera()
        {
            cameraTransform.position = new Vector3((_state.xMin + _state.xMax) / 2f, (_state.yMin + _state.yMax) / 2f, -10f);
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

                    if (x == 0 && y == 0) { // check for player tile (usually first tile created)
                        Gizmos.color = new Color(0.28f, 0.98f, 0.85f, 1f);
                        Gizmos.DrawIcon(new Vector3(x, y, -1), "TestPlayerModel.png"); // function currently can't pull image
                    } else if (x < gridLevelData.startingFriendlyColumns) {    
                        // crafts player area; needs modification depending on 'Game' actions
                        Gizmos.color = Color.green;
                    } else if (x >= gridOOB) {          // check for main enemy last stand tiles
                        Gizmos.color = new Color(0.28f, 0.20f, 0.25f, 1f);
                    } else {                            // assume you are in enemy territory
                        Gizmos.color = new Color(0.41f, 0.31f, 0.39f, 1f); 
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
            if (instance != null && instance != this) {
                Destroy(gameObject);
                throw new DuplicateNameException("More than 1 grid manager in the scene!");
            }
            
            instance = this;

            // Initialize grid cell container
            gridCellContainer = new GameObject("Grid Cell Container");

            _state = new GridState();
            _manipulator = new GridManipulator(_state, gridCellPrefab, gridCellContainer.transform);
            _manipulator.Build(gridLevelData);

            if (debuggerEnable == true) {
                gameObject.AddComponent<GridManagerDebugger>();
            }
        
            CenterCamera();
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        void OnDestroy()
        {
            if (instance != this)
                return;
            
            instance = null;
        }
    
    }
}

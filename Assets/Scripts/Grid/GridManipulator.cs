// Authors: [Jacky, Jeremy, Mark]

using Event;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Grid
{
    /// <summary>
    /// Handles every change to the grid after it is created: territory control and adding/removing columns and rows.
    /// Instantiated as a base C# class by the grid manager on startup.
    ///
    /// Knows nothing about rendering; raises Changed after every operation so the grid manager can redraw.
    /// </summary>
    public class GridManipulator : IGridSystemControl
    {
        readonly GridState _state;
        readonly GridCell _cellPrefab;
        readonly Transform _cellContainer;
    
        //use the event system API we have
        // public event Action Changed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="state">grid data this manipulator changes</param>
        /// <param name="cellPrefab">prefab cloned for every new cell</param>
        /// <param name="cellContainer">parent of every cell in the hierarchy</param>
        public GridManipulator(GridState state, GridCell cellPrefab, Transform cellContainer)
        {
            this._state = state;
            this._cellPrefab = cellPrefab;
            this._cellContainer = cellContainer;
        }

        /// <summary>
        /// Set the starting bounds and territory from the level data and create every cell
        /// </summary>
        /// <param name="levelData">grid specific level data</param>
        public void Build(GridManagerLevelData levelData)
        {
            _state.xMin = levelData.xBoundingBoxMin;
            _state.xMax = _state.xMin + levelData.gridSize.x - 1; //-1 cause of 0 indexing
            _state.yMin = levelData.yBoundingBoxMin;
            _state.yMax = _state.yMin + levelData.gridSize.y - 1;
            _state.uncapturableEnemyColumns = levelData.uncapturableColumns;

            _state.meridian = ClampBorder(_state.xMin + levelData.startingFriendlyColumns - 1); //-1 cause of 0 indexing

            for (int x = _state.xMin; x <= _state.xMax; x++)
                CreateColumn(x);
            this.Redraw();
        }

        /// <summary>
        /// Change control of a single column
        /// 
        /// by default, increases "friendly" player territory by 1 col
        /// </summary>
        /// <param name="friendly"></param>
        /// <param name="columns"></param>
        /// @return: whether all desired columns were terraformed
        public bool AdvanceTerritory(bool friendly, int columns)
        {
            int index = 0;
            if (friendly) { 
                while (_state.meridian < _state.CaptureLimit && index < columns)
                {
                    index++;
                    _state.meridian += 1;
                    SetColumnOwnership(_state.meridian, true);
                }
            }
            else
            {
                while (_state.meridian >= _state.xMin && index < columns) {
                    index++;
                    SetColumnOwnership(_state.meridian, false);
                    _state.meridian -= 1;
                }
            }
        
            GameplayEvents.Grid.GridTerraformEvent.CallEvent((columns, _state.meridian, friendly));
            Redraw();
            return index >= columns;
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
                _state.xMax += 1;
            }
            else
            {
                bool isFriendly = _state.meridian >= _state.xMin; // player owns the current leftmost column
                _state.xMin -= 1;
                if (!isFriendly)
                    _state.meridian = _state.xMin - 1;
            }

            CreateColumn(side == GridSide.Right ? _state.xMax : _state.xMin);
            GameplayEvents.Grid.GridColumnDimensionUpdateEvent.CallEvent((side, _state));

        }

        /// <summary>
        /// Shrink the grid by one column on the given side. The grid always keeps at least one column.
        /// </summary>
        /// <param name="side">Edge of the grid to remove the column from</param>
        public bool RemoveGridColumn(GridSide side)
        {
            if (_state.xMin == _state.xMax)
                return false;
        

            if (side == GridSide.Right)
            {
                DestroyColumn(_state.xMax);
                _state.xMax -= 1;
            }
            else
            {
                DestroyColumn(_state.xMin);
                _state.xMin += 1;
            }

            // Removing on the right shifts the uncapturable columns left, removing on the left can empty
            // the player's territory; either way the border may need pulling back into range
            EnforceBorderLimits();
            GameplayEvents.Grid.GridColumnDimensionUpdateEvent.CallEvent((side, _state));
            return true;
        }

        /// <summary>
        /// Extend the grid by one row on the given side. Ownership of the new cells follows the current border.
        /// </summary>
        /// <param name="side">Edge of the grid to add the row to</param>
        public void AddGridRow(GridRowSide side)
        {
            if (side == GridRowSide.Top)
            {
                _state.yMax += 1;
                CreateRow(_state.yMax);
            }
            else
            {
                _state.yMin -= 1;
                CreateRow(_state.yMin);
            }
            GameplayEvents.Grid.GridRowDimensionUpdateEvent.CallEvent((side, _state));
        }

        /// <summary>
        /// Shrink the grid by one row on the given side. The grid always keeps at least one row.
        /// </summary>
        /// <param name="side">Edge of the grid to remove the row from</param>
        public bool RemoveGridRow(GridRowSide side)
        {
            if (_state.yMin == _state.yMax)
                return false;
        
            if (side == GridRowSide.Top)
            {
                DestroyRow(_state.yMax);
                _state.yMax -= 1;
            }
            else
            {
                DestroyRow(_state.yMin);
                _state.yMin += 1;
            }

            GameplayEvents.Grid.GridRowDimensionUpdateEvent.CallEvent((side, _state));
            return true;
        
        }

        // Instantiate a single grid cell at the given coordinate. Ownership follows the border.
        void CreateCell(int x, int y)
        {
            // Instantiate a new grid cell at the specified position
            GridCell newCell = Object.Instantiate(_cellPrefab, new Vector3(x, y, 0), Quaternion.identity, _cellContainer);
            // Name the cell for easier identification in the hierarchy
            newCell.name = $"GridCell_{x}_{y}";
            Vector2Int position = new Vector2Int(x, y);
            newCell.SetCellPosition(position);
            newCell.isFriendly = x <= _state.meridian;
            // Store the cell in the grid cells dictionary for easy access later
        
            // in interest of not rethreading the current architecture
            // columns will act more like markers rather than collections
        
        
            _state.Cells[position] = newCell;
        
        
        }

        // Destroy a single grid cell at the given coordinate, if it exists
        void DestroyCell(int x, int y)
        {
            Vector2Int coordinate = new Vector2Int(x, y);
            if (_state.Cells.TryGetValue(coordinate, out var cell))
            {
                Object.Destroy(cell.gameObject);
                _state.Cells.Remove(coordinate);
            }
        }

    
    
    
        // Instantiate a full column of grid cells at the given x coordinate
        void CreateColumn(int x)
        {
            for (int y = _state.yMin; y <= _state.yMax; y++)
                CreateCell(x, y);
        }

        // Destroy a full column of grid cells at the given x coordinate
        void DestroyColumn(int x)
        {
            for (int y = _state.yMin; y <= _state.yMax; y++)
                DestroyCell(x, y);
        }

        // Instantiate a full row of grid cells at the given y coordinate
        void CreateRow(int y)
        {
            for (int x = _state.xMin; x <= _state.xMax; x++)
                CreateCell(x, y);
        }

        // Destroy a full row of grid cells at the given y coordinate
        void DestroyRow(int y)
        {
            for (int x = _state.xMin; x <= _state.xMax; x++)
                DestroyCell(x, y);
        }

        // Set the ownership of every cell in a column
        void SetColumnOwnership(int x, bool isFriendly)
        {
            for (int y = _state.yMin; y <= _state.yMax; y++)
                _state.Cells[new Vector2Int(x, y)].isFriendly = isFriendly;
        }

        // Keep a border value between "no friendly columns" and the capture limit
        int ClampBorder(int value)
        {
            return Mathf.Max(_state.xMin - 1, Mathf.Min(value, _state.CaptureLimit));
        }

        // Pull the border back inside the allowed range after the grid changes shape,
        // giving any columns that fall outside it back to the enemy
        void EnforceBorderLimits()
        {
            int clamped = ClampBorder(_state.meridian);
            for (int x = clamped + 1; x <= _state.meridian; x++)
                SetColumnOwnership(x, false);
            _state.meridian = clamped;
        }
    
    
    
        public void Redraw()
        {
            // Iterate through grid cells
            for (int x = _state.xMin; x <= _state.xMax; x++)
            {
                for (int y = _state.yMin; y <= _state.yMax; y++)
                {
                    GridCell cell = _state.Cells[new Vector2Int(x, y)];
                    // Update the texture based on whether the cell is friendly or enemy and if it's offset
                    bool isOffset = ((x + y) & 1) == 1; // Simple checkerboard pattern for offset (& instead of % so negative coordinates work)
                    cell.UpdateTexture(isOffset);
                }
            }
        }
    }
}

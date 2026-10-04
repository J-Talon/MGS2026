// ===========================================
// Project: Terra Terralis 2026
// File: GridSystem.cs
// Author: Samyat Gautam (github: FadedBronze)
//
// Description: 
// The core grid system logic handles 
// positioning of tiles, thier sizes, gaps 
// between them, and moving gameobjects across 
// tiles.
// ===========================================

using UnityEngine;
using Event;
using UnityEngine.Assertions;
using System;

namespace GridSystem {

    /// <summary>
    /// Can be passed by [SerializeField] but mutations/updates done through GameplayEvents
    /// </summary>
    class GridManager : MonoBehaviour {
        [SerializeField]
        private Vector2 origin;

        [SerializeField]
        private float tileSize;

        [SerializeField]
        private int columns;
        [SerializeField]
        private int rows;

        public int Columns { get { return columns; } }
        public int Rows { get { return rows; } }
        public float TileSize { get { return tileSize; } }

        [SerializeField]
        private int maxRows;
        [SerializeField]
        private int maxColumns;

        private Tile[] tiles;

        [SerializeField]
        private Camera camera;

        [SerializeField]
        private GameObject tileGameObject;
        
        [SerializeField]
        private float gap;

        // ======== Setup ========

        public void Start() {
            tiles = new Tile[maxRows*maxColumns];

            for (int i = 0; i < maxRows; i++) {
                for (int j = 0; j < maxColumns; j++) {
                    Tile tile = new();
                    int idx = GridPositionToListIndex(new(j, i));
                    tiles[idx] = tile;
                    tile.gridPosition = new(j, i);
                }
            }
            
            SetupTiles();

            CenterGridInCamera(camera);

            GameplayEvents.moveGridEntity.AddEventListener(MoveGridEntityListener);
            GameplayEvents.placeGridEntity.AddEventListener(PlaceGridEntityListener);
            GameplayEvents.removeGridEntity.AddEventListener(RemoveGridEntityListener);
            GameplayEvents.resizeGrid.AddEventListener(ResizeGridListener);
        }

        bool revalidate = false;

        // runs whenever a property has changed in the inspector
        private void OnValidate() {
            revalidate = true; 
        }

        private void Update() {
            if (revalidate) {
                ResizeGridListener((rows, columns));
                revalidate = false;
            }
        }

        /// <summary>
        /// instantiates all tile prefabs then calls thier Init function function
        /// this is meant to run once at start
        /// </summary>
        public void SetupTiles() {
            int playerColumns = columns/2;

            for (int i = 0; i < maxRows; i++) {
                for (int j = 0; j < maxColumns; j++) {
                    TerritoryOwnership territory;
                    if (j < playerColumns) {
                        territory = TerritoryOwnership.Player;
                    } else {
                        territory = TerritoryOwnership.Enemy;
                    }

                    Tile tile = GetTile(new(j, i));
                    GameObject instance = Instantiate(tileGameObject);
                    instance.transform.SetParent(transform);

                    tile.gameTile = instance.GetComponent<GameTile>();
                    tile.gameTile.Init(territory);
                }
            }

            ResizeGridListener((rows, columns));
        }

        /// <summary>
        /// sets the grid origin such that the grid is centered in the camera
        /// </summary>
        private void CenterGridInCamera(Camera camera) {
            Assert.IsFalse(tileSize == 0);
            Assert.IsFalse(columns == 0);
            Assert.IsFalse(rows == 0);
            Assert.IsFalse(camera == null);

            // the bounds of the camera in screen pixel coordinates: [0, 0, width, height]
            Rect viewport = camera.pixelRect;

            // the minimum (minX, minY) and maximum (maxY, maxY) in world coordinates (depends on camera position in the world)
            Vector2 worldViewportMin = camera.ScreenToWorldPoint(viewport.min);
            Vector2 worldViewportMax = camera.ScreenToWorldPoint(viewport.max);

            // the size (width, height) in world coordinates
            Vector2 worldViewportSize = worldViewportMax - worldViewportMin;

            // in fractional values so you may have 1/2, 3/4, ect of a tile aswell
            float tilesFitInScreenX = worldViewportSize.x / tileSize;
            float tilesFitInScreenY = worldViewportSize.y / tileSize;
            
            float paddingTilesX = tilesFitInScreenX - columns;
            float paddingTilesY = tilesFitInScreenY - rows;

            // in order to center the grid
            float leftPadding = paddingTilesX * tileSize / 2;
            float topPadding = paddingTilesY * tileSize / 2;
            
            Vector2 padding = new(leftPadding, topPadding);

            origin = worldViewportMin + padding + tileSize * new Vector2(0.5f, 0.5f);
        }

        // ======== Listeners ========
        
        /// <summary>
        /// resizes the grid and updates tile positions, scale, and visibility (based on whether its inside the non-max bounds)
        /// </summary>
        public void ResizeGridListener((int new_rows, int new_columns) data) {
            Assert.IsTrue(data.new_rows <= maxRows && data.new_columns <= maxColumns);
            columns = data.new_columns;
            rows = data.new_rows;
            
            CenterGridInCamera(camera);

            for (int i = 0; i < maxRows; i++) {
                for (int j = 0; j < maxColumns; j++) {
                    Tile tile = GetTile(new(j, i));
                    tile.gameTile.Visible = i < rows && j < columns;
                    tile.gameTile.transform.position = GridToWorldPosition(new(j, i));
                    tile.gameTile.transform.localScale = new(tileSize-gap/2, tileSize-gap/2);
                }
            }

            GameplayEvents.gridResized.CallEvent(ValueTuple.Create());
        }

        public void MoveGridEntityListener((Vector2 position, Vector2 previousPosition) data) {
            var movingEntity = GetTile(data.previousPosition).entity;
            var destinationTile = GetTile(data.position);

            if (destinationTile.entity == null || destinationTile.entity == movingEntity) {
                destinationTile.entity = movingEntity;
            } else {
                Assert.IsTrue(false, "tile already has another entity");
            }
        }

        public void PlaceGridEntityListener((Vector2 position, GameObject gameObject) data) {
            var destinationTile = GetTile(data.position);

            if (destinationTile.entity == null) {
                destinationTile.entity = data.gameObject;
            } else {
                Assert.IsTrue(false, "tile already has another entity");
            }
        }
        
        public void RemoveGridEntityListener(Vector2 position) {
            var destinationTile = GetTile(position);

            if (destinationTile.entity == null) {
                Assert.IsTrue(false, "tile has no entity to remove");
            } else {
                destinationTile.entity = null;
            }
        }

        // ========= Utils =========

        public Vector2 WorldToGridPosition(Vector2 worldPosition) {
            Vector2 displacementFromOrigin = worldPosition - origin;
            Vector2 displacementFromOriginGrid = displacementFromOrigin / tileSize;
            return displacementFromOriginGrid;
        }
        
        public Vector2 GridToWorldPosition(Vector2 gridPosition) {
            return gridPosition * tileSize + origin;
        }

        public Vector2 ListIndexToGridPosition(int index) {
            return new(index%maxColumns, index/maxColumns); 
        }
        
        public int GridPositionToListIndex(Vector2 gridPosition) {
            return (int)Math.Floor(gridPosition.y) * maxColumns + (int)Math.Floor(gridPosition.x);
        }
        
        public Vector2 FindEntityGridPosition(GameObject entity, out bool success) {
            for (int i = 0; i < tiles.Length; i++) {
                if (tiles[i].entity == entity) {
                    success = true;
                    return ListIndexToGridPosition(i);
                }
            }
            success = false;
            return Vector2.zero;
        }

        public bool WithinBounds(Vector2 gridPosition) {
            float row = gridPosition.y;
            float column = gridPosition.x;

            return row <= rows-1 && column <= columns-1 && row >= 0 && column >= 0;
        }

        public static bool OnSameTile(Vector2 a, Vector2 b) {
            return (int)Math.Floor(a.x+0.5) == (int)Math.Floor(b.x+0.5) && (int)Math.Floor(a.y+0.5) == (int)Math.Floor(b.y+0.5);
        }
        
        public static bool PassedTileCenter(Vector2 a, Vector2 b) {
            return !OnSameTile(new(a.x+0.5f, a.y+0.5f), new(b.x+0.5f, b.y+0.5f));
        }

        private Tile GetTile(Vector2 gridPosition) {
            return tiles[GridPositionToListIndex(gridPosition)];
        }
        
        private Tile GetTile(int row, int column) {
            return tiles[row * maxColumns + column];
        }
    }
}

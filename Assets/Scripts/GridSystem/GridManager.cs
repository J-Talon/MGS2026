// ===========================================
// Project: Terra Terralis 2026
// File: GridSystem.cs
// Author: Samyat Gautam (github: FadedBronze)
// Description: TODO
// ===========================================

using UnityEngine;
using System.Collections.Generic;
using Event;
using UnityEngine.Assertions;

namespace GridSystem {

    /// <summary>
    /// TODO
    /// </summary>
    class GridManager : MonoBehaviour {
        [SerializeField]
        Vector2 origin;

        [SerializeField]
        private float tileSize = 0.0f;

        [SerializeField]
        private int columns;
        [SerializeField]
        private int rows;

        private Tile[] tiles;

        [SerializeField]
        Camera camera;

        [SerializeField]
        GameObject tileGameObject;
        
        // ======== Setup ========

        private void UpdateGrid() {
            CenterGridInCamera(camera);

            tiles = new Tile[rows*columns];
            for (int i = 0; i < rows*columns; i++) {
                Tile tile = new();
                tiles[i] = tile;
                tile.gridPosition = ListIndexToGridPosition(i);
            }

            SetupTiles();
        }
        
        public void Start() {
            UpdateGrid();

            GameplayEvents.moveGridEntity.AddEventListener(MoveGridEntityListener);
            GameplayEvents.placeGridEntity.AddEventListener(PlaceGridEntityListener);
            GameplayEvents.removeGridEntity.AddEventListener(RemoveGridEntityListener);
        }

        public void SetupTiles() {
            int playerColumns = columns/2;

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    Vector2 worldPosition = GridToWorldPosition(new(j, i));

                    TerritoryOwnership territory;
                    if (j < playerColumns) {
                        territory = TerritoryOwnership.Player;
                    } else {
                        territory = TerritoryOwnership.Enemy;
                    }

                    Tile tile = GetTile(new(j, i));
                    GameObject instance = Instantiate(tileGameObject);

                    tile.gameTile = instance.GetComponent<GameTile>();
                    tile.gameTile.Init(worldPosition, tileSize, territory);
                }
            }
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

            origin = worldViewportMin + padding + new Vector2(0.5f, 0.5f);
        }

        // ======== Listeners ========

        public void MoveGridEntityListener((Vector2 position, Vector2 previousPosition) data) {
            var movingEntity = GetTile(data.previousPosition).entity;
            var destinationTile = GetTile(data.position);

            if (destinationTile.entity == null) {
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

        private Vector2 WorldToGridPosition(Vector2 worldPosition) {
            Vector2 displacementFromOrigin = worldPosition - origin;
            Vector2 displacementFromOriginGrid = displacementFromOrigin / tileSize;
            return displacementFromOriginGrid;
        }
        
        private Vector2 GridToWorldPosition(Vector2 gridPosition) {
            return gridPosition * tileSize + origin;
        }

        private Tile GetTile(Vector2 gridPosition) {
            return tiles[GridPositionToListIndex(gridPosition)];
        }
        
        private Tile GetTile(int row, int column) {
            return tiles[row * columns + column];
        }

        public Vector2 ListIndexToGridPosition(int index) {
            return new(index%columns, index/columns); 
        }
        
        public int GridPositionToListIndex(Vector2 gridPosition) {
            return (int)gridPosition.y * columns + (int)gridPosition.x;
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
    }
}

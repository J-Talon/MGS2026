// ===========================================
// Project: Terra Terralis 2026
// File: GridSystem.cs
// Author: Samyat Gautam (github: FadedBronze)
// Description: TODO
// ===========================================

using UnityEngine;

namespace GridSystem {
    enum TerritoryOwnership {
        Player,
        Enemy,
    }
    
    class Tile {
        public GameObject entity;
        public GameTile gameTile;
        public Vector2 gridPosition;
    }
}

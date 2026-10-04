// ===========================================
// Project: Terra Terralis 2026
// File: Types.cs
// Author: Samyat Gautam (github: FadedBronze)
//
// Description: 
// Additional GridSystem types in another file 
// because unity seems to complain when there 
// is multiple classes in a MonoBehaviour 
// inheriting classes file
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

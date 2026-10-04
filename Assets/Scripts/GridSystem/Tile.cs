// ===========================================
// Project: Terra Terralis 2026
// File: Tile.cs
// Author: Samyat Gautam (github: FadedBronze)
// Description: Game Tile class is assigned to tile prefab this script can be changed for whatever tile/environment specific behaviour is required
// ===========================================

using UnityEngine;
using UnityEngine.Assertions;

namespace GridSystem {
    class GameTile : MonoBehaviour {
        public TerritoryOwnership territory;
        public SpriteRenderer renderer;

        public bool Visible {
            get {
                Assert.IsNotNull(renderer);
                return renderer.enabled;
            }
            set {
                Assert.IsNotNull(renderer);
                renderer.enabled = value;
            }
        }

        public void Init(TerritoryOwnership territory) {
            this.territory = territory;

            renderer = gameObject.GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);

            switch (this.territory) {
                case TerritoryOwnership.Enemy:
                    renderer.color = Color.purple;
                    break;
                case TerritoryOwnership.Player:
                    renderer.color = Color.yellowGreen;
                    break;
            }

            renderer.color = new Color(
                renderer.color.r*0.75f+Random.Range(0, 0.25f), 
                renderer.color.g*0.75f+Random.Range(0, 0.25f), 
                renderer.color.b*0.75f+Random.Range(0, 0.25f), 
            1.0f);
        }
    }    
}



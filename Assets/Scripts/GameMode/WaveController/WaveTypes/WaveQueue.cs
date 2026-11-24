// Author - Wilson -> Nov 15 2026
// Simple spawning.
// Entities are taken out the list one by one like a queue.
// Spawns the entity at a random location that is not currently occupied.

using UnityEngine;
using Gameplay.Core;
using Entities.Abstract;

namespace Gameplay.Wave
{
    public class WaveQueue : Wave
    {
        protected IGridSystemView gridManager;

        public WaveQueue (WeightedRandomEntity weightedEntities, int tokens, int maxEntities = 4, double spawnDelay = 0.5) : base(weightedEntities, tokens, maxEntities, spawnDelay)
        {
            gridManager = GridManager.instance;
        }

        protected override (EntityType type, Vector2Int pos) GetNextSpawn()
        {
            if (spawnList.Count > 0)
            {
                EntityType lastItem = spawnList[^1];
                spawnList.RemoveAt(spawnList.Count - 1);

                return (lastItem, GetUnoccupiedEnemyCell().GetCellPosition());
            }

            return (null, default);
        }

        // This could be moved to the Grid Manager, however only this class needs it as of now so maybe not.
        private GridCell GetUnoccupiedEnemyCell()
        {
            var enemyBounds = gridManager.GetEnemyGridBounds();
            Vector2Int min = enemyBounds.min;
            Vector2Int max = enemyBounds.max;

            bool hasSpace = false;
            for (int y = min.y; y <= max.y; y++)
            {
                for (int x = min.x; x <= max.x; x++)
                {
                    if (gridManager.GetTileAtPosition(new Vector2Int(x, y)).GetIsOccupied() == false) {
                        hasSpace = true;
                        break;
                    }
                }
            }

            if (!hasSpace)
            {
                return null;
            }

            GridCell cell = null;

            do
            {
                int randomX = Random.Range(min.x, max.x);
                int randomY = Random.Range(min.y, max.y);
                cell = gridManager.GetTileAtPosition(new Vector2Int(randomX, randomY));
            } while (cell.GetIsOccupied() == true);

            return cell;
        }
    }
}

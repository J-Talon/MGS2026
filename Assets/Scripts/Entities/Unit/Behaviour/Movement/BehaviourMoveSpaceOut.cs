// Author: Jeremy
using UnityEngine;
using Entities.Abstract;
using System.Collections.Generic;
// using System.Text;

namespace Entities.Unit.Behaviour.Movement
{
    public class BehaviourMoveSpaceOut : EntityBehaviour
    {
        public override BehaviourAction BehaviourAction { get; } = BehaviourAction.Move;
        public override BehaviourType BehaviourType { get; } = BehaviourType.Neutral;
        public override BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.Medium;
        private readonly IGridSystemView gridManager = GridManager.instance;
        public int searchRange = 2;

        public override bool Tick()
        {
            if (!base.RollProficiency()) return false;
            return MoveSpaceOut();
        }

        // Space out in a way that the unit tries to occupy the entire zone
        private bool MoveSpaceOut()
        {
            Vector2Int currentPosition = Vector2Int.RoundToInt(unit.GetPosition());
            // Create heatmap
            int[,] heatmap = CreateHeatMap();
            // Find closest least populated cell
            Vector2Int nearestTargetCell = FindNearestQuietCell(heatmap, currentPosition, searchRange: searchRange);
            // Move towards target position
            return AttemptMove(currentPosition, nearestTargetCell);
        }

        /// <summary>
        /// Create heatmap of enemy units within enemy bounds
        /// </summary>
        /// <returns></returns>
        private int[,] CreateHeatMap()
        {
            IReadOnlyCollection<Entity> enemyUnits = EntityManager.GetInstance().GetEnemyUnits();
            (Vector2Int bottomLeft, Vector2Int topRight) = gridManager.GetEnemyGridBounds();

            // Create a heatmap array
            int[,] heatmap = new int[topRight.x - bottomLeft.x + 1, topRight.y - bottomLeft.y + 1]; // +1 because bounds are inclusive

            // Populate heatmap by looping through all enemies
            foreach (Entity enemy in enemyUnits)
            {
                // Skip itself
                if (enemy.GetEntityID() == this.unit.GetEntityID()) continue;

                Vector2Int position = Vector2Int.RoundToInt(enemy.GetPosition());
                int x = position.x - bottomLeft.x; // offset from world coordinates border
                int y = position.y - bottomLeft.y; // offset here is typically 0, but may change in the future

                // Add heat value for occupied position and surrounding in diamond pattern
                // Looks like this:
                // . . 1 . .
                // . 1 2 1 .
                // 1 2 3 2 1
                // . 1 2 1 .
                // . . 1 . .
                // Center
                AddToHeatmap(heatmap, x, y, 3);
                // First ring
                AddToHeatmap(heatmap, x + 1, y, 2);
                AddToHeatmap(heatmap, x - 1, y, 2);
                AddToHeatmap(heatmap, x, y + 1, 2);
                AddToHeatmap(heatmap, x, y - 1, 2);
                // Outer ring
                AddToHeatmap(heatmap, x + 2, y, 1);
                AddToHeatmap(heatmap, x - 2, y, 1);
                AddToHeatmap(heatmap, x, y + 2, 1);
                AddToHeatmap(heatmap, x, y - 2, 1);
                AddToHeatmap(heatmap, x + 1, y + 1, 1);
                AddToHeatmap(heatmap, x - 1, y - 1, 1);
                AddToHeatmap(heatmap, x + 1, y - 1, 1);
                AddToHeatmap(heatmap, x - 1, y + 1, 1);
            }
            return heatmap;
        }

        /// <summary>
        /// Find the nearest coldest cell to move to
        /// </summary>
        private Vector2Int FindNearestQuietCell(int[,] heatmap, Vector2Int currentPosition, int searchRange)
        {
            (Vector2Int bottomLeft, _) = gridManager.GetEnemyGridBounds();
            int x = currentPosition.x - bottomLeft.x; // offset from world coordinates border
            int y = currentPosition.y - bottomLeft.y; // offset here is typically 0, but may change in the future
            if (!InHeatmap(heatmap, x, y)) return currentPosition; // unit is outside the enemy zone. shouldn't happen

            int bestHeat = heatmap[x, y]; // a cell has to beat the current one to be worth moving to
            if (bestHeat == 0) return currentPosition; // can't do better than 0

            List<Vector2Int> best = new List<Vector2Int>(); // coldest cells found so far
            List<Vector2Int> ring = new List<Vector2Int>(); // coldest cells in the current ring

            // Search ring by ring (nearest first)
            for (int range = 1; range <= searchRange; range++)
            {
                int ringHeat = int.MaxValue;
                ring.Clear();

                for (int dx = -range; dx <= range; dx++)
                {
                    int dy = range - Mathf.Abs(dx);
                    CollectColdest(heatmap, new Vector2Int(x + dx, y + dy), ref ringHeat, ring);
                    if (dy != 0) // dy == 0 means +dy and -dy are the same cell
                        CollectColdest(heatmap, new Vector2Int(x + dx, y - dy), ref ringHeat, ring);
                }

                // Only a strictly colder ring replaces the best, so ties keep the nearer ring
                if (ringHeat < bestHeat)
                {
                    bestHeat = ringHeat;
                    (best, ring) = (ring, best); // swap: 'best' now holds this ring's cells
                }

                if (bestHeat == 0) break; // nothing further out can beat 0
            }

            // nothing colder than the current cell within range, stay
            if (best.Count == 0) return currentPosition;

            // All best cells are equally cold and equally near, so pick randomly to avoid bias
            return best[Random.Range(0, best.Count)] + bottomLeft; // heatmap index -> world
        }

        /// <summary>
        /// Keeps 'cells' holding only the coldest cells seen in this ring
        /// </summary>
        private void CollectColdest(int[,] heatmap, Vector2Int index, ref int coldest, List<Vector2Int> cells)
        {
            if (!InHeatmap(heatmap, index.x, index.y)) return;
            int heat = heatmap[index.x, index.y];

            if (heat < coldest) // new coldest: forget the warmer ones
            {
                coldest = heat;
                cells.Clear();
                cells.Add(index);
            }
            else if (heat == coldest) // tie: keep both
            {
                cells.Add(index);
            }
        }


        /// <summary>
        /// Try to add to the heatmap
        /// </summary>
        private void AddToHeatmap(int[,] heatmap, int x, int y, int val)
        {
            if (InHeatmap(heatmap, x, y)) heatmap[x, y] += val;
        }

        /// <summary>
        /// Checks if indexes are valid in heatmap
        /// </summary>
        private bool InHeatmap(int[,] heatmap, int x, int y)
        {
            return x >= 0 && x < heatmap.GetLength(0) && y >= 0 && y < heatmap.GetLength(1);
        }

        /// <summary>
        /// Attempt to move towards cell
        /// </summary>
        private bool AttemptMove(Vector2Int currentPosition, Vector2Int targetPosition)
        {
            Vector2Int delta = targetPosition - currentPosition;
            if (delta == Vector2Int.zero) return false; // already there

            // One step along each axis toward the target (zero if already lined up on that axis)
            Vector2Int stepX = new Vector2Int(System.Math.Sign(delta.x), 0);
            Vector2Int stepY = new Vector2Int(0, System.Math.Sign(delta.y));

            // Try the axis with more distance remaining first
            bool xFirst = Mathf.Abs(delta.x) >= Mathf.Abs(delta.y);
            Vector2Int first = xFirst ? stepX : stepY;
            Vector2Int second = xFirst ? stepY : stepX;

            // Both steps are on a shortest path, so if the first is blocked, try the other
            if (first != Vector2Int.zero && unit.TryMoving(first)) return true;
            if (second != Vector2Int.zero && unit.TryMoving(second)) return true;
            return false;
        }

        // public void Print2DArray<T>(T[,] array)
        // {
        //     StringBuilder sb = new StringBuilder();

        //     // GetLength(0) is rows, GetLength(1) is columns
        //     for (int row = 0; row < array.GetLength(0); row++)
        //     {
        //         for (int col = 0; col < array.GetLength(1); col++)
        //         {
        //             sb.Append(array[row, col]);
        //             sb.Append(" "); // Add spacing between columns
        //         }
        //         sb.AppendLine(); // Move to the next line after each row
        //     }

        //     Debug.Log(sb.ToString());
        // }
    }
}

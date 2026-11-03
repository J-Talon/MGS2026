// Authors: [Jacky, Jeremy, Mark]

using System;
using Entity.Abstract;
using Entity.Unit.Behaviour.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Entity.Unit
{
    /// <summary>
    /// Base unit class. Will make use of composition for specific behaviours
    /// </summary>
    public class EntityUnit : Abstract.Entity, IUnitMoveSnappy, IUnitMoveSmooth, IUnitFire
    {
        
        
        /// <summary>
        /// Validates passed in firing data then creates projectile from factory
        /// </summary>
        /// <param name="deltaFiringPosition">The relative firing position to the unit</param>
        /// <param name="projectileType">actual unit to spawn</param>
        public void FireProjectile(Vector2Int deltaFiringPosition, EntityType projectileType)
        {
            Vector2 firePos = gameObject.transform.position + (Vector3Int)deltaFiringPosition;
            IGridSystemView gridManager = GridManager.instance;
            GridCell newCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(firePos));
            if (newCell != null) //new cell exists
                EntityManager.GetInstance().GetEntity(projectileType, new Vector3(firePos.x, firePos.y, -1), isFriendly);
        
        }

        /// <summary>
        /// Implementation of continuous movement
        /// </summary>
        /// <param name="deltaMovement"></param>
        public void MoveUnitSmooth(Vector2 deltaMovement)
        {
            gameObject.transform.position = gameObject.transform.position + (Vector3)deltaMovement;
        }



        [CanBeNull]
        public GridCell GetCurrentCell()
        {
            Vector2 pos = gameObject.transform.position;
            Vector2Int rounded = Vector2Int.RoundToInt(pos);
            return GridManager.instance.GetTileAtPosition(rounded);
        }


        /// <summary>
        /// Implementation of snappy based movement (int)
        /// also registers self to grid cell while also preventing movement if new grid cell is occupied
        /// To account for possible 0 delta movement, unregistration is done first to grid cell before registration to avoid unit accidentlaly unoccuppying its current cell
        ///
        /// Talon J: Add return true or false for whether the movement was successful
        /// </summary>
        /// <param name="deltaMovement"></param>
        public bool MoveUnitSnappy(Vector2Int deltaMovement)
        {
            Vector2 oldPos = gameObject.transform.position;
            Vector2 newPos = gameObject.transform.position + (Vector3Int)deltaMovement;
            IGridSystemView gridManager = GridManager.instance;
            GridCell newCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(newPos));
            if (newCell.isOccupied == false) //new cell is not occupied, can move there
            {
                GridCell oldCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(oldPos));
                oldCell.EntityUnitLeave(); //unregister from old cell

                newCell.EntityUnitOccupy(this); //register self to new cell
                gameObject.transform.position = newPos;
                return true;
            }
            return false;
        }



        public override void OnStart()
        {
            IGridSystemView gridManager = GridManager.instance;   
            GridCell cell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(gameObject.transform.position));
            cell.EntityUnitOccupy(this); //register self to new cell
        }
    
    

        public override void Die()
        {
            IGridSystemView gridManager = GridManager.instance;
            GridCell gridPosition = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(gameObject.transform.position));
            gridPosition.EntityUnitLeave(); //unregister from grid cell
            base.Die();
        }


        public override void Tick()
        {
            if (!this.isAlive) return;

            if (!this.isInvulnerable && this.currentHealth <= 0) {
                this.Die();
            }
        }
    
    
        public bool MoveUp()
        {
            return TryMoving(Vector2Int.up);
        }

        public bool MoveRight()
        {
            return TryMoving(Vector2Int.right);
        }

        public bool MoveDown()
        {
            return TryMoving(Vector2Int.down);
        }

        public bool MoveLeft()
        {
            return TryMoving(Vector2Int.left);
        }

        // function to try to move in a direction, if it can it will
        protected bool TryMoving(Vector2Int dir)
        {
            Vector2Int currentPosition = Vector2Int.RoundToInt(GetPosition());
            Vector2Int newPosition = currentPosition + dir;   

            IGridSystemView gridView = GridManager.instance;
            GridCell targetTile = gridView.GetTileAtPosition(newPosition);

            if (targetTile != null && !targetTile.isFriendly && !targetTile.isOccupied)
            {
                MoveUnitSnappy(newPosition-currentPosition);
                return true;
                // Debug.Log("Valid move: " + dir.dir);
            }
            return false;
        }
    }
}
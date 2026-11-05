// Authors: [Jacky, Jeremy, Mark]
// Added collectable entity check - Wilson October 26 2026


using Effect.Abstract;
using Entities.Abstract;
using Entities.Collectable;
using Entities.Unit.Behaviour.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Entities.Unit
{
    /// <summary>
    /// Base unit class. Will make use of composition for specific behaviours
    /// </summary>
    public class EntityUnit : Abstract.Entity, IUnitMoveSnappy, IUnitMoveSmooth, IUnitFire
    {
        
        //(Talon J) effect system
        protected LocalizedEffectManager<EntityEffect> effectManager;
        
        public override void OnStart()
        {
            effectManager = new  LocalizedEffectManager<EntityEffect>();
            
            IGridSystemView gridManager = GridManager.instance;   
            GridCell cell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(gameObject.transform.position));
            cell.EntityUnitOccupy(this); //register self to new cell
        }

        
        
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


        public bool AddEffect(EntityEffect effect) {
            return this.effectManager.AddEffect(effect);
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
        
        
        
        public sealed override bool TakeDamage(float attackDamage) {
            return this.TakeDamage(attackDamage, null);
        }


        public virtual bool TakeDamage(float attackDamage, [CanBeNull] Entity source)
        {
            if (!isAlive || isInvulnerable || attackDamage <= 0f) return false;
            
            //(Talon J) effect system
            bool isCancelled = false;
            effectManager.EffectCallback((effect) => { effect.PreOnEntityDamage(source, ref attackDamage, ref isCancelled); });
            if (isCancelled) return false;
            
            currentHealth -= attackDamage;
            if (currentHealth <= 0f) Die();
            return true;
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
            
            
            if (newCell.isOccupied) //new cell is not occupied, can move there
                return false;
            
            GridCell oldCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(oldPos));
            
             //   (Talon J) effect system
             bool isCancelled = false;
             effectManager.EffectCallback((effect) => { effect.PreOnGridMove(oldCell, newCell, deltaMovement, ref isCancelled); });
             if (isCancelled) return false;
            
            
            oldCell.EntityUnitLeave(); //unregister from old cell

            CollectableEntity collectable = newCell.occupiedCollectable;
            if (collectable != null)
            {
                collectable.OnInteract(this);
            }
            
            newCell.EntityUnitOccupy(this); //register self to new cell
            gameObject.transform.position = newPos;
            return true;
                
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
            
            effectManager.Tick(Time.deltaTime);
            
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
        public bool TryMoving(Vector2Int dir)
        {
            Vector2Int currentPosition = Vector2Int.RoundToInt(GetPosition());
            Vector2Int newPosition = currentPosition + dir;   

            IGridSystemView gridView = GridManager.instance;
            GridCell targetTile = gridView.GetTileAtPosition(newPosition);

            if (targetTile != null && targetTile.isFriendly == this.isFriendly && !targetTile.isOccupied)
            {
                MoveUnitSnappy(newPosition-currentPosition);
                return true;
                // Debug.Log("Valid move: " + dir.dir);
            }
            return false;
        }
    }
}
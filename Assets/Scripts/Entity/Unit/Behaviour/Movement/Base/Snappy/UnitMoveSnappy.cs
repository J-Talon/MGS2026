// Authors: [Jacky, Jeremy, Mark]

using UnityEngine;

namespace Entity.Unit.Behaviour.Movement.Base.Snappy
{
    /// <summary>
    /// Base class for snappy movement behaviour of units. This class is intended to be inherited by specific movement behaviour implementations.
    /// </summary>
    [RequireComponent(typeof(EntityUnit))]
    public abstract class UnitMoveSnappy : EntityBehaviour
    {
        
 
        private float movementCooldown;

        void Start()
        {
            movementCooldown = unit.GetMovementTickTimer(); //before we even move, inherit the movement cooldown
        }

        public override void Tick()
        {
            if (movementCooldown > 0)
            {
                movementCooldown -= Time.deltaTime;
            }
            else
            {
                movementCooldown = unit.GetMovementTickTimer();
                unit.MoveUnitSnappy(MovementRoutine());
            }
        }

        /// <summary>
        /// The actual movement behaviour to be implemented in a concrete class implementation.
        /// </summary>
        /// <returns></returns>
        protected abstract Vector2Int MovementRoutine();
    }
}
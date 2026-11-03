// Authors: [Jacky, Jeremy, Mark]

using UnityEngine;

namespace Entity.Unit.Behaviour.Movement.Base.Smooth
{
    /// <summary>
    /// Base class for smooth movement behaviour of units. This class is intended to be inherited by specific movement behaviour implementations.
    /// </summary>
    [RequireComponent(typeof(EntityUnit))]
    [System.Obsolete("This script is deprecated. Use methods found in EntityUnit")]
    public abstract class UnitMoveSmooth : EntityBehaviour
    {

        public override void Tick()
        {
            this.unit.MoveUnitSmooth(MovementRoutine());
        }
        
        /// <summary>
        /// The actual movement behaviour to be implemented in a concrete class implementation.
        /// </summary>
        protected abstract Vector2 MovementRoutine();
    }
}
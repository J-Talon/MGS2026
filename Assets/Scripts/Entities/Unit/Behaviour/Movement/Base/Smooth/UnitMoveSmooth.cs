// Authors: [Jacky, Jeremy, Mark]

using UnityEngine;

namespace Entities.Unit.Behaviour.Movement.Base.Smooth
{
    /// <summary>
    /// Base class for smooth movement behaviour of units. This class is intended to be inherited by specific movement behaviour implementations.
    /// </summary>
    [RequireComponent(typeof(EntityUnit))]
    [System.Obsolete("This script is deprecated. Use methods found in EntityUnit")]
    public abstract class UnitMoveSmooth : EntityBehaviour
    {

        public override bool Tick()
        {
            this.unit.MoveUnitSmooth(MovementRoutine());
            return true; // Always return true since smooth movement is continuous and does not have a cooldown
        }

        /// <summary>
        /// The actual movement behaviour to be implemented in a concrete class implementation.
        /// </summary>
        protected abstract Vector2 MovementRoutine();
    }
}
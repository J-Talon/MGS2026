//author jacky

using System;
using Entity.Abstract;
using UnityEngine;

namespace Entity.Unit.Behaviour.Attack
{
    /// <summary>
    /// Control firing behaviour of a unit based on various factors. Currently allows for firing on cooldown + holding fire (while checking every frame if can fire again)
    /// 
    /// Actual firing not handled here, instead data is passed to the attached entity unit for actual projectile creation
    /// </summary>
    public abstract class BehaviourUnitFire : EntityBehaviour
    {
        [field: SerializeField] protected EntityType projectileType;

        [field: SerializeField] protected int fireCooldown;

  
        #region projectile instance data

        // /// <summary>
        // /// Cooldown time before firing
        // /// </summary>
        private float _timeLeft; //trigger firing immedietly?

        /// <summary>
        /// If unit chose to not fire during a cycle, instead run a firing check everyy frame now
        /// </summary>
        private bool heldFire = false;

        #endregion


        
        protected override void Initialize() {
            this._timeLeft = fireCooldown;
        }



        /// <summary>
        /// Main loop, decrements timer when still on firing cooldown
        /// Else, resets timer then calls the actual firing behaviour defined in a actual concrete class
        /// 
        /// IF unit chose not to fire previous cycle, also trigger firing routine check every frame until it does fire
        /// </summary>
        public override void Tick()
        {
            if (_timeLeft > 0 && heldFire == false)
            {
                _timeLeft -= Time.deltaTime;
            }
            else
            {
                _timeLeft = fireCooldown;

                Tuple<Vector2Int, EntityType, bool> firedData = FireRoutine();

                if (firedData.Item3 == true)
                {
                    heldFire = false;
                    unit.FireProjectile(firedData.Item1, firedData.Item2);
                }
                else
                {
                    heldFire = true;
                }
            
            }
        }


        /// <summary>
        /// The actual firing behaviour to be implemented in a concrete class implementation.
        /// 
        /// Return tuple stating the spawn position, projectile type, and if spawning is even allowed for this fire cycle
        /// </summary>
        protected abstract Tuple<Vector2Int, EntityType, bool> FireRoutine();
    }
}

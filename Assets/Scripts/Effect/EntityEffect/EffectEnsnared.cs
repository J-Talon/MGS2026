using Effect.Abstract;
using Entities.Unit;
using UnityEngine;


/*
 * Authors Sayeed, Talon
 *
 */
namespace Effect.EntityEffect
{
    public class EffectEnsnared: Abstract.EntityEffect
    {
        private const double DAMAGE_INTERVAL = 0.75;
        private const float DAMAGE_AMOUNT = 0.5f;

        private double damageTimer = 0;
        
        public EffectEnsnared(EntityUnit affector, double duration, bool finite = true) : base(affector, duration, finite) { }

        public override void OnStart() {
            //todo add ensnared vfx here
        }

        public override void OnEnd() {
            //todo remove ensnared vfx here
        }

        public override void Tick(double deltaTime)
        {
            damageTimer += deltaTime;

            while (damageTimer >= DAMAGE_INTERVAL)
            {
                affector.TakeDamage(DAMAGE_AMOUNT, null);
                damageTimer -= DAMAGE_INTERVAL;
            }
            base.Tick(deltaTime);
        }

        public override EffectType GetEffectType()
        {
            return EffectType.ENTITY_MOVEMENT_LOCK;
        }

        public override void PreOnGridMove(GridCell oldCell, GridCell newCell, Vector2Int moveDir, ref bool isCancelled) {
            isCancelled = true;
        }
    }
}

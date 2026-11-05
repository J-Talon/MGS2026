using Effect.Abstract;
using Entities.Unit;
using UnityEngine;

namespace Effect.EntityEffect
{
    public class EffectEnsnared: Abstract.EntityEffect {
        
        public EffectEnsnared(EntityUnit affector, double duration, bool finite = true) : base(affector, duration, finite) { }

        public override void OnStart() {
            //todo add ensnared vfx here
        }

        public override void OnEnd() {
            //todo remove ensnared vfx here
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
using Effect.Abstract;
using Effect.EntityEffect;
using Entities.Unit;
using Particle.Base;
using UnityEngine;

namespace Effect.GridEffect
{
    public class EffectBurning: Abstract.GridEffect
    {

        private ParticleWrapperBase particle;
        
        public EffectBurning(GridCell affector, double duration, bool finite) : base(affector, duration, finite) { }

        public override void OnStart() {
            Vector3 position = affector.transform.position;
            particle = ParticleType.FIRE.Play(position.x, position.y, position.z);
            particle.SetScaledParent(affector.transform);
        }

        public override void OnEnd() {
            particle.GetParticleSystem().Stop();
        }


        public override void OnEntityEnter(EntityUnit unit) {
            unit.AddEffect(new EffectOnFire(unit, duration));
        }


        public override EffectType GetEffectType() {
            return EffectType.GRID_BURNING;
        }
    }
}
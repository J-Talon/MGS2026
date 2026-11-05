using Effect.Abstract;
using Entities.Unit;
using Particle.Base;
using UnityEngine;

namespace Effect.EntityEffect
{
    //mmmm hotdogs
    public class EffectOnFire: Abstract.EntityEffect
    {
        private ParticleWrapperBase particle;
        private double time;
        private const double DAMAGE_TIMER = 1;
        
        
        public EffectOnFire(EntityUnit affector, double duration, bool finite = true) : base(affector, duration, finite) { }

        public override void OnStart()
        {
            Vector3 position = affector.transform.position;
            particle = ParticleType.FIRE.Play(position.x, position.y, position.z);
            particle.SetScaledParent(affector.transform);
        }

        public override void OnEnd()
        {
            //particles are auto returned when all particles die
            particle.GetParticleSystem().Stop();
        }

        public override void Tick(double deltaTime)
        {
            base.Tick(deltaTime);
            time += deltaTime;
            if (time >= DAMAGE_TIMER) {
                time = 0;
                affector.TakeDamage(0.5f, null);
            }
        }


        public override EffectType GetEffectType() {
            return EffectType.ENTITY_BURNING;
        }
    }
}
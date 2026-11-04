using System;
using Particle.Base;
using UnityEngine;

namespace Entities.Projectile.ProjectileTypes
{
    /// <summary>
    /// Peashooter Spell
    /// </summary>
    public class PeaBullet : EntityProjectile
    {
        public float wiggleFactor;
        public float wiggleSpeed;
        private float randPhase = 0;


        public override void OnStart() {
            randPhase = UnityEngine.Random.Range(0, (float)Math.PI*2);
        }



        public override void Die()
        {
            //ParticleType.DEBUG.Play(transform.position.x, transform.position.y, -1f);
            // ParticleType.SPARK.Play(transform.position.x, transform.position.y, -1.0f);
            base.Die();
        }

        public override void Tick()
        {
            base.Tick();

            var dtime = (double)Time.time;

            // slight perpindicular movement to make the bullet look more interesting
            // Vector2 wiggle = new(0, 
            //     (float)Math.Sin(
            //         dtime*Math.PI*2*speed*wiggleSpeed + randPhase
            //     )*
            //     Math.Clamp(wiggleFactor, 0, 0.5f)
            // );

            MoveProjectileSmooth(new Vector2(1, 0)*speed);

            // check if enemy is at position and damage them and disappear if it is
            var entity = GridManager.instance.GetEntityAtTilePosition(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        

            if (entity != null && !entity.GetIsFriendly()) {
                entity.TakeDamage(damage);
                GridCell cell = entity.GetCurrentCell();
              
                if (cell == null) {
                    Die();
                    return;
                }

                Vector2Int vec = cell.GetCellPosition();
                ParticleType.SPARK.Play(vec.x, vec.y, -1);
                Die();
            }
        }
    }
}

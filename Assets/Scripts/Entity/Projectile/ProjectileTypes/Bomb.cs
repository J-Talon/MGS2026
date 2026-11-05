using System;
using Particle.Base;
using UnityEditor;
using UnityEngine;

namespace Entity.Projectile.ProjectileTypes
{
    class Bomb : EntityProjectile {
        public Vector2Int target;
        public Vector2Int start;
        public float height;
        private float nextHeight;

        public override void OnStart() {
            var h = UnityEngine.Random.Range(height, height/2);

            if (UnityEngine.Random.Range(0, 1) > 0.5) {
                h *= -1;
            }

            nextHeight = h;
        }
    

        Vector2 CalculateTrajectory(Vector2Int start, Vector2Int end, float height, float time) {
            var startVec = new Vector2(start.x, start.y);
            var endVec = new Vector2(end.x, start.y);
            var delta = endVec - startVec;

            float deltaY = (new Vector2(0, end.y) - new Vector2(0, start.y)).y;

            // the total y distance scaled by the height
            var normalizedEndY = deltaY / height;

            // get a value perpendicular to direct path from start to end in order to create the illusion of fligh
            var tangent = Vector2.Perpendicular(delta.normalized);

            //default arc that basically just has a vertex at y = 1
            float arc = 4.0f * time * (1.0f - time);

            //line that goes from start point to endpoint
            float riseY = normalizedEndY * time;

            // basically prevents a super high porabala when below
            var timeCurved = arc * (1.0f - normalizedEndY) + riseY;

            return tangent * (float)timeCurved * height + delta * time + startVec;
        }

        void DamageTile(IGridSystemView gridView, Vector2Int pos) {
            var entity = gridView.GetEntityAtTilePosition(pos);

            if (entity) {
                entity.TakeDamage(damage);
            }
        }

        public override void Die() {
            ParticleType.SPARK.Play(target.x, target.y, -1);

            IGridSystemView gridView = GridManager.instance;
            DamageTile(gridView, target);
            DamageTile(gridView, target + new Vector2Int(0, 1));
            DamageTile(gridView, target + new Vector2Int(0, -1));
            DamageTile(gridView, target + new Vector2Int(1, 0));
            DamageTile(gridView, target + new Vector2Int(-1, 0));

            base.Die();
        }

        public override void Tick() { 
        
            base.Tick();
            var lifeSpan = Time.time - spawnTime;

            Debug.Assert(maxLifeSpan != 0);

            var deathPercent = lifeSpan / maxLifeSpan;

            // update position based on trajectory
            var traj = CalculateTrajectory(start, target, nextHeight, deathPercent);
            transform.position = new Vector3(traj.x, traj.y, -1);
        }
    }
}

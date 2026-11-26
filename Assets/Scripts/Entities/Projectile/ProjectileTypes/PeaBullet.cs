//who the fuck authored this file?
//edits made by Jordan - Nov 20, 2026
// I believe it was Samyat. - Talon. See Entity-Projectile Handling P2 from W4.

using Grid;
using Particle.Base;
using UnityEngine;

namespace Entities.Projectile.ProjectileTypes
{
    /// <summary>
    /// Peashooter Spell
    /// </summary>
    public class PeaBullet : EntityProjectile
    {
        private Vector2 direction = new Vector2(1, 0);
        public void SetDirection(Vector2 value) { direction = value; }
        
        

        public override void Tick()
        {
            base.Tick();

            if (isFriendly == true)
            {
                MoveProjectileSmooth(new Vector2(1, 0) * speed);
            }

            else
            {
                MoveProjectileSmooth(new Vector2(-1, 0) * speed);
            }

            // check if enemy is at position and damage them and disappear if it is
            var entity = GridManager.instance.GetEntityAtTilePosition(Vector2Int.RoundToInt(transform.position));

            if (entity != null && IsHostile(entity) == true)
            {
                entity.TakeDamage(damage);
                GridCell cell = entity.GetCurrentCell();

                if (cell == null)
                {
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

/*
 Changelog

Jordan - Nov 20, 2026

changed the hardcoded right direction of the MoveProjectileSmooth to check team allegiance, and return a left or right travelling pea depending. 
also fixed the team allegiance of the pea to use IsHostile() instead of GetIsFriendly(). Enemies can use peas too, so its a hostility check, not if its an enemy check

Jeremy - Nov 23 
Modified class to implement bullet enemies

Talon (Same date) - Cleaned up orphan data
 
*/

// Authors: [Jacky, Jeremy, Mark]

using System;
using Entities.Projectile.Interfaces;
using UnityEngine;

namespace Entities.Projectile.ProjectileBehaviours.ProjectileOnHit
{
    /// <summary>
    /// to be implemented later, collisions and other stuff is still undecided by game design
    /// </summary>
    [Obsolete("Inline this into the projectile class")]
    public class ProjectileOnHitBasic : MonoBehaviour, IProjectileOnHit
    {
        public void OnHitMapBorder()
        {
            throw new System.NotImplementedException();
        }

        public void OnHitUnit()
        {
            throw new System.NotImplementedException();
        }
    }
}

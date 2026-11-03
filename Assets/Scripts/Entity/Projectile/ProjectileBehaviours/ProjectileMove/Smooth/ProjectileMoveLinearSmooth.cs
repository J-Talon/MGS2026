// Authors: [Jacky, Jeremy, Mark]

using System;
using UnityEngine;

namespace Entity.Projectile.ProjectileBehaviours.ProjectileMove.Smooth
{
    /// <summary>
    /// Implements linear movement behaviour (in a line)
    /// Switches direction based on fired projectile friendly status
    /// </summary>
    [Obsolete]
    public class ProjectileMoveLinearSmooth : ProjectileMoveSmooth
    {
        protected override Vector2 MovementRoutine()
        {
            Vector2 deltaVector = new Vector2(0, 0);
            if (parentProjectile.GetIsFriendly() == true)
            {
                deltaVector.x += parentProjectile.GetSpeed() * Time.deltaTime;
            }
            else
            {
                deltaVector.x -= parentProjectile.GetSpeed() * Time.deltaTime;
            }
            return deltaVector;
        }

    }
}

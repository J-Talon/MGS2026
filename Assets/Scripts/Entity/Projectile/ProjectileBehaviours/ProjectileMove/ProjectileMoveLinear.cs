// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Implements linear movement behaviour (in a line)
/// Switches direction based on fired projectile friendly status
/// </summary>
public class ProjectileMoveLinear : ProjectileMove
{
    protected override Vector2 movementRoutine()
    {
        Vector2 deltaVector = new Vector2(0,0);
        if (parentProjectile.isFriendly == true)
        {
            deltaVector.x += 1;
        }
        else
        {
            deltaVector.x -= 1;
        }
        return deltaVector;
    }

}

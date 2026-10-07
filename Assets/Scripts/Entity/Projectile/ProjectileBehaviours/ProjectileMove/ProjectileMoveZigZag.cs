// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Implements zig zig movement behaviour(1 up, then 1 down, with constant left/right movement)
/// </summary>
public class ProjectileMoveZigZag : ProjectileMove
{
    /// <summary>
    /// should go up or down 1
    /// </summary>
    private bool zigged = false; 

    protected override Vector2 movementRoutine()
    {
        Vector2 deltaVector = new Vector2(0,0);

        deltaVector.x += 1; //constant movement in horizontal direction

        if (zigged)
        {
            deltaVector.y += 1;
        }
        else
        {
            deltaVector.y -= 1;
        }

        zigged = !zigged;

        if (parentProjectile.isFriendly == false) //flips horizontal movement based on projectile friendly status
        {
            deltaVector.x *= -1;
        }
        return deltaVector;
    }

}

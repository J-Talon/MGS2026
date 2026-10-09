// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Implements linear movement behaviour (in a line)
/// Switches direction based on fired projectile friendly status
/// </summary>
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

// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Implements linear movement behaviour (in a line)
/// Switches direction based on fired projectile friendly status
/// TEST BEHAVIOUR FOR CHECKING ARCHITECTURE
/// </summary>
public class ProjectileMoveLinearGrid : ProjectileMoveGrid
{
    protected override Vector2Int movementRoutine()
    {
        Vector2Int deltaVector = new Vector2Int(0, 0);
        if (parentProjectile.GetIsFriendly() == true)
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

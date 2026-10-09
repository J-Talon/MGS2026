// Authors: [Jacky, Jeremy, Mark]
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Implements zig zig movement behaviour(1 up, then 1 down, with constant left/right movement)
/// TEST BEHAVIOUR FOR CHECKING ARCHITECTURE
/// </summary>
public class ProjectileMoveZigZagSmooth : ProjectileMoveSmooth
{
    /// <summary>
    /// should go up or down 1
    /// </summary>
    private bool zigged = false;

    /// <summary>
    /// TEMP VARIABLE max zig amount
    /// </summary>
    private float zigMax = 1f;

    /// <summary>
    /// TEMP VARIABLE, this should be subject to change, track amount of zig so it can be reversed later
    /// </summary>
    private float currentZig = 0f;


    protected override Vector2 movementRoutine()
    {
        Vector2 deltaVector = new Vector2(0, 0);

        deltaVector.x += parentProjectile.GetSpeed() * Time.deltaTime; //constant movement in horizontal direction

        if (zigged)
        {
            deltaVector.y += parentProjectile.GetSpeed() * Time.deltaTime;
        }
        else
        {
            deltaVector.y -= parentProjectile.GetSpeed() * Time.deltaTime;
        }

        currentZig = math.clamp(currentZig + parentProjectile.GetSpeed() * Time.deltaTime, 0, zigMax);
        if (currentZig == zigMax)
        {
            zigged = !zigged;
            currentZig = 0;
        }


        if (parentProjectile.GetIsFriendly() == false) //flips horizontal movement based on projectile friendly status
        {
            deltaVector.x *= -1;
        }
        return deltaVector;
    }

}

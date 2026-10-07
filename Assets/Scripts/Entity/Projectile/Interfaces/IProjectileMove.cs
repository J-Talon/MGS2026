using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public interface IProjectileMove
{
    /// <summary>
    /// On movement tick, projectile moves by this amount after internal tick timers 
    /// </summary>
    /// <param name="deltaMovement"></param>
    void MoveProjectile(Vector2 deltaMovement);

}

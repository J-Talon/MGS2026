using UnityEngine;

/// <summary>
/// interface should be implemented by the projectile class with the projectile move behaviour class referencing the methods in here for better composition
/// </summary>
public interface IProjectileMove
{
    public void MoveProjectile(Vector2 deltaMovement);
}

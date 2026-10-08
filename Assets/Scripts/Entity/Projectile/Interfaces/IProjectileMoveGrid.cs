using UnityEngine;

/// <summary>
/// interface should be implemented by the projectile class with the projectile move behaviour class referencing the methods in here for better composition
/// </summary>
public interface IProjectileMoveGrid
{
    /// <summary>
    /// Grid based movement
    /// </summary>
    /// <param name="deltaMovement"></param>
    public void MoveProjectileGridSnap(Vector2Int deltaMovement);
}

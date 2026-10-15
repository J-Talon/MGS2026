using UnityEngine;

/// <summary>
/// interface should be implemented by the projectile class with the projectile move behaviour class referencing the methods in here for better composition
/// </summary>
public interface IProjectileMoveSmooth
{
    /// <summary>
    /// Continous based movement (i.e PvZ)
    /// </summary>
    /// <param name="deltaMovement"></param>
    public void MoveProjectileSmooth(Vector2 deltaMovement);
}

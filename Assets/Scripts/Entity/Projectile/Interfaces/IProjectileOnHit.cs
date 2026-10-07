using UnityEngine;

/// <summary>
/// interfance should be implemented by the on hit behaviour classes with the projectile class referencing the interface and methods in here for composition
/// </summary>
public interface IProjectileOnHit
{
    public void OnHitMapBorder();

    public void OnHitUnit();
}

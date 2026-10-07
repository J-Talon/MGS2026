using UnityEngine;

public interface IProjectileOnHit
{
    /// <summary>
    /// When collider hits a unit, ideally called only when entering a collider
    /// </summary>
    void OnHitUnit();

    /// <summary>
    /// When projectile coordinates go beyond map borders
    /// </summary>
    void OnHitMapBorder();
}   

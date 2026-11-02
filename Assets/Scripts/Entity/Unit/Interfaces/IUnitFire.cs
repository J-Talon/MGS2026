//Author: Jacky
using UnityEngine;

public interface IUnitFire
{
    /// <summary>
    /// Fire project at this direction given the appropriate entity type
    /// Ideally implementing interface should check if can even fire AND if passedd entity type is even a projectile
    /// </summary>
    public void FireProjectile(Vector2Int fireDirection, EntityType projectileType);

}

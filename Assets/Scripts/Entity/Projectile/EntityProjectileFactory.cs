// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Factory class responsible for spawning projectiles in the game. It provides a method to create projectiles based on their type, position, and other properties.
/// </summary>
public class EntityProjectileFactory
{
    public EntityProjectile SpawnProjectile(ProjectileType projectileType, Vector3 position, bool isFriendly = false, float maxLifeSpan = 10f)
    {
        GameObject prefabToSpawn = projectileType.GetPrefab();
        if (prefabToSpawn == null)
        {
            Debug.LogError($"No prefab found for projectile type: {projectileType.GetAssetPath()}");
            return null;
        }

        EntityProjectile projectile = GameObject.Instantiate(prefabToSpawn, position, Quaternion.identity).GetComponent<EntityProjectile>();
        if (projectile != null)
        {
            projectile.SetIsFriendly(isFriendly);
            projectile.SetMaxLifeSpan(maxLifeSpan);
        }
        else
        {
            Debug.LogError($"Prefab for {projectileType.GetAssetPath()} does not have an EntityProjectile component.");
        }

        return projectile;
    }
}
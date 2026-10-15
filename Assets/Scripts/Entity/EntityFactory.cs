using UnityEngine;

/// <summary>
/// Handles construction of new entities 
/// </summary>
public static class EntityFactory
{
    /// <summary>
    /// Spawns Entity given the appropriate type and setup data
    /// Handles spawning of all entity types (i.e projectile and unit)
    /// </summary>
    /// <param name="type">See EntityType</param>
    /// <param name="position">World space position</param>
    /// <param name="isFriendly">Defaults to false (enemy player), otherwise set to true if part of freindly side</param>
    /// <param name="maxLifeSpan">Unused, Ignore</param>
    /// <returns></returns>
    public static Entity SpawnEntity(EntityType type, Vector3 position, bool isFriendly = false, float maxLifeSpan = 0f)
    {
        GameObject prefabToSpawn = type.GetPrefab();
        if (prefabToSpawn == null)
        {
            Debug.LogError($"No prefab found for entity type: {type.GetAssetPath()}");
            return null;
        }

        Entity entity = GameObject.Instantiate(prefabToSpawn, position, Quaternion.identity).GetComponent<Entity>();
        if (entity != null)
        {
            entity.Setup(isFriendly, type);
            //entity.SetEntityType(type); //IMPORTANT, this is only way to reference the original prefab
            //entity.SetIsFriendly(isFriendly);

            //(jacky) do we need, we can technically just set this in prefab
            //entity.SetMaxLifeSpan(maxLifeSpan);

            // (Jeremy) TODO - pass in JSON data to setup entity variables (e.g. health, speed, etc.) if needed in the future
        }
        else
        {
            Debug.LogError($"Prefab for {type.GetAssetPath()} does not have an Entity component.");
        }

        return entity;
    }
}

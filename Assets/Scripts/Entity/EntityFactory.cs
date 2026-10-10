using UnityEngine;

/// <summary>
/// Part of overall entity manager 
/// handles construction of new entities
/// Should not be directly used
/// </summary>
public class EntityFactory
{
    public Entity SpawnEntity(EntityType type, Vector3 position, bool isFriendly = false, float maxLifeSpan = 0f)
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
            entity.setEntityType(type); //IMPORTANT, this is only way to reference the original prefab
            entity.SetIsFriendly(isFriendly);
            
            //(jacky) do we need, we can technically just set this in prefab
            //entity.SetMaxLifeSpan(maxLifeSpan);
        }
        else
        {
            Debug.LogError($"Prefab for {type.GetAssetPath()} does not have an Entity component.");
        }

        return entity;
    }
}

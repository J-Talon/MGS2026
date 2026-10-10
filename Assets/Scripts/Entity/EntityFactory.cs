using UnityEngine;

public class EntityFactory
{
    public static Entity SpawnEntity(EntityType unitType, Vector3 position, bool isFriendly = false, float maxLifeSpan = 0f)
    {
        GameObject prefabToSpawn = unitType.GetPrefab();
        if (prefabToSpawn == null)
        {
            Debug.LogError($"No prefab found for entity type: {unitType.GetAssetPath()}");
            return null;
        }

        Entity entity = GameObject.Instantiate(prefabToSpawn, position, Quaternion.identity).GetComponent<Entity>();
        if (entity != null)
        {
            entity.SetIsFriendly(isFriendly);
            
            //(jacky) do we need, we can technically just set this in prefab
            //entity.SetMaxLifeSpan(maxLifeSpan);
        }
        else
        {
            Debug.LogError($"Prefab for {unitType.GetAssetPath()} does not have an Entity component.");
        }

        return entity;
    }
}

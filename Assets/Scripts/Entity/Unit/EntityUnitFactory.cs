// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Factory class responsible for spawning units in the game. It provides a method to create units based on their type, position, and other properties.
/// </summary>
public class EntityUnitFactory
{
    public EntityUnit SpawnUnit(UnitType unitType, Vector3 position, bool isFriendly = false, float maxLifeSpan = 0f)
    {
        GameObject prefabToSpawn = unitType.GetPrefab();
        if (prefabToSpawn == null)
        {
            Debug.LogError($"No prefab found for unit type: {unitType.GetAssetPath()}");
            return null;
        }

        EntityUnit unit = GameObject.Instantiate(prefabToSpawn, position, Quaternion.identity).GetComponent<EntityUnit>();
        if (unit != null)
        {
            unit.SetIsFriendly(isFriendly);
            unit.SetMaxLifeSpan(maxLifeSpan);
        }
        else
        {
            Debug.LogError($"Prefab for {unitType.GetAssetPath()} does not have an EntityUnit component.");
        }

        return unit;
    }
}
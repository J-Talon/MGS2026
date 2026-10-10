using System.IO;
using UnityEngine;

public class EntityType
{
    private readonly string _assetPath;
    private readonly GameObject _prefab;

    public EntityType(string assetPath)
    {
        _assetPath = assetPath;
        _prefab = Resources.Load<GameObject>(assetPath);

        if (_prefab == null)
            throw new FileNotFoundException("Could not find the unit prefab:" + assetPath);

        if (_prefab.GetComponent<Entity>() == null)
            throw new MissingComponentException("The entity asset " + assetPath + " must have a EntityUnit component attached.");
    }

    public string GetAssetPath() { return _assetPath; }
    public GameObject GetPrefab() { return _prefab; }

    // Example of a predefined unit type. You can add more as needed.
    public static readonly EntityType UnitTest = new EntityType("Entity/Unit/Prefabs/UnitTest");
    
    // Example of a predefined projectile type. You can add more as needed.
    public static readonly EntityType ProjectileTestLinearGrid = new EntityType("Entity/Projectile/Prefabs/ProjectileTestLinearGrid");
    public static readonly EntityType ProjectileTestLinearSmooth = new EntityType("Entity/Projectile/Prefabs/ProjectileTestLinearSmooth");
    public static readonly EntityType ProjectileTestZiggedGrid = new EntityType("Entity/Projectile/Prefabs/ProjectileTestZiggedGrid");
    public static readonly EntityType ProjectileTestZiggedSmooth = new EntityType("Entity/Projectile/Prefabs/ProjectileTestZiggedSmooth");
}

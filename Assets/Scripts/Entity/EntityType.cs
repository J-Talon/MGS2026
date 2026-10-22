using System.IO;
using UnityEngine;

public enum EntityClassification
{
    projectile, unit
}

public class EntityType
{
    /// <summary>
    /// (jacky) mainly used for distingushing units (i.e if we only pool "projectiles", this can help figure out if they actually are)
    /// </summary>
    private readonly EntityClassification _entityClassification;
    private readonly string _assetPath;
    private readonly GameObject _prefab;

    public EntityType(string assetPath, EntityClassification classification)
    {
        _entityClassification = classification;
        _assetPath = assetPath;
        _prefab = Resources.Load<GameObject>(assetPath);

        if (_prefab == null)
            throw new FileNotFoundException("Could not find the unit prefab:" + assetPath);

        if (_prefab.GetComponent<Entity>() == null)
            throw new MissingComponentException("The entity asset " + assetPath + " must have a EntityUnit component attached.");
    }

    public EntityClassification GetEntityClassification(){ return _entityClassification; }
    public string GetAssetPath() { return _assetPath; }
    public GameObject GetPrefab() { return _prefab; }

    // Example of a predefined unit type. You can add more as needed.
    public static readonly EntityType UnitTest = new EntityType("Entity/Unit/Prefabs/UnitTest", EntityClassification.unit);
    public static readonly EntityType UnitTestDemon = new EntityType("Entity/Unit/Prefabs/UnitTestDemon", EntityClassification.unit);

    // Example of a predefined projectile type. You can add more as needed.
    public static readonly EntityType ProjectileTestLinearGrid = new EntityType("Entity/Projectile/Prefabs/ProjectileTestLinearGrid", EntityClassification.projectile);
    public static readonly EntityType ProjectileTestLinearSmooth = new EntityType("Entity/Projectile/Prefabs/ProjectileTestLinearSmooth", EntityClassification.projectile);
    public static readonly EntityType ProjectileTestZiggedGrid = new EntityType("Entity/Projectile/Prefabs/ProjectileTestZiggedGrid", EntityClassification.projectile);
    public static readonly EntityType ProjectileTestZiggedSmooth = new EntityType("Entity/Projectile/Prefabs/ProjectileTestZiggedSmooth", EntityClassification.projectile);

    public static readonly EntityType ProjectilePea = new EntityType("Entity/Spells/Pea", EntityClassification.projectile);
    public static readonly EntityType ProjectileBomb = new EntityType("Entity/Spells/Bomb", EntityClassification.projectile);
}

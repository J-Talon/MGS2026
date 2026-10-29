using System.IO;
using UnityEngine;

public enum EntityClassification
{
    Projectile,
    Unit,
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
            throw new FileNotFoundException("[EntityType] Could not find the unit prefab:" + assetPath);

        switch (classification)
        {
            case EntityClassification.Unit:
                if (_prefab.GetComponent<EntityUnit>() == null) 
                    throw new MissingComponentException("[EntityType] Entities of type Unit must have a class extending from type EntityUnit:" + assetPath);
                break;
            
            case EntityClassification.Projectile:
                if (_prefab.GetComponent<EntityProjectile>() == null) 
                    throw new MissingComponentException("[EntityType] Entities of type Projectile must have a class extending from type EntityProjectile:" + assetPath);
                break;
            
            default:
                throw new MissingComponentException(
                    "[EntityType] Classification not defined! Register it in the enum!");
        }
    }

    public EntityClassification GetEntityClassification() { return _entityClassification; }
    public string GetAssetPath() { return _assetPath; }
    public GameObject GetPrefab() { return _prefab; }

    // Example of a predefined unit type. You can add more as needed.
    public static readonly EntityType UnitTest = new EntityType("Entity/Unit/Prefabs/UnitTest", EntityClassification.Unit);
    public static readonly EntityType UnitTestDemon = new EntityType("Entity/Unit/Prefabs/UnitTestDemon", EntityClassification.Unit);
    public static readonly EntityType UnitTestTurret = new EntityType("Entity/Unit/Prefabs/TurretTest", EntityClassification.Unit);

    // Example of a predefined projectile type. You can add more as needed.
    public static readonly EntityType ProjectileTestLinearGrid = new EntityType("Entity/Projectile/Prefabs/ProjectileTestLinearGrid", EntityClassification.Projectile);
    public static readonly EntityType ProjectileTestLinearSmooth = new EntityType("Entity/Projectile/Prefabs/ProjectileTestLinearSmooth", EntityClassification.Projectile);
    public static readonly EntityType ProjectileTestZiggedGrid = new EntityType("Entity/Projectile/Prefabs/ProjectileTestZiggedGrid", EntityClassification.Projectile);
    public static readonly EntityType ProjectileTestZiggedSmooth = new EntityType("Entity/Projectile/Prefabs/ProjectileTestZiggedSmooth", EntityClassification.Projectile);

    public static readonly EntityType ProjectilePea = new EntityType("Entity/Spells/Pea", EntityClassification.Projectile);
    public static readonly EntityType ProjectileBomb = new EntityType("Entity/Spells/Bomb", EntityClassification.Projectile);
    public static readonly EntityType ProjectileBoomerang = new EntityType("Entity/Spells/Boomerang", EntityClassification.Projectile);
}

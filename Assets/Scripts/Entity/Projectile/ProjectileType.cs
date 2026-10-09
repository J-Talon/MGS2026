// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using System.IO;


/// <summary>
/// Represents a type of projectile in the game. Each projectile type is associated with a prefab that defines its appearance and behavior. This class provides methods to retrieve the prefab and asset path for the projectile type.
/// See ParticleType by Talon for similar implementation.
/// </summary>
public class ProjectileType
{
    private readonly string _assetPath;
    private readonly GameObject _prefab;

    public ProjectileType(string assetPath)
    {
        _assetPath = assetPath;
        _prefab = Resources.Load<GameObject>(assetPath);

        if (_prefab == null)
            throw new FileNotFoundException("Could not find the projectile prefab:" + assetPath);

        if (_prefab.GetComponent<EntityProjectile>() == null)
            throw new MissingComponentException("The projectile asset " + assetPath + " must have a EntityProjectile component attached.");
    }

    public string GetAssetPath() { return _assetPath; }
    public GameObject GetPrefab() { return _prefab; }

    // Example of a predefined projectile type. You can add more as needed.
    public static readonly ProjectileType ProjectileTestLinearGrid = new ProjectileType("Entity/Projectile/ProjectileTestLinearGrid");
    public static readonly ProjectileType ProjectileTestLinearSmooth = new ProjectileType("Entity/Projectile/ProjectileTestLinearSmooth");
    public static readonly ProjectileType ProjectileTestZiggedGrid = new ProjectileType("Entity/Projectile/ProjectileTestZiggedGrid");
    public static readonly ProjectileType ProjectileTestZiggedSmooth = new ProjectileType("Entity/Projectile/ProjectileTestZiggedSmooth");
}
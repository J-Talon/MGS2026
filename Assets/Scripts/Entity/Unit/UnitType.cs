// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using System.IO;

/// <summary>
/// Represents a type of unit in the game. Each unit type is associated with a prefab that defines its appearance and behavior. This class provides methods to retrieve the prefab and asset path for the unit type.
/// See ParticleType by Talon for similar implementation.
/// </summary>
public class UnitType
{
    private readonly string _assetPath;
    private readonly GameObject _prefab;

    public UnitType(string assetPath)
    {
        _assetPath = assetPath;
        _prefab = Resources.Load<GameObject>(assetPath);

        if (_prefab == null)
            throw new FileNotFoundException("Could not find the unit prefab:" + assetPath);

        if (_prefab.GetComponent<EntityUnit>() == null)
            throw new MissingComponentException("The unit asset " + assetPath + " must have a EntityUnit component attached.");
    }

    public string GetAssetPath() { return _assetPath; }
    public GameObject GetPrefab() { return _prefab; }

    // Example of a predefined unit type. You can add more as needed.
    public static readonly UnitType UnitTest = new UnitType("Entity/Unit/Prefabs/UnitTest");
}
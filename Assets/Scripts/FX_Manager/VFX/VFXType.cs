using System.IO;
using UnityEngine;

/*
Effect types.
Different from particle effects.
Literally anything visual that particles cannot do (puddles, highlights, etc.)
The VFX can include particles in them!

@author Wilson (1oopy)
*/
public class VFXType
{
    private readonly string _assetPath;
    private readonly GameObject _prefab;

    public VFXType(string assetPath)
    {
        _assetPath = assetPath;
        _prefab = Resources.Load<GameObject>(assetPath);
        
        // Yells at you
        if (_prefab == null) 
            throw new FileNotFoundException("Could not find the VFX asset:"+assetPath);
            
        if (_prefab.GetComponent<VisualEffectBase>() == null)
            throw new MissingComponentException("The particle asset "+assetPath+" must have a visual effect component");
    }

    public string GetAssetPath() {return _assetPath;}
    public GameObject GetPrefab() { return _prefab; }

    // TYPES add more if needed
    public static readonly VFXType GridPersistent = new VFXType("VFX/Prefabs/GridPersistent");
}

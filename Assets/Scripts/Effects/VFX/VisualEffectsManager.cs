using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/*
Manages visual effects.
Different from particle effects.
Literally anything visual that particles cannot do (puddles, highlights, etc.)
The VFX can include particles in them!

This is basically completely copied from EntityManager.cs (Oct 25 2026)
(I'm sorry, I'm not writing another manager if there is one that exists and works. Please forgive me.)
Some slight changes:
    - No factory script, it spawns the object here (Probably gonna change in the future oh well)
    - Added UniqueID assignment to the manager

@author Wilson (1oopy)
*/
public class VisualEffectsManager
{
    /// Track effects that are no longer actively used
    /// Key of vfx type to check specific lists for those specific vfx
    private readonly Dictionary<VFXType, Queue<VisualEffectBase>> reservePool;
    private readonly Dictionary<int, VisualEffectBase> activeEffects;
    
    public VisualEffectsManager()
    {
        reservePool = new Dictionary<VFXType, Queue<VisualEffectBase>>();
        activeEffects = new Dictionary<int, VisualEffectBase>();
    }

    /// Singleton instance
    private static VisualEffectsManager instance = null;

    /// Returns the singleton instance.
    public static VisualEffectsManager GetInstance()
    {
        if (instance == null)
            instance = new VisualEffectsManager();

        return instance;
    }

    // Unique ID assignment
    private static int _nextVFXID = 0;

    public static int GetUniqueID()
    {
        return Interlocked.Increment(ref _nextVFXID);
    }

    #region stats

    public int GetTotalPooledVFXCount()
    {
        int totalPooled = 0;
        foreach (var vfxType in reservePool)
        {
            totalPooled += vfxType.Value.Count;
        }
        return totalPooled;
    }

    public int GetActiveVFXCount()
    {
        return activeEffects.Count;
    }

    public int GetAllVFXCount()
    {
        return GetTotalPooledVFXCount() + GetActiveVFXCount();
    }

    #endregion

    #region Pooling Handling

    /*
    get an effect from pool
    (Stolen from EntityManager)

    If effect with matching type exists in pool, reset it and return it.
    Else create a new one and return it
    */
    public VisualEffectBase GetVisualEffect(VFXType type, Vector3 position)
    {
        VisualEffectBase vfx = null;
        if (reservePool.ContainsKey(type) != true) //pool list for this specific effect doesn't even exist
        {
            reservePool.Add(type, new Queue<VisualEffectBase>());
        }

        if (reservePool[type].Count == 0) //no effects were pooled, must make a new one
        {
            GameObject prefabToSpawn = type.GetPrefab();
            if (prefabToSpawn == null)
            {
                Debug.LogError($"No prefab found for entity type: {type.GetAssetPath()}");
                return null;
            }

            vfx = Object.Instantiate(prefabToSpawn, position, Quaternion.identity).GetComponent<VisualEffectBase>();
            Object.DontDestroyOnLoad(vfx); //ensure vfx that is part of pool system cannot be destroyed via scene change

            vfx.Setup(type, (Vector2)position);
        }
        else
        { //can remove from pool, set up and return it
            vfx = reservePool[type].Dequeue();
            
            vfx.Setup(type, (Vector2)position);
        }

        //Debug.Log("adding " + entity.GetEntityID());
        activeEffects.Add(vfx.GetVFXID(), vfx); //add to active dictionary for tracking
        return vfx;
    }

    /*
    return an effect to the pool
    (Stolen from EntityManager)
    */
    public void ReturnVisualEffect(VisualEffectBase vfx)
    {
        activeEffects.Remove(vfx.GetVFXID()); //remove from active dictionary

        //disable and ensure it won't be removed on scene change, etc...
        vfx.gameObject.SetActive(false);

        if (reservePool.ContainsKey(vfx.GetVFXType()) != true) //will have to instantiate the new dictionary element since it didn't exist in pool before 
        {
            reservePool.Add(vfx.GetVFXType(), new Queue<VisualEffectBase>());
        }

        reservePool[vfx.GetVFXType()].Enqueue(vfx); //add to pool
    }

    /// To be called manually to unregister an effect (in the event it should be destroyed rather than be pooled)
    public void Unregister(VisualEffectBase vfx)
    {
        activeEffects.Remove(vfx.GetVFXID());
    }

    #endregion Pooling
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Pool system is adapted from Unity's built in framework for pooling (uses stack to use older ones first (not sure reasoning))
/// We note several things from pool system:
/// - don't destroy on load is used when placed into reserve pool (reason being if scene changed then pooled objects will persist)
/// - only a reserve pool is tracked, not active (reason being that i can't think of a reason to track active entities for now) (maybe just have a "active list" of entities instead)
///
/// IMPORTANT THING TO DO OPTIONALLY: using entity classification enum in EntityType, disallow pooling for certain entities
/// (i.e as talon said, maybe not the best idea to pool units)
/// 
/// Also to note, maybe replace queue for pool storage? if we need a specific object in the queue, we literally can't get it
///  </summary>
public class EntityManager
{
    /// <summary>
    /// Track entities that are no longer actively used
    /// Key of entity type to check specific lists for those specific entities
    /// </summary>
    private readonly Dictionary<EntityType, Queue<Entity>> reservePool;

    private readonly Dictionary<int, Entity> activeEntities;

    private EntityManager()
    {
        reservePool = new Dictionary<EntityType, Queue<Entity>>();
        activeEntities = new Dictionary<int, Entity>();
    }

    /// <summary>
    /// Singleton instance
    /// </summary>
    private static EntityManager instance = null;

    /// <summary>
    /// Returns the singleton instance.
    /// </summary>
    /// <returns></returns>
    public static EntityManager GetInstance()
    {
        if (instance == null)
            instance = new EntityManager();

        return instance;
    }

    #region stats

    public int GetTotalPooledEntitiesCount()
    {
        int totalPooled = 0;
        foreach (var entityType in reservePool) //lowkey using foreach cause idk how else to iterate through dict in a clean way
        {
            totalPooled += entityType.Value.Count;
        }
        return totalPooled;
    }

    public int GetActiveEntitiesCount()
    {
        return activeEntities.Count;
    }

    public int GetAllEntitiesCount()
    {
        return GetTotalPooledEntitiesCount() + GetActiveEntitiesCount();
    }

    #endregion

    #region Pooling Handling

    /// <summary>
    /// get from pool
    /// 
    /// If entity with matching type exists in pool, reset it to new state, and return it
    /// Else, simply create a new entity and return it
    /// </summary>
    /// <param name="unitType"></param>
    /// <param name="position"></param>
    /// <param name="isFriendly"></param>
    /// <param name="maxLifeSpan"></param>
    /// <returns></returns>
    public Entity GetEntity(EntityType type, Vector3 position, bool isFriendly = false, float maxLifeSpan = 0f)
    {
        Entity entity = null;
        if (reservePool.ContainsKey(type) != true) //pool list for this specific entity doesn't even exist
        {
            reservePool.Add(type, new Queue<Entity>());
        }

        if (reservePool[type].Count == 0) //no entities was pooled, must make a new one
        {
            entity = EntityFactory.SpawnEntity(type, position, isFriendly, maxLifeSpan);
            Object.DontDestroyOnLoad(entity); //ensure entity that is part of pool system cannot be destroyed via scene change
        }
        else
        { //can remove from pool, reset to original state and return it
            entity = reservePool[type].Dequeue();
            entity.ResetToOriginal();
            entity.gameObject.SetActive(true);

            //resets entity in new position
            entity.transform.position = position;
        }

        //Debug.Log("adding");
        activeEntities.Add(entity.GetEntityID(), entity); //add to active dictionary for tracking
        return entity;
    }

    /// <summary>
    /// Return entity to pool based off of its entity type
    /// </summary>
    /// <param name="entity"></param>
    public void ReturnEntity(Entity entity)
    {
        activeEntities.Remove(entity.GetEntityID()); //remove from active dictionary

        //disable and ensure it won't be removed on scene change, etc...
        entity.gameObject.SetActive(false);

        if (reservePool.ContainsKey(entity.GetEntityType()) != true) //will have to instantiate the new dictionary element since it didn't exist in pool before 
        {
            reservePool.Add(entity.GetEntityType(), new Queue<Entity>());
        }

        reservePool[entity.GetEntityType()].Enqueue(entity); //add to pool
    }

    /// <summary>
    /// To be called manually to unregister a entity (in the event it should be destroyed rather than be pooled)
    /// </summary>
    public void Unregister(Entity entity)
    {
        activeEntities.Remove(entity.GetEntityID());
    }

    #endregion Pooling
}

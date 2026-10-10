using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Pool;
using UnityEngine.UIElements;


/// <summary>
/// Pool system is adapted from Unity's built in framework for pooling (uses stack to use older ones first (not sure reasoning))
/// We note several things from pool system:
/// - don't destroy on load is used when placed into reserve pool (reason being if scene changed then pooled objects will persist)
/// - only a reserve pool is tracked, not active (reason being that i can't think of a reason to track active entities for now) (maybe just have a "active list" of entities instead)
/// </summary>
public class EntityManager
{
    EntityFactory entityFactory;

    /// <summary>
    /// Track entities that are no longer actively used
    /// Key of entity type to check specific lists for those specific entities
    /// </summary>
    private readonly Dictionary<EntityType, Queue<Entity>> reservePool;

    private readonly List<Entity> activeEntities;

    private EntityManager()
    {
        entityFactory = new EntityFactory();
        reservePool = new Dictionary<EntityType, Queue<Entity>>();
        activeEntities = new List<Entity>();
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
        foreach(var entityType in reservePool) //lowkey using foreach cause idk how else to iterate through dict in a clean way
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
            entity = entityFactory.SpawnEntity(type, position, isFriendly, maxLifeSpan);
        }
        else
        { //can remove from pool, reset to original state and return it
            entity = reservePool[type].Dequeue();
            entity.gameObject.SetActive(true);
            //TODDO - need to also remove the "dont destroy on load" if possible after retrieving from pool
            //TODO- also add the "reset"/"setup" functions to deal with resetting a pooled entity back to original state   
            entity.transform.position = position;
        }

        Debug.Log("adding");
        activeEntities.Add(entity); //add to active list
        return entity;
    }

    /// <summary>
    /// Return entity to pool based off of its entity type
    /// </summary>
    /// <param name="entity"></param>
    public void ReturnEntity(Entity entity)
    {
        activeEntities.Remove(entity); //remove from active list

        //disable and ensure it won't be removed on scene change, etc...
        entity.gameObject.SetActive(false);
        Object.DontDestroyOnLoad(entity);

        if (reservePool.ContainsKey(entity.getEntityType()) != true) //will have to instantiate the new dictionary element since it didn't exist in pool before 
        {
            reservePool.Add(entity.getEntityType(), new Queue<Entity>());
        }

        reservePool[entity.getEntityType()].Enqueue(entity); //add to pool
    }

    #endregion Pooling
}

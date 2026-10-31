// Authors: [Jacky, Jeremy, Mark]



using UnityEngine;

/// <summary>
/// Base class for all entities in the game, including players, enemies and projectiles. Contains common stats and properties that all entities share.
/// </summary>

public abstract class Entity : MonoBehaviour
{
    [Header("Identifiers")]
    [field: SerializeField] protected bool isInvulnerable = false;
    [field: SerializeField] protected bool isFriendly = false;
    [field: SerializeField] protected bool isAlive = true;
    // SG:
    // properties for figuringing out the player and whether its holding the boomerang respectively
    [field: SerializeField] protected bool isPlayer = false;
    [field: SerializeField] protected bool isHoldingBoomerang = true;

    static protected int globalStaticIDCounter = 0;
    protected int entityID; // Unique identifier for the specific instance of the entity, useful for tracking and managing entities in the game world.

    [Header("Stats")]
    [field: SerializeField] protected float maxHealth = 100;
    [field: SerializeField] protected float damage = 10f;
    [field: SerializeField] protected float speed = 1f;
    [field: SerializeField] protected float maxLifeSpan = 0f; // 0 means infinite lifespan
    [field: SerializeField] protected float movementTickTimer = 1f;

    [Header("Runtime Stats")]
    [field: SerializeField] protected float currentHealth; // [field: SerializeField] just to see HP in editor
    public float spawnTime;   //the timestamp when the entity was "spawned"

    #region instance variables
    // Player position variables
    protected EntityType entityType;
    #endregion

    // SG: are these necessary for properties with a setter?
    // Setters and Getters for encapsulation
    public bool GetIsHoldingBoomerang() { return isHoldingBoomerang; }
    public void SetIsHoldingBoomerang(bool value) { isHoldingBoomerang = value; }
    public void SetIsFriendly(bool value) { isFriendly = value; }
    public void SetEntityType(EntityType type) { entityType = type; }
    public EntityType GetEntityType() { return entityType; }
    public bool GetIsFriendly() { return isFriendly; }
    public void SetMaxLifeSpan(float value) { maxLifeSpan = value; }
    public bool GetIsPlayer() { return isPlayer; }
    public void SetIsPlayer(bool value) { isPlayer = value; }
    public void SetSpeed(float value) { speed = value; }
    public float GetSpeed() { return speed; }
    public float GetMovementTickTimer() { return movementTickTimer; }
    public Vector3 GetPosition() { return transform.position; }
    public int GetEntityID() { return entityID; }

    static int AssignUniqueID()
    {
        globalStaticIDCounter++;
        //Debug.Log(globalStaticIDCounter);
        return globalStaticIDCounter;
    }

    /// <summary>
    /// Common start up for entity
    /// </summary>
    protected void Start()
    {

    }

    /// <summary>
    /// Initializes the entity's health and sets up any necessary components. This method is called when the entity is first created in the game.
    /// </summary>
    protected virtual void Awake()
    {
        entityID = AssignUniqueID();
        // Debug.Log(entityID);
        currentHealth = maxHealth;
        spawnTime = Time.time;
    }

    /// <summary>
    /// initial setup on first creation
    /// </summary>
    public void Setup(bool friendly, EntityType type)
    {
        //Debug.Log("first time setup");
        this.entityType = type;
        this.isFriendly = friendly;
    }

    /// <summary>
    /// Resets this entity to base state by copying values from original Prefab.
    /// Should be overriden if specific reset behaviour beyond generic entity reset is needed
    /// </summary>
    public virtual void ResetToOriginal()
    {
        //Debug.Log("Resetting entity to original state");
        Entity original = entityType.GetPrefab().GetComponent<Entity>(); //NOTE: right now we don't actually need this

        //reset the runtime stats
        isAlive = true;
        currentHealth = original.maxHealth;
        spawnTime = Time.time;
        isHoldingBoomerang = false;
        isPlayer = false;
    }

    /// <summary>
    /// Causes the entity to take damage. This method is called when the entity is hit by an attack.
    /// Talon J: Return: True if the entity took damage, return false if it did not
    /// 
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public virtual bool TakeDamage(float damage)
    {
        if (!isAlive || isInvulnerable || damage <= 0f) return false;
        currentHealth -= damage;
        if (currentHealth <= 0f) Die();
        return true;
    }

    /// <summary>
    /// Modified: Talon J
    /// Heals the entity, return true if was successfully healed
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public virtual bool Heal(float amount)
    {
        if (!isAlive || amount <= 0f) return false;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        return true;
    }

    /// <summary>
    /// Handles death logic and triggers events.
    /// </summary>
    public virtual void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        // if the entity is a unit, unregister it from the grid cell it occupies
        if (entityType.GetEntityClassification() == EntityClassification.Unit)
        {
            IGridSystemView gridManager = GridManager.instance;
            GridCell gridPosition = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(gameObject.transform.position));
            gridPosition.EntityUnitLeave(); //unregister from grid cell
        }
        Despawn();
    }

    /// <summary>
    /// Destroys game object (no pooling)
    /// </summary>
    public virtual void DespawnFull()
    {
        EntityManager.GetInstance().Unregister(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Pools the gameobject, if pooling shouldn't be implemented
    /// </summary>
    public virtual void Despawn()
    {
        EntityManager.GetInstance().ReturnEntity(this);
    }

    /// <summary>
    /// Check if this entity can target or damage another entity.
    /// By default, entities are hostile to each other if they have different IsFriendly values.
    /// </summary>
    public virtual bool IsHostile(Entity other)
    {
        return other != null && isFriendly != other.isFriendly;
    }

    public abstract void Tick();

    
    //@Talon J
    //update is valid here. No need to repeat it in subclasses
    public void Update()
    {
        this.Tick();
    }
}

// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using System;

/// <summary>
/// Base class for all entities in the game, including players, enemies and projectiles. Contains common stats and properties that all entities share.
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [Header("Identifiers")]
    [field: SerializeField] protected bool isInvulnerable = false;
    [field: SerializeField] protected bool isFriendly = false;
    [field: SerializeField] protected bool isAlive = true;

    [Header("Stats")]
    [field: SerializeField] protected float maxHealth = 100;
    [field: SerializeField] protected float damage = 10f;
    [field: SerializeField] protected float speed = 1f;
    [field: SerializeField] protected float maxLifeSpan = 0f; // 0 means infinite lifespan
    [field: SerializeField] protected float movementTickTimer = 1f;

    [Header("Runtime Stats")]
    protected float currentHealth;
    public float spawnTime;

    #region instance variables
    // Player position variables
    protected Vector3 position;
    
    protected EntityType entityType;
    #endregion

    // Setters and Getters for encapsulation
    public void SetIsFriendly(bool value) { isFriendly = value; }
    public void SetEntityType(EntityType type) { entityType = type; }
    public EntityType GetEntityType() { return entityType; }
    public bool GetIsFriendly() { return isFriendly; }
    public void SetMaxLifeSpan(float value) { maxLifeSpan = value; }
    public void SetSpeed(float value) { speed = value; }
    public float GetSpeed() { return speed; }
    public float GetMovementTickTimer() { return movementTickTimer; }
    public Vector3 GetPosition() { return transform.position; }

    // Events for UI, audio, or visual effects listeners
    public event Action<float, float> OnHealthChanged; // (currentHealth, maxHealth)
    public event Action OnDeath;

    /// <summary>
    /// Initializes the entity's health and sets up any necessary components. This method is called when the entity is first created in the game.
    /// </summary>
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        spawnTime = Time.time;
        position = gameObject.transform.position;
    }

    /// <summary>
    /// initial setup on first creation
    /// </summary>
    public void Setup(bool isFriendly, EntityType entityType)
    {
        //Debug.Log("first time setup");
        this.entityType = entityType;
        this.isFriendly = isFriendly;
    }

    /// <summary>
    /// Resets this entity to base state by copying values from original Prefab. Should be overriden if specific reset behaviour beyond generic entity reset is needed
    /// </summary>
    public virtual void Reset()
    {
        //Debug.Log("Resetting entity to original state");
        Entity original = entityType.GetPrefab().GetComponent<Entity>(); //NOTE: right now we don't actually need this

        //reset the runtime stats
        isAlive = true;
        currentHealth = original.maxHealth;
        spawnTime = Time.time;
    }

    /// <summary>
    /// Causes the entity to take damage. This method is called when the entity is hit by an attack.
    /// </summary>
    /// <param name="amount">The amount of damage to take.</param>
    public virtual void TakeDamage(float damage)
    {
        if (!isAlive || isInvulnerable || damage <= 0f) return;
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0f) Die();
    }

    public virtual void Heal(float amount)
    {
        if (!isAlive || amount <= 0f) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Handles death logic and triggers events.
    /// </summary>
    public virtual void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        OnDeath?.Invoke();
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

    public virtual void Tick()
    {
        if (maxLifeSpan > 0f && Time.time - spawnTime >= maxLifeSpan)
        {
            Die();
        }
    }
}
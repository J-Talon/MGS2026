// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using System;

/// <summary>
/// Base class for all entities in the game, including players, enemies and projectiles. Contains common stats and properties that all entities share.
/// </summary>
public abstract class Entity : MonoBehaviour
{

    [Header("Identifiers")]
    [field: SerializeField] public bool isInvulnerable {protected set; get;}
    [field: SerializeField] public bool isFriendly {protected set; get;} = false;
    [field: SerializeField] public bool isAlive {protected set; get;} = true;

    [Header("Stats")]
    [field: SerializeField] public float maxHealth {protected set; get;} = 100;
    [field: SerializeField] public float currentHealth {protected set; get;}
    [field: SerializeField] public float damage {protected set; get;}
    [field: SerializeField] public float speed {protected set; get;} = 1f;
    [field: SerializeField] public float maxLifeSpan {protected set; get;} = 0f; // 0 means infinite lifespan
    [field: SerializeField] public float movementTickTimer {protected set; get;} = 1f;

    //[field: SerializeField] public Texture2D sprite {protected set; get;}

    

    #region instance variables
    // Player position variables
    protected Vector2 position;
    
    #endregion

    // Events for UI, audio, or visual effects listeners
    public event Action<float, float> OnHealthChanged; // (currentHealth, maxHealth)
    public event Action OnDeath;

    /// <summary>
    /// Initializes the entity's health and sets up any necessary components. This method is called when the entity is first created in the game.
    /// </summary>
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        position = gameObject.transform.position;
    }

    /// <summary>
    /// Called when the entity is enabled in the scene. Resets health and sets up lifespan if applicable.
    /// </summary>
    protected virtual void OnEnable()
    {
        isAlive = true;
        currentHealth = maxHealth;
        if (maxLifeSpan > 0f) Invoke(nameof(Despawn), maxLifeSpan);
    }

    /// <summary>
    /// Called when the entity is disabled in the scene. Cancels any pending lifespan despawn calls.
    /// </summary>
    protected virtual void OnDisable()
    {
        CancelInvoke(nameof(Despawn));
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
    /// Destroys or recycles (pools) the GameObject. Override if using object pooling.
    /// </summary>
    public virtual void Despawn()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Check if this entity can target or damage another entity.
    /// By default, entities are hostile to each other if they have different IsFriendly values.
    /// </summary>
    public virtual bool IsHostile(Entity other)
    {
        return other != null && isFriendly != other.isFriendly;
    }
}
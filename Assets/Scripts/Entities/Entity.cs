// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using System;

/// <summary>
/// Base class for all entities in the game, including players, enemies and projectiles. Contains common stats and properties that all entities share.
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] protected bool isInvulnerable;
    [SerializeField] protected bool isFriendly;
    [SerializeField] protected bool isAlive = true;

    [Header("Stats")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float maxLifeSpan = 0f; // 0 means infinite lifespan

    // Public properties to access the entity's stats and properties. Example: if (entity.IsFriendly) { ... }
    public bool IsFriendly => isFriendly;
    public bool IsInvulnerable => isInvulnerable;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float Damage => damage;
    public bool IsAlive => isAlive;
    public float MaxLifeSpan => maxLifeSpan;

    // Events for UI, audio, or visual effects listeners
    public event Action<float, float> OnHealthChanged; // (currentHealth, maxHealth)
    public event Action OnDeath;

    /// <summary>
    /// Initializes the entity's health and sets up any necessary components. This method is called when the entity is first created in the game.
    /// </summary>
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
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
        if (!IsAlive || isInvulnerable || damage <= 0f) return;
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0f) Die();
    }

    public virtual void Heal(float amount)
    {
        if (!IsAlive || amount <= 0f) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Handles death logic and triggers events.
    /// </summary>
    public virtual void Die()
    {
        if (!IsAlive) return;
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
        return other != null && IsFriendly != other.IsFriendly;
    }
}
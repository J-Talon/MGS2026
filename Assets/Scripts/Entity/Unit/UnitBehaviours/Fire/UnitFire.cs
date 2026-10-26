//author jacky

using System;
using System.Numerics;
using UnityEngine;

/// <summary>
/// Fire a projectile based on a timer
/// </summary>
public abstract class UnitFire : MonoBehaviour
{
    [field: SerializeField] protected EntityType projectileType;

    [field: SerializeField] protected int fireCooldown;

    /// <summary>
    /// reference for controlling the projectile
    /// </summary>
    protected EntityUnit parentUnit;

    #region projectile instance data

    /// <summary>
    /// Cooldown time before firing
    /// </summary>
    private float timeLeft; //trigger firing immedietly?

    #endregion

    /// <summary>
    /// Setup internal cooldown tracker and interface getting
    /// </summary>
    void Awake()
    {
        parentUnit = GetComponent<EntityUnit>();
        timeLeft = fireCooldown;
    }

    /// <summary>
    /// Main loop, decrements timer when still on firing cooldown
    /// Else, resets timer then calls the actual firing behaviour defined in a actual concrete class
    /// </summary>
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = fireCooldown;

            Tuple<Vector2Int, EntityType, bool> firedData = FireRoutine();
            if (firedData.Item3 == true)
            {
                parentUnit.FireProjectile(firedData.Item1, firedData.Item2);
            }
            
        }
    }

    /// <summary>
    /// The actual firing behaviour to be implemented in a concrete class implementation.
    /// 
    /// Return tuple stating the spawn position, projectile type, and if spawning is even allowed for this fire cycle
    /// </summary>
    protected abstract Tuple<Vector2Int, EntityType, bool> FireRoutine();
}

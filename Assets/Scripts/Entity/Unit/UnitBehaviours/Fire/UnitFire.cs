//author jacky

using System;
using System.Numerics;
using UnityEngine;

/// <summary>
/// Control firing behaviour of a unit based on various factors. Currently allows for firing on cooldown + holding fire (while checking every frame if can fire again)
/// 
/// Actual firing not handled here, instead data is passed to the attached entity unit for actual projectile creation
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

    /// <summary>
    /// If unit chose to not fire during a cycle, instead run a firing check everyy frame now
    /// </summary>
    private bool heldFire = false;

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
    /// 
    /// IF unit chose not to fire previous cycle, also trigger firing routine check every frame until it does fire
    /// </summary>
    void Update()
    {
        if (timeLeft > 0 && heldFire == false)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = fireCooldown;

            Tuple<Vector2Int, EntityType, bool> firedData = FireRoutine();

            if (firedData.Item3 == true)
            {
                heldFire = false;
                parentUnit.FireProjectile(firedData.Item1, firedData.Item2);
            }
            else
            {
                heldFire = true;
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

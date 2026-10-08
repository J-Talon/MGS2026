// Authors: [Jacky, Jeremy, Mark]
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// abstract class for projectile movement behaviour. Used abstract due to common stuff for all projectile movements (i.e timer tracking between movements)
/// </summary>
public abstract class ProjectileMoveGrid : MonoBehaviour
{
    /// <summary>
    /// reference for controlling the projectile
    /// </summary>
    protected EntityProjectile parentProjectile;

    #region projectile instance data

    /// <summary>
    /// Cooldown time before movement
    /// </summary>
    private float timeLeft;

    #endregion

    /// <summary>
    /// Setup internal cooldown tracker and interface getting
    /// </summary>
    void Start()
    {
        parentProjectile = GetComponent<EntityProjectile>();
    }

    /// <summary>
    /// Main loop, decrements timer when still on movement cooldown
    /// Else, resets timer then calls the actual movement behaviour defined in a actual concrete class
    /// </summary>
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = parentProjectile.movementTickTimer;
            parentProjectile.MoveProjectileGridSnap(movementRoutine());  
        }
              
    }

    /// <summary>
    /// The actual movement behaviour to be implemented in a concrete class implementation.
    /// </summary>
    protected abstract Vector2Int movementRoutine();
}

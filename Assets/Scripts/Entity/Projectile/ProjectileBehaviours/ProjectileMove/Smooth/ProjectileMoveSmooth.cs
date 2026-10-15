// Authors: [Jacky, Jeremy, Mark]
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// abstract class for projectile movement behaviour. Used abstract due to common stuff for all projectile movements (i.e timer tracking between movements)
/// TEST BEHAVIOUR FOR CHECKING ARCHITECTURE
/// </summary>
public abstract class ProjectileMoveSmooth : MonoBehaviour
{
    /// <summary>
    /// reference for controlling the projectile
    /// </summary>
    protected EntityProjectile parentProjectile;

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
        parentProjectile.MoveProjectileSmooth(MovementRoutine());
    }

    /// <summary>
    /// The actual movement behaviour to be implemented in a concrete class implementation.
    /// </summary>
    protected abstract Vector2 MovementRoutine();
}

// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Base class for snappy movement behaviour of units. This class is intended to be inherited by specific movement behaviour implementations.
/// </summary>
[RequireComponent(typeof(EntityUnit))]
public abstract class UnitMoveSnappy : MonoBehaviour
{
    protected EntityUnit parentUnit;
    private float movementCooldown;

    void Start()
    {
        parentUnit = GetComponent<EntityUnit>();
    }

    void Update()
    {
        if (movementCooldown > 0)
        {
            movementCooldown -= Time.deltaTime;
        }
        else
        {
            movementCooldown = parentUnit.GetMovementTickTimer();
            parentUnit.MoveUnitSnappy(MovementRoutine());
        }
    }

    /// <summary>
    /// The actual movement behaviour to be implemented in a concrete class implementation.
    /// </summary>
    /// <returns></returns>
    protected abstract Vector2Int MovementRoutine();
}
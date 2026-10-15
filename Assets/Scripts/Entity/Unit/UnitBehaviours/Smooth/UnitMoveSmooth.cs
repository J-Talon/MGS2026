// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Base class for smooth movement behaviour of units. This class is intended to be inherited by specific movement behaviour implementations.
/// </summary>
[RequireComponent(typeof(EntityUnit))]
public abstract class UnitMoveSmooth : MonoBehaviour
{
    protected EntityUnit parentUnit;

    void Start()
    {
        parentUnit = GetComponent<EntityUnit>();
    }

    void Update()
    {
        parentUnit.MoveUnitSmooth(MovementRoutine());
    }

    /// <summary>
    /// The actual movement behaviour to be implemented in a concrete class implementation.
    /// </summary>
    protected abstract Vector2 MovementRoutine();
}
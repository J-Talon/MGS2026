//author jacky
using System;
using UnityEngine;

/// <summary>
/// TEST BEHAVIOUR FOR TURRET, fire in a cardinal direction specified beforehand
/// </summary>
public class UnitFireCardinalPeas : UnitFire
{
    [field: SerializeField] protected Vector2Int fireDirection;

    protected override Tuple<Vector2Int, EntityType> FireRoutine()
    {
        return  Tuple.Create(fireDirection, EntityType.ProjectileTestLinearGrid); 
    } 
}

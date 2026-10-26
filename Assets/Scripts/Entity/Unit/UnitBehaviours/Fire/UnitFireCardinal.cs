//author jacky
using System;
using UnityEngine;

/// <summary>
/// TEST BEHAVIOUR FOR TURRET, fire in a cardinal direction specified beforehand
/// also has additional checks for only firing in the cardinal direction ONLY if opposite freindly status units are on that row/col
/// </summary>
public class UnitFireCardinalPeas : UnitFire
{
    [field: SerializeField] protected Vector2Int fireDirection;

    protected override Tuple<Vector2Int, EntityType, bool> FireRoutine()
    {
        if (fireDirection == Vector2Int.zero)
        {
            Debug.LogError("must choose a cardinal direction");
            return null;
        }

        Vector2Int checkPos = Vector2Int.RoundToInt(parentUnit.GetPosition()) + fireDirection;
        IGridSystemView gridSystem = GridManager.instance;
        GridCell tile = gridSystem.GetTileAtPosition(checkPos);
        while (true) //continue scanning the cardinal direction until unit is found or until end of grid boundaries (do not fire if )
        {
            if (tile == null) //reached end, do not fire
            {
                print("could not fire, reached end of grid");
                return Tuple.Create(fireDirection, EntityType.ProjectileTestLinearGrid, false); 
            }
            else if (tile.isOccupied == true)
            { 
                if (tile.occupiedUnit.GetIsFriendly() != parentUnit.GetIsFriendly()) //if freindly status is not the same CAN fire
                {
                    return Tuple.Create(fireDirection, EntityType.ProjectileTestLinearGrid, true); 
                }
            }

            //otherwise check next pos
            checkPos += fireDirection;
            tile = gridSystem.GetTileAtPosition(checkPos);
        }
    } 
}

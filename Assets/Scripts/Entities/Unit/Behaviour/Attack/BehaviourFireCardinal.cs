//author jacky

using System;
using Entities.Abstract;
using UnityEngine;

namespace Entities.Unit.Behaviour.Attack
{
    /// <summary>
    /// TEST BEHAVIOUR FOR TURRET, fire in a cardinal direction specified beforehand
    /// also has additional checks for only firing in the cardinal direction ONLY if opposite freindly status units are on that row/col
    /// </summary>
    public class BehaviourFireCardinal : BehaviourUnitFire
    {
        [field: SerializeField] private Vector2Int fireDirection;

        protected override Tuple<Vector2Int, EntityType, bool> FireRoutine()
        {
            if (fireDirection == Vector2Int.zero)
            {
                Debug.LogError("must choose a cardinal direction");
                return null;
            }

            Vector2Int checkPos = Vector2Int.RoundToInt(unit.GetPosition()) + fireDirection;
            IGridSystemView gridSystem = GridManager.instance;
            GridCell tile = gridSystem.GetTileAtPosition(checkPos);
            while (true) //continue scanning the cardinal direction until unit is found (fire) or until end of grid boundaries (do not fire)
            {
                if (tile == null) //reached end, do not fire
                {
                    //print("could not fire, reached end of grid");
                    return Tuple.Create(fireDirection, EntityType.ProjectilePea, false); 
                }
                else if (tile.isOccupied == true)
                { 
                    if (tile.occupiedUnit.GetIsFriendly() != unit.GetIsFriendly()) //if freindly status is not the same CAN fire
                    {
                        return Tuple.Create(fireDirection, EntityType.ProjectilePea, true); 
                    }
                }

                //otherwise check next pos
                checkPos += fireDirection;
                tile = gridSystem.GetTileAtPosition(checkPos);
            }
        } 
    }
}

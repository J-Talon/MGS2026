// Authors: [Jacky, Jeremy, Mark]

using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base unit class. Will make use of composition for specific behaviours
/// </summary>
public class EntityUnit : Entity, IUnitMoveSnappy, IUnitMoveSmooth, IUnitFire
{
    /// <summary>
    /// Validates passed in firing data then creates projectile from factory
    /// </summary>
    /// <param name="deltaFiringPosition">The relative firing position to the unit</param>
    /// <param name="projectileType">actual unit to spawn</param>
    public void FireProjectile(Vector2Int deltaFiringPosition, EntityType projectileType)
    {
        Vector2 firePos = gameObject.transform.position + (Vector3Int)deltaFiringPosition;
        IGridSystemView gridManager = GridManager.instance;
        GridCell newCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(firePos));
        if (newCell != null) //new cell exists
            EntityManager.GetInstance().GetEntity(projectileType, new Vector3(firePos.x, firePos.y, -1), isFriendly);
        
    }

    /// <summary>
    /// Implementation of continuous movement
    /// </summary>
    /// <param name="deltaMovement"></param>
    public void MoveUnitSmooth(Vector2 deltaMovement)
    {
        gameObject.transform.position = gameObject.transform.position + (Vector3)deltaMovement;
    }

    /// <summary>
    /// Implementation of snappy based movement (int)
    /// also registers self to grid cell while also preventing movement if new grid cell is occupied
    /// To account for possible 0 delta movement, unregistration is done first to grid cell before registration to avoid unit accidentlaly unoccuppying its current cell
    ///
    /// Talon J: Add return true or false for whether the movement was successful
    /// </summary>
    /// <param name="deltaMovement"></param>
    public bool MoveUnitSnappy(Vector2Int deltaMovement)
    {
        Vector2 oldPos = gameObject.transform.position;
        Vector2 newPos = gameObject.transform.position + (Vector3Int)deltaMovement;
        IGridSystemView gridManager = GridManager.instance;
        GridCell newCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(newPos));
        if (newCell.isOccupied == false) //new cell is not occupied, can move there
        {
            GridCell oldCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(oldPos));
            oldCell.EntityUnitLeave(); //unregister from old cell

            newCell.EntityUnitOccupy(this); //register self to new cell
            gameObject.transform.position = newPos;
            return true;

            /*
            //debugging code for ensuring new and old cells change occupation status
            Debug.Log("-----");
            Debug.Log(newCell.isOccupied);
            Debug.Log(oldCell.isOccupied);
            Debug.Log("-----");
            */
        }
        return false;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        UnitSetup();
    }

    /// <summary>
    /// Dedicated unit setup
    /// For now, it just ensures that unit properly registers itself to its current grid cell (for grid based movement)
    /// NOTE, we are assuming that "Unit Smooth" is never used (as said by execs about not worry about units not needings to use smooth movement behaviour)
    /// </summary>
    void UnitSetup()
    {
        IGridSystemView gridManager = GridManager.instance;   
        GridCell cell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(gameObject.transform.position));
        cell.EntityUnitOccupy(this); //register self to new cell
    
        //Debug.Log("setup");
        //Debug.Log(cell.isOccupied);
    }

    public override void Tick() { }
}
// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Base unit class. Will make use of composition for specific behaviours
/// </summary>
public class EntityUnit : Entity, IUnitMoveSnappy, IUnitMoveSmooth
{
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
    /// 
    /// also registers self to grid cell while also preventing movement if new grid cell is occupied 
    /// </summary>
    /// <param name="deltaMovement"></param>
    public void MoveUnitSnappy(Vector2Int deltaMovement)
    {
        Vector2 oldPos = gameObject.transform.position;
        Vector2 newPos = gameObject.transform.position + (Vector3Int)deltaMovement;
        IGridSystemView gridManager = GridManager.instance;
        GridCell newCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(newPos));
        if (newCell.isOccupied == false) //new cell is not occupied, can move there
        {
            newCell.EntityUnitOccupy(this); //register self to new cell
            gameObject.transform.position = newPos;

            GridCell oldCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(oldPos));
            oldCell.EntityUnitLeft(); //register self to new cell
        }
        else
        {
            //do nothing ig
        }

     
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveUnitSnappy(Vector2Int.zero); //we call this right at the start to ensure we register self to grid cell at game start, not just on movement
    }

    // Update is called once per frame
    void Update()
    {
        base.Tick();
    }
}
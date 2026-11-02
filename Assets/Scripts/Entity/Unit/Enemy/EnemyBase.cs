/// Author: Angad 
using UnityEngine;

/// <summary>
/// Base enemy class, attatch to enemy game objects
/// </summary>
public class EnemyBase : EntityUnit
{
    // holds enemy behaviours such as random movement, attacking, etc.
    private EnemyBehaviour[] ebehaviours;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();

        // add attatched behabiours
        ebehaviours = GetComponents<EnemyBehaviour>();
    }

    void Update()
    {
        Tick();
    }

    public override void Tick()
    {
        base.Tick();

        if (!isAlive) return; // in case of dead enemy

        // tick each behaviour
        foreach (EnemyBehaviour ebehaviour in ebehaviours)
        {
            ebehaviour.Tick(this);
        }
    }

    public bool MoveUp()
    {
        return TryMoving(Vector2Int.up);
    }

    public bool MoveRight()
    {
        return TryMoving(Vector2Int.right);
    }

    public bool MoveDown()
    {
        return TryMoving(Vector2Int.down);
    }

    public bool MoveLeft()
    {
        return TryMoving(Vector2Int.left);
    }

    // function to try to move in a direction, if it can it will
    protected bool TryMoving(Vector2Int dir)
    {
        Vector2Int currentPosition = Vector2Int.RoundToInt(GetPosition());
        Vector2Int newPosition = currentPosition + dir;   

        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(newPosition);

        if (targetTile != null && !targetTile.isFriendly && !targetTile.isOccupied)
        {
            MoveUnitSnappy(newPosition-currentPosition);
            return true;
            // Debug.Log("Valid move: " + dir.dir);
        }
        return false;
    }
}

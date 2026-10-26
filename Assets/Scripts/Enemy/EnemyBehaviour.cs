using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System;
using System.Linq;

public class EnemyBehaviour : EntityUnit
{
    private float movementCooldownTimer = 0f; //a timer that actually keeps track of the remaining time before the next movement

    private List<Func<bool>> movements = new List<Func<bool>>();

    protected new void Awake()
    {
        movements.Add(MoveUp);
        movements.Add(MoveRight);
        movements.Add(MoveDown);
        movements.Add(MoveLeft);
    } 

    protected bool MoveUp()
    {
        return TryMoving(Vector2Int.up);
    }

    protected bool MoveRight()
    {
        return TryMoving(Vector2Int.right);
    }

    protected bool MoveDown()
    {
        return TryMoving(Vector2Int.down);
    }

    protected bool MoveLeft()
    {
        return TryMoving(Vector2Int.left);
    }

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

    private void RandomlyMove()
    {
        List<Func<bool>> movementsLeft = new List<Func<bool>>(movements);

        foreach(Func<bool> movementDirection in movementsLeft.ToList())
        {
            Func<bool> randomMovement = movements[Random.Range(0,movements.Count)];
            if (randomMovement())
            {
                movementCooldownTimer = GetMovementTickTimer();
                return;
            }
            else movementsLeft.Remove(movementDirection);
        }
    }

    protected virtual void ProcessAIMovement(Entity entity)
    {
        //checks the movementCooldownTimer. If the countdown is greater than 0, continue counting down
        if (movementCooldownTimer > 0f)
        {
            movementCooldownTimer -= Time.deltaTime;
        }
        else RandomlyMove();
    }
}

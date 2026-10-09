// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

public class UnitMoveSnappyRandom : UnitMoveSnappy
{
    readonly IGridSystemView gridView = GridManager.instance;

    private Vector2Int GenerateRandomDirection()
    {
        // Generate a random direction for movement
        int randomDirection = Random.Range(0, 4);
        Vector2Int deltaVector = randomDirection switch
        {
            0 => Vector2Int.up,
            1 => Vector2Int.down,
            2 => Vector2Int.left,
            _ => Vector2Int.right,
        };
        return deltaVector;
    }

    protected override Vector2Int MovementRoutine()
    {
        Vector2Int deltaVector = GenerateRandomDirection();
        GridCell targetPosition = gridView.GetTileAtPosition(Vector2Int.RoundToInt(parentUnit.GetPosition()) + deltaVector);
        if (targetPosition == null || targetPosition.isOccupied || parentUnit.GetIsFriendly() != targetPosition.isFriendly)
        {
            // If the new position is out of bounds, No movement
            deltaVector = Vector2Int.zero;
        }
        return deltaVector;
    }
}
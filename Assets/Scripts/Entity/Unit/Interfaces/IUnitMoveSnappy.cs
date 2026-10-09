// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

public interface IUnitMoveSnappy
{
    /// <summary>
    /// Snappy based movement
    /// </summary>
    /// <param name="deltaMovement"></param>
    public void MoveUnitSnappy(Vector2Int deltaMovement);
}
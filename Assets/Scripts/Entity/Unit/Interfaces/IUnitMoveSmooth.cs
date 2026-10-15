// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

public interface IUnitMoveSmooth
{
    /// <summary>
    /// Continuous based movement (i.e PvZ)
    /// </summary>
    /// <param name="deltaMovement"></param>
    public void MoveUnitSmooth(Vector2 deltaMovement);
}
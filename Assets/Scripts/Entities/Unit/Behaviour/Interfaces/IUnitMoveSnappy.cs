// Authors: [Jacky, Jeremy, Mark]

using UnityEngine;

namespace Entities.Unit.Behaviour.Interfaces
{
    public interface IUnitMoveSnappy
    {
        /// <summary>
        /// Snappy based movement
        /// Return: Whether the move was successful on the grid
        /// </summary>
        /// <param name="deltaMovement"></param>
        public bool MoveUnitSnappy(Vector2Int deltaMovement);
    }
}
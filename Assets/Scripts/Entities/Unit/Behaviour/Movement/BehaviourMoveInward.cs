// Author: Jeremy
using UnityEngine;
using Entities.Abstract;

namespace Entities.Unit.Behaviour.Movement
{
    public class BehaviourMoveInward : EntityBehaviour
    {
        public override BehaviourAction BehaviourAction { get; } = BehaviourAction.Move;
        public override BehaviourType BehaviourType { get; } = BehaviourType.Offensive;
        public override BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.Medium;
        protected IGridSystemView gridManager = GridManager.instance;

        public override bool Tick()
        {
            if (!base.RollProficiency()) return false;
            return MoveInward();
        }

        // Move towards the center of the team's zone. If the unit is already in the center, it will not move.
        private bool MoveInward()
        {
            Vector3 currentPosition = unit.GetPosition();
            (Vector2Int bottomLeft, Vector2Int topRight) = gridManager.GetEnemyGridBounds();
            Vector2Int centerPosition = new Vector2Int((bottomLeft.x + topRight.x) / 2, (bottomLeft.y + topRight.y) / 2);
            Vector2Int direction = new Vector2Int(centerPosition.x - (int)currentPosition.x, centerPosition.y - (int)currentPosition.y);

            bool attempt = false;
            bool moveHorizontallyFirst = Random.value < 0.5f;
            if (moveHorizontallyFirst)
            {
                if (direction.x > 0) attempt = unit.MoveRight();
                else if (direction.x < 0) attempt = unit.MoveLeft();

                // If the horizontal move was not successful, try vertical move
                if (!attempt)
                {
                    if (direction.y > 0) attempt = unit.MoveUp();
                    else if (direction.y < 0) attempt = unit.MoveDown();
                }
            }
            else
            {
                if (direction.y > 0) attempt = unit.MoveUp();
                else if (direction.y < 0) attempt = unit.MoveDown();

                // If the vertical move was not successful, try horizontal move
                if (!attempt)
                {
                    if (direction.x > 0) attempt = unit.MoveRight();
                    else if (direction.x < 0) attempt = unit.MoveLeft();
                }
            }

            return attempt;
        }
    }
}

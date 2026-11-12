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
            gridManager.GetEnemyGridBounds();
            return false;
        }
    }
}

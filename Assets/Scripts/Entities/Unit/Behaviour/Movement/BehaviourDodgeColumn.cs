// Author: Jeremy
using UnityEngine;
using Entities.Abstract;
using System.Collections.Generic;

namespace Entities.Unit.Behaviour.Movement
{
    public class BehaviourDodgeColumn : EntityBehaviour
    {
        public override BehaviourAction BehaviourAction { get; } = BehaviourAction.Move;
        public override BehaviourType BehaviourType { get; } = BehaviourType.Neutral;
        public override BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.High;

        public override bool Tick()
        {
            if (!base.RollProficiency()) return false;
            return DodgeColumn();
        }

        private bool DodgeColumn()
        {
            Vector3 position = unit.GetPosition();
            IReadOnlyCollection<Entity> players = EntityManager.GetInstance().GetPlayers();

            foreach (Entity player in players)
            {
                // Get player reticle positions
                Vector2Int playerPosition = player.GetComponent<PlayerController>().GetCrosshairPosition();

                // Check if the player is in the same column as the unit
                if (Mathf.Approximately(playerPosition.x, position.x))
                {
                    // Try to move left or right in a random order
                    bool moveLeftFirst = Random.value < 0.5f;
                    bool attempt = moveLeftFirst ? unit.MoveLeft() : unit.MoveRight();
                    if (!attempt) attempt = moveLeftFirst ? unit.MoveRight() : unit.MoveLeft();
                    return attempt;
                }
            }
            return false;
        }
    }
}
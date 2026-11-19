// Author: Jeremy
using UnityEngine;
using Entities.Abstract;
using System.Collections.Generic;

namespace Entities.Unit.Behaviour.Movement
{
    public class BehaviourDodgeRow : EntityBehaviour
    {
        public override BehaviourAction BehaviourAction { get; } = BehaviourAction.Move;
        public override BehaviourType BehaviourType { get; } = BehaviourType.Neutral;
        public override BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.High;

        public override bool Tick()
        {
            if (!base.RollProficiency()) return false;
            return DodgeRow();
        }

        private bool DodgeRow()
        {
            Vector3 position = unit.GetPosition();
            IReadOnlyCollection<Entity> players = EntityManager.GetInstance().GetPlayers();

            foreach (Entity player in players)
            {
                Vector3 playerPosition = player.GetPosition();

                // Check if the player is in the same row as the unit
                if (Mathf.Approximately(playerPosition.y, position.y))
                {
                    // Try to move up or down in a random order
                    bool moveUpFirst = Random.value < 0.5f;
                    bool attempt = moveUpFirst ? unit.MoveUp() : unit.MoveDown();
                    if (!attempt) attempt = moveUpFirst ? unit.MoveDown() : unit.MoveUp();
                    return attempt;
                }
            }
            return false;
        }
    }
}
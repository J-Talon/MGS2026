/// Author: Angad 

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Random enemy movement, attatch to 
namespace Entities.Unit.Behaviour.Movement
{
    public class BehaviourRandomMove : EntityBehaviour
    {
        public override BehaviourAction BehaviourAction { get; } = BehaviourAction.Move;
        public override BehaviourType BehaviourType { get; } = BehaviourType.Neutral;
        public override BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.Low;

        // ovverided tick function
        public override bool Tick()
        {
            //checks the movementCooldownTimer. If the countdown is greater than 0, continue counting down
            if (unit.GetMovementCooldownTimer() > 0f)
            {
                unit.SetMovementCooldownTimer(unit.GetMovementCooldownTimer() - Time.deltaTime);
                return false;
            }

            bool attempt = RandomlyMove(unit);
            if (attempt) unit.SetMovementCooldownTimer(unit.GetMovementTickTimer()); // If successful, reset the cooldown timer
            return attempt;
        }

        // Function for the enemy to make a single random valid move
        private bool RandomlyMove(EntitySentient enemy)
        {
            // list of all possible movements, not all may be valid ones
            List<Func<bool>> movements = new List<Func<bool>>
            {
                enemy.MoveUp,
                enemy.MoveRight,
                enemy.MoveDown,
                enemy.MoveLeft
            };

            // iterate until valid move, or don't move
            while (movements.Count > 0)
            {
                // choose random number from possible number of valid movements
                int movementIndex = Random.Range(0, movements.Count);

                // get that movement function from enemyBase corresponding to that random number
                Func<bool> movement = movements[movementIndex];

                // remove it from possible valid moves
                movements.RemoveAt(movementIndex);

                // if it was valid, that means the enemy has moved and no need to keep looping
                if (movement()) return true;
            }
            return false;
        }
    }
}

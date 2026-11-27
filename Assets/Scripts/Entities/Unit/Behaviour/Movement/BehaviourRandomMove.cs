/// Author: Angad 
//edited - Jordan -> Nov 27, 2026
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

// Random enemy movement, attatch to 
namespace Entities.Unit.Behaviour.Movement
{
    public class BehaviourRandomMove : EntityBehaviour
    {
        public override BehaviourAction BehaviourAction { get; } = BehaviourAction.Move;
        public override BehaviourType BehaviourType { get; } = BehaviourType.Neutral;
        public override BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.Low;

        // overrided tick function
        public override bool Tick()
        {
            if (!RollProficiency()) return false;
            return RandomlyMove(unit);
        }

        // Function for the enemy to make a single random valid move
        private bool RandomlyMove(EntitySentient enemy)
        {
            //if the enemy can cast spells, and they are currently inactionable, aka they are in the middle of a spell, dont move
            if (enemy.TryGetComponent<EnemySpellCaster>(out EnemySpellCaster enemySpellCaster) == true)
            {
                if (enemySpellCaster.IsActionable() == false)
                {
                    return false;
                }
            }

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

/*
CHANGELOG
///
Jordan - November 27, 2026

checks for EnemySpellCaster now to see if they can cast a spell or not because they are inactionable or not
*/
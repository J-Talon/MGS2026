/// Author: Angad
// Modified: Talon J for refactoring
using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

namespace Entities.Unit.Behaviour
{
    /// <summary>
    /// Base enemy class, attatch to enemy game objects
    /// </summary>
    public class EntitySentient : EntityUnit
    {
        // holds enemy behaviours such as random movement, attacking, etc.
        private readonly Dictionary<BehaviourPriority, List<EntityBehaviour>> movementBehaviours = new Dictionary<BehaviourPriority, List<EntityBehaviour>>();
        private readonly Dictionary<BehaviourPriority, List<EntityBehaviour>> attackBehaviours = new Dictionary<BehaviourPriority, List<EntityBehaviour>>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Awake()
        {
            base.Awake();

            // initialize dictionaries with empty arrays for each priority level
            foreach (BehaviourPriority priority in System.Enum.GetValues(typeof(BehaviourPriority)))
            {
                movementBehaviours[priority] = new List<EntityBehaviour>();
                attackBehaviours[priority] = new List<EntityBehaviour>();
            }

            // add attached behaviours to the appropriate dictionary based on their action and priority
            EntityBehaviour[] behaviours = GetComponents<EntityBehaviour>();
            foreach (EntityBehaviour ebehaviour in behaviours)
            {
                switch (ebehaviour.BehaviourAction)
                {
                    case BehaviourAction.Move:
                        movementBehaviours[ebehaviour.BehaviourPriority].Add(ebehaviour);
                        break;
                    case BehaviourAction.Attack:
                        attackBehaviours[ebehaviour.BehaviourPriority].Add(ebehaviour);
                        break;
                    default:
                        Debug.LogWarning($"Unknown behaviour action: {ebehaviour.BehaviourAction} for entity: {gameObject.name}");
                        break;
                }
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (!isAlive)
                return;

            // Tick movement and attack behaviours based on their priority and proficiency
            ExecuteBehaviours(BehaviourAction.Move, movementBehaviours);
            ExecuteBehaviours(BehaviourAction.Attack, attackBehaviours);
        }

        private void ExecuteBehaviours(BehaviourAction action, Dictionary<BehaviourPriority, List<EntityBehaviour>> behavioursDict)
        {
            float proficiency = GetProficiency(action);
            foreach (BehaviourPriority priority in System.Enum.GetValues(typeof(BehaviourPriority)))
            {
                List<Func<bool>> behaviours = new List<Func<bool>>();
                foreach (EntityBehaviour behaviour in behavioursDict[priority]) behaviours.Add(behaviour.Tick);

                bool hasExecuted = false;
                while (behaviours.Count > 0)
                {
                    // choose random number from possible number of valid behaviours
                    int index = Random.Range(0, behaviours.Count);
                    Func<bool> behaviour = behaviours[index];
                    behaviours.RemoveAt(index);

                    // roll to determine if the behaviour should be executed based on proficiency
                    float roll = Random.Range(0f, 1f);
                    if (roll > proficiency) continue; // If the roll is greater than the proficiency, skip this behaviour

                    // Execute the behaviour
                    hasExecuted = behaviour();
                    // Debug.Log($"Executed behaviour: {behaviour.Method.DeclaringType.Name}");
                    if (hasExecuted && action == BehaviourAction.Move) break; // if a movement successfully executed, don't tick the rest
                }
                if (hasExecuted && action == BehaviourAction.Move) break; // if a movement successfully executed, don't tick the rest
            }
        }

        /// <summary>
        /// Returns the proficiency value for the given behaviour action. This value is used to determine the likelihood of executing a behaviour.
        /// </summary>
        /// <returns></returns>
        private float GetProficiency(BehaviourAction action)
        {
            switch (action)
            {
                case BehaviourAction.Move:
                    return base.GetMovementProficiency();
                case BehaviourAction.Attack:
                    return base.GetAttackProficiency();
                default:
                    Debug.LogWarning($"Unknown behaviour action: {action} for entity: {gameObject.name}");
                    return 0f;
            }
        }
    }
}

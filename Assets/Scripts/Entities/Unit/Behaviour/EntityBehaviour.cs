/// Author: Angad 

using UnityEngine;

// base behaviour abstract class, other's inherit from this
// requires EnemyBase to be attatched

namespace Entities.Unit.Behaviour
{
    public enum BehaviourAction
    {
        Move,
        Attack,
        None,
    }

    // Currently not used, but can be used to categorize behaviours in the future
    public enum BehaviourType
    {
        Offensive,
        Defensive,
        Neutral,
    }

    public enum BehaviourPriority
    {
        High,
        Medium,
        Low,
    }

    [RequireComponent(typeof(EntitySentient))]
    public abstract class EntityBehaviour : MonoBehaviour
    {
        protected EntitySentient unit;
        public virtual BehaviourAction BehaviourAction { get; } = BehaviourAction.None;
        public virtual BehaviourType BehaviourType { get; } = BehaviourType.Neutral;
        public virtual BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.Medium;

        private void Awake()
        {
            unit = GetComponent<EntitySentient>();
            this.Initialize();
        }

        protected virtual void Initialize() { }

        // tick function all behaviours will need
        public abstract bool Tick();
    }
}

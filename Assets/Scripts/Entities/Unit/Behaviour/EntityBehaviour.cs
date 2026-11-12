/// Author: Angad 

using UnityEngine;
using Random = UnityEngine.Random;

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
        [Range(0f, 1f)] public float Proficiency = 1f;

        private void Awake()
        {
            unit = GetComponent<EntitySentient>();
            this.Initialize();
        }

        protected virtual void Initialize() { }

        // tick function all behaviours will need
        public abstract bool Tick();

        // roll to determine if the behaviour should be executed based on proficiency
        public bool RollProficiency()
        {
            float roll = Random.Range(0f, 1f);
            Debug.Log($"Rolling for proficiency: {Proficiency}, rolled: {roll}");
            if (roll <= Proficiency) return true;
            return false;
        }
    }
}

/// Author: Angad 

using UnityEngine;

// base behaviour abstract class, other's inherit from this
// requires EnemyBase to be attatched

namespace Entities.Unit.Behaviour
{
    [RequireComponent(typeof(EntitySentient))]
    public abstract class EntityBehaviour : MonoBehaviour
    {
        protected EntitySentient unit;
        private void Awake()
        {
           unit = GetComponent<EntitySentient>();
           this.Initialize();
        }

        protected virtual void Initialize() { }

        // tick function all behaviours will need
        public abstract void Tick();
    }
}

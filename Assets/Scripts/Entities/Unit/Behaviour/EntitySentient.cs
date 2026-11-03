/// Author: Angad
// Modified: Talon J for refactoring

namespace Entities.Unit.Behaviour
{
    /// <summary>
    /// Base enemy class, attatch to enemy game objects
    /// </summary>
    public class EntitySentient : EntityUnit
    {
        // holds enemy behaviours such as random movement, attacking, etc.
        private EntityBehaviour[] ebehaviours;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        
        protected override void Awake()
        {
            base.Awake();

            // add attatched behabiours
            ebehaviours = GetComponents<EntityBehaviour>();
        }
    

        public override void Tick()
        {
            base.Tick();

            if (!isAlive)
                return;
            
            // tick each behaviour
            foreach (EntityBehaviour ebehaviour in ebehaviours)
            {
                ebehaviour.Tick();
            }
        }
    }
}

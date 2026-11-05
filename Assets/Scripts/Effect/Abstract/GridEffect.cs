using Entities.Collectable;
using Entities.Unit;
using JetBrains.Annotations;

namespace Effect.Abstract
{
    public abstract class GridEffect: Effect
    {
        
        protected GridCell affector;

        public GridEffect(GridCell affector, double duration, bool finite = true) : base(duration, finite) {
            this.affector = affector;
        }
        
        //these events are not cancellable because they are performed immediately after validity checks
        
        //called when an entity enters the grid cell
        public virtual void OnEntityEnter(EntityUnit unit) { }

        //called when an entity exits the grid cell
        public virtual void OnEntityLeave(EntityUnit unit) { }
        
    }
}
using Entities.Collectable;
using Entities.Unit;
using JetBrains.Annotations;

namespace Effect.Abstract
{
    public abstract class GridEffect: Effect
    {
        
        protected GridCell affector;

        public GridEffect(GridCell affector, double duration, bool finite) : base(duration, finite) {
            this.affector = affector;
        }

        //if the entity just spawned in then yeah previous cell is null
        public virtual void PreOnEntityEnter(EntityUnit unit, [CanBeNull] GridCell previousCell, ref bool isCancelled) { }

        public virtual void PreOnEntityLeave(EntityUnit unit, [CanBeNull] GridCell previousCell, ref bool isCancelled) { }

        public virtual void OnCollectableEnter(CollectableEntity collectable, ref bool isCancelled) { }
        
    }
}
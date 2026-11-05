using Effect.Abstract;
using Effect.EntityEffect;
using Entities.Unit;

namespace Effect.GridEffect
{
    public class EffectEnsnareTrap: Abstract.GridEffect {

        public EffectEnsnareTrap(GridCell affector, double duration, bool finite) : base(affector, duration, finite) { }

        public override void OnStart() {
            //todo add vfx
        }
        public override void OnEnd() {
            //todo remove vfx
        }

        public override EffectType GetEffectType() {
            return EffectType.GRID_SNARE_TRAP;
        }

        public override void OnEntityEnter(EntityUnit unit) {
            unit.AddEffect(new EffectEnsnared(unit, 3));
        }
    }
}
using Entities.Abstract;
using Entities.Unit;
using JetBrains.Annotations;
using UnityEngine;

namespace Effect.Abstract
{
    public abstract class EntityEffect : Effect
    {
        
        protected EntityUnit affector;

        public EntityEffect(EntityUnit affector, double duration, bool finite) : base(duration, finite) {
            this.affector = affector;
        }
        
        //callbacks for events local to the current entity
        //if you need global events then sure define them below
        
        //if an entity effect needs to damage something on tick, but not have any callbacks, we should
        //use a damageRaw function that is also defined in entity
        public virtual void PreOnEntityDamage([CanBeNull] Entity damager, ref float damage, ref bool isCancelled) { }
        
        public virtual void PreOnGridMove(GridCell oldCell, GridCell newCell, Vector2Int moveDir, ref bool isCancelled) { }

        public virtual void PreOnAttack(/* Spell spell,*/ref bool isCancelled) { }  //todo change this to be a spell callback 
        
        //add other callbacks here
        
    }
}
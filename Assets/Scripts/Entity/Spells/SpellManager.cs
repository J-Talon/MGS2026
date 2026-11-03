using System;
using Entity.Abstract;
using Entity.Projectile.ProjectileTypes;
using Entity.Unit;
using Event;
using UnityEngine;

namespace Entity.Spells
{
    /// <summary>
    /// This has binds to all the spell events and summons different spells 
    /// as events are fired it should be modified when adding new spells
    /// </summary>
    ///
    [Obsolete("Use the new spell manager system")]
    class SpellManager {

        
        //temporary reroute for the meantime
        public void onSpell1(EntityUnit player)
        {
            // var (position, _) = GridManager.instance.FindAllEntities((entity) => entity.GetIsPlayer())[0];
            
            Vector3 pos = player.GetPosition();
            Vector2Int position = new Vector2Int((int)pos.x, (int)pos.y);

            // spawn
            var manager = EntityManager.GetInstance();
            manager.GetEntity(EntityType.ProjectilePea, new Vector3(position.x, position.y, -1));
        }

        public void onSpell2(EntityUnit player)
        {
            // var (position, _) = GridManager.instance.FindAllEntities((entity) => entity.GetIsPlayer())[0];
            //
            Vector3 pos = player.GetPosition();
            Vector2Int position = new Vector2Int((int)pos.x, (int)pos.y);

            var manager = EntityManager.GetInstance();
            var entity = manager.GetEntity(EntityType.ProjectileBomb, new Vector3(position.x, position.y, -1));
            var bomb = entity.gameObject.GetComponent<Bomb>();

            // SG: the lifetime of the bomb determines how long it takes to travel
            bomb.SetMaxLifeSpan(10/bomb.GetSpeed());

            // SG: the target is decided in advance and the arch is calculated from it
            bomb.target = new Vector2Int(4, 2);
            bomb.start = position;
        }

        public void onSpell3()
        {
        }

        // public void Start() {
        //     GameplayEvents.Input.spell1Pressed.AddEventListener(Spell1Event);
        //     GameplayEvents.Input.spell2Pressed.AddEventListener(Spell2Event);
        //     GameplayEvents.Input.spell3Pressed.AddEventListener(Spell3Event);
        // }
        
    }
}

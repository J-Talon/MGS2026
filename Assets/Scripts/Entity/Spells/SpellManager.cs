using UnityEngine;
using System;
using Event;

/// <summary>
/// This has binds to all the spell events and summons different spells 
/// as events are fired it should be modified when adding new spells
/// </summary>
class SpellManager : MonoBehaviour {
    public void Start() {
        GameplayEvents.Input.spell1Pressed.AddEventListener(Spell1Event);
        GameplayEvents.Input.spell2Pressed.AddEventListener(Spell2Event);
        GameplayEvents.Input.spell3Pressed.AddEventListener(Spell3Event);
    }

    // Pea Shooter Spawning
    public void Spell1Event(ValueTuple data) {
        var (position, _) = GridManager.instance.FindAllEntities((entity) => entity.GetIsPlayer())[0];

        // spawn
        var manager = EntityManager.GetInstance();
        manager.GetEntity(EntityType.ProjectilePea, new Vector3(position.x, position.y, -1));
    }

    // Bomb Spawning
    public void Spell2Event(ValueTuple data) {
        var (position, _) = GridManager.instance.FindAllEntities((entity) => entity.GetIsPlayer())[0];

        var manager = EntityManager.GetInstance();
        var entity = manager.GetEntity(EntityType.ProjectileBomb, new Vector3(position.x, position.y, -1));
        var bomb = entity.gameObject.GetComponent<Bomb>();

        // SG: the lifetime of the bomb determines how long it takes to travel
        bomb.SetMaxLifeSpan(10/bomb.GetSpeed());

        // SG: the target is decided in advance and the arch is calculated from it
        bomb.target = new Vector2Int(4, 2);
        bomb.start = position;
    }

    // Boomerang Spawning
    public void Spell3Event(ValueTuple data) {
        var (position, player) = GridManager.instance.FindAllEntities((entity) => entity.GetIsPlayer())[0];

        if (player.GetIsHoldingBoomerang()) {
            var manager = EntityManager.GetInstance();
            var entity = manager.GetEntity(EntityType.ProjectileBoomerang, new Vector3(position.x, position.y, -1));
            var boomerang = entity.gameObject.GetComponent<Boomerang>();

            // SG: the direction property doesn't work for non horizontal values but it could be fleshed out if we decide to keep this
            boomerang.direction = new Vector2(1, 0);
            player.SetIsHoldingBoomerang(false);
        }
    }
}

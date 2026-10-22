using UnityEngine;
using Particle.Base;
using System;
using Event;

class SpellManager : MonoBehaviour {
    public void Start() {
        GameplayEvents.Input.spell1Pressed.AddEventListener(Spell1Event);
        GameplayEvents.Input.spell2Pressed.AddEventListener(Spell2Event);
        GameplayEvents.Input.spell3Pressed.AddEventListener(Spell3Event);
    }

    public void Spell1Event(ValueTuple data) {
        var (position, _) = GridManager.instance.FindAllEntities((entity) => entity.GetIsPlayer())[0];

        var manager = EntityManager.GetInstance();
        var entity = manager.GetEntity(EntityType.ProjectilePea, new Vector3(position.x, position.y, -1));
        var peaBullet = entity.gameObject.GetComponent<PeaBullet>();
        peaBullet.direction = new Vector2(1, 0);
    }

    public void Spell2Event(ValueTuple data) {
        var (position, _) = GridManager.instance.FindAllEntities((entity) => entity.GetIsPlayer())[0];

        var manager = EntityManager.GetInstance();
        var entity = manager.GetEntity(EntityType.ProjectileBomb, new Vector3(position.x, position.y, -1));
        var bomb = entity.gameObject.GetComponent<Bomb>();
        bomb.SetMaxLifeSpan(10/bomb.GetSpeed());
        bomb.target = new Vector2Int(4, 2);
        bomb.start = position;
    }

    public void Spell3Event(ValueTuple data) {
    }
}

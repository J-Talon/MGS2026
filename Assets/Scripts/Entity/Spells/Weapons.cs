using UnityEngine;
using Particle.Base;
using System;
using Event;

class SpellManager : MonoBehaviour {
    GameObject pea_prefab;

    public void Start() {
        pea_prefab = Resources.Load<GameObject>("Entity/Spells/Pea");
        Debug.Assert(pea_prefab != null);

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
    }

    public void Spell3Event(ValueTuple data) {
    }
}

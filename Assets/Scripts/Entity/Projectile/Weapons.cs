using UnityEngine;
using Particle.Base;
using System;
using Event;

class SpellManager : MonoBehaviour {
    public void Awake() {
        GameplayEvents.Input.spell1Pressed.AddEventListener(Spell1Event);
        GameplayEvents.Input.spell2Pressed.AddEventListener(Spell2Event);
        GameplayEvents.Input.spell3Pressed.AddEventListener(Spell3Event);
    }    

    public void Spell1Event(ValueTuple data) {
        var (position, entity) = GridManager.instance.FindAllEntitiesWithTag("Player")[0];
        ParticleType.FIRE.Play(position.x, position.y, -1);
    }

    public void Spell2Event(ValueTuple data) {
    }

    public void Spell3Event(ValueTuple data) {
    }
}

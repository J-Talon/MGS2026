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
        //GridManager.instance
        //var position = GridManager.instance.GetTileAtPosition(playerLocation).transform.position;
        //ParticleType.FIRE.Play(position.x, position.y, -1);
    }

    public void Spell2Event(ValueTuple data) {
        //var position = GridManager.instance.GetTileAtPosition(playerLocation).transform.position;
        //Debug.Log(position);
        //ParticleType.FIRE.Play(position.x, position.y, -1);
    }

    public void Spell3Event(ValueTuple data) {
        //var position = GridManager.instance.GetTileAtPosition(playerLocation).transform.position;
        //Debug.Log(position);
        //ParticleType.FIRE.Play(position.x, position.y, -1);
    }
}

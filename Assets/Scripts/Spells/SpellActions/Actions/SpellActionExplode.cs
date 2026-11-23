//author - Jordan -> Nov 13, 2026

using UnityEngine;

//a spells most likely defualt use of the explosion/attack. If the attack doesnt need specific fancy bullshit, probs use this for the explosion attack part
public class SpellActionExplode : SpellAction
{
    private SpellExplosion explosion; //the actual attack

    //set the stuff from the settings. See ExplodeSettings.cs
    public SpellActionExplode(ExplodeSettings explodeSettings)
    {
        explosion = explodeSettings.explosion;
    }

    //kaboom
    public override void OnPerformAction()
    {
        explosion.StartExplosion(castingData.targetGrid, castingData);
        EndAction();
    }
}

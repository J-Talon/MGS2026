//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellActionBeam.cs
//probs needs more stuff later on because lazer technically should be a lasting lazer, aka multihit
public class BeamSettings : ISpellActionSettings
{
    public SpellExplosion explosion; //kaboom

    public BeamSettings(SpellExplosion spellExplosion)
    {
        explosion = spellExplosion;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionBeam(this);
    }
}

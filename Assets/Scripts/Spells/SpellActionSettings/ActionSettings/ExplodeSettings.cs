//author - Jordan -> Nov 13, 2026

using UnityEngine;

//settings for SpellActionExplode.cs
public class ExplodeSettings : ISpellActionSettings
{
    public SpellExplosion explosion; //kaboom
    
    // Capture the crosshair position when exploding
    public bool useCurrentTarget;

    public ExplodeSettings(SpellExplosion spellExplosion, bool useCurrentTarget = false)
    {
        explosion = spellExplosion;
        this.useCurrentTarget = useCurrentTarget;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionExplode(this);
    }
}

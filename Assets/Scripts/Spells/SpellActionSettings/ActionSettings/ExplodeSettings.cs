//author - Jordan -> Nov 13, 2026

using UnityEngine;

//settings for SpellActionExplode.cs
public class ExplodeSettings : ISpellActionSettings
{
    public SpellExplosion explosion; //kaboom

    public ExplodeSettings(SpellExplosion spellExplosion)
    {
        explosion = spellExplosion;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionExplode(this);
    }
}

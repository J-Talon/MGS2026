//author - Jordan -> Nov 13, 2026
using UnityEngine;

//settings for SpellActionEndless.cs
//maybe i should make the names more consistent, but thats for later, im lazy
public class MeteorSettings : ISpellActionSettings
{
    public float timeBetweenMeteors; //delay between next meteor drop
    public SpellExplosion explosion; //kaboom
    
    public MeteorSettings(float meteorDelay, SpellExplosion spellExplosion)
    {
        timeBetweenMeteors = meteorDelay;
        explosion = spellExplosion;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionEndless(this);
    }
}

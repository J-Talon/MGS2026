//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellBeginningBeam.cs
public class BeamSettings : ISpellBeginningSettings
{
    public SpellActionBeginning CreateBeginning()
    {
        return new SpellBeginningBeam();
    }
}

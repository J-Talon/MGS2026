//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellBeginningShoot.cs
//also rename this trash ass name later
public class ShootSettings : ISpellBeginningSettings
{
    public SpellActionBeginning CreateBeginning()
    {
        return new SpellBeginningShoot();
    }
}

//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellBeginningPlaceTurret.cs
public class PlaceTurretSettings : ISpellBeginningSettings
{
    public SpellActionBeginning CreateBeginning()
    {
        return new SpellBeginningPlaceTurret();
    }
}

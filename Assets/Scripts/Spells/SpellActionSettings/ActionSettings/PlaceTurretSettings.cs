//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellActionPlaceTurret.cs
public class PlaceTurretSettings : ISpellActionSettings
{
    public SpellAction CreateAction()
    {
        return new SpellActionPlaceTurret();
    }
}

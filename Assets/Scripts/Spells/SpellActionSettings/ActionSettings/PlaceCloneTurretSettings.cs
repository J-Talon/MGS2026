//author - Jordan -> Nov 27, 2026

using UnityEngine;

//settings for SpellActionPlaceTurret.cs
public class PlaceCloneTurretSettings : ISpellActionSettings
{
    public SpellAction CreateAction()
    {
        return new SpellActionPlaceCloneTurret();
    }
}

//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellActionShoot.cs
//also rename this trash ass name later
public class ShootSettings : ISpellActionSettings
{
    public SpellAction CreateAction()
    {
        return new SpellActionShoot();
    }
}

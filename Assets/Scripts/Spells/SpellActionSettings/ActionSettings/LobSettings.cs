//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellActionLob.cs
public class LobSettings : ISpellActionSettings
{
    public float travelTime; //time taken for bomb to reach targetGrid
    public float directDamage; //damage specifically if the lobbed bomb hits the head of the enemy (aka lands on them directly for bonus damage)

    public LobSettings(float spellTravelTime, float spellDirectDamage)
    {
        travelTime = spellTravelTime;
        directDamage = spellDirectDamage;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionLob(this);
    }
}



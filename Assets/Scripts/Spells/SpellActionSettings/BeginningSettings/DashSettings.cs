//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellBeginningDash.cs
public class DashSettings : ISpellBeginningSettings
{
    public float travelTime; //time taken to reach targetGrid (again, may be replaced with a speed for more consistency)
    public float slashDelay; //time taken before slashing after the the dash is done

    public DashSettings(float dashTravelTime, float spellSlashDelay)
    {
        travelTime = dashTravelTime;
        slashDelay = spellSlashDelay;
    }

    public SpellActionBeginning CreateBeginning()
    {
        return new SpellBeginningDash(this);
    }

}

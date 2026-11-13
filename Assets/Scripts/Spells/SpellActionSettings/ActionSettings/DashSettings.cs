//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellActionDash.cs
public class DashSettings : ISpellActionSettings
{
    public float travelTime; //time taken to reach targetGrid (again, may be replaced with a speed for more consistency)

    public DashSettings(float dashTravelTime)
    {
        travelTime = dashTravelTime;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionDash(this);
    }

}

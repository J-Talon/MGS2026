//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellActionRetreat.cs
public class RetreatSettings : ISpellActionSettings
{
    public float travelTime; //time taken to travel (may be replaced with a speed later, ive written this like 6 or 7 times now)
    public RetreatSettings(float dashTravelTime)
    {
        travelTime = dashTravelTime;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionRetreat(this);
    }
}

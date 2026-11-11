//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellEndingRetreat.cs
public class RetreatSettings : ISpellEndingSettings
{
    public float retreatDelay; //delay before starting the retreat
    public float travelTime; //time taken to travel (may be replaced with a speed later, ive written this like 6 or 7 times now)
    public RetreatSettings(float spellRetreatDelay, float dashTravelTime)
    {
        retreatDelay = spellRetreatDelay;
        travelTime = dashTravelTime;
    }

    public SpellActionEnding CreateEnding()
    {
        return new SpellEndingRetreat(this);
    }
}

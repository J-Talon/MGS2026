//author - Jordan -> Nov 08, 2026

using UnityEngine;

//retreating back
public class SpellEndingRetreat : SpellActionEnding
{
    
    public float retreatDelay; //time delay that freezes the player before they retreat

    //time it takes to reach the casterGrid
    //maybe better to replace with a speed, because its always the same duration no matter distance, it looks really slow for closer distance targets
    public float travelTime;
    public float travelTimer; //timer that helps the above

    //set the stuff from the settings. See RetreatSettings.cs
    public SpellEndingRetreat(RetreatSettings retreatSettings)
    {
        retreatDelay = retreatSettings.retreatDelay;
        travelTime = retreatSettings.travelTime;

        SetStartupDelay(retreatDelay); //func from SpellAction.cs
    }

    //more setting of stuff, overriden function, see SpellAction.cs and SpellActionEnding.cs
    public override void OnPerformAction()
    {
        travelTimer = travelTime;
    }

    //more setting of stuff, overriden function, see SpellAction.cs
    public override void OnTick()
    {
        //do the time stuff for the travelling
        travelTimer = travelTimer - Time.deltaTime;
        Retreat();

        //done travelling? end the spell
        if (travelTimer < 0f)
        {
            EndSpell();
        }
    }

    //lerp of the dash to move the player
    //is flawed, doesnt actually update players occupancy states, so no collecting or taking damage from the travel (players original grid takes the damage)
    public void Retreat()
    {
        float lerpTime = 1f - (travelTimer / travelTime);

        castingData.spellCaster.transform.position = Vector2.Lerp(castingData.targetGrid - castingData.direction, castingData.casterGrid, lerpTime);
    }
}
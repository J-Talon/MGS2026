//author - Jordan -> Nov 08, 2026

using UnityEngine;

//retreating back
public class SpellActionRetreat : SpellAction
{
    //time it takes to reach the casterGrid
    //maybe better to replace with a speed, because its always the same duration no matter distance, it looks really slow for closer distance targets
    public float travelTime;
    public float travelTimer; //timer that helps the above
    private Vector2 startPosition; //tracks the starting position

    //set the stuff from the settings. See RetreatSettings.cs
    public SpellActionRetreat(RetreatSettings retreatSettings)
    {
        travelTime = retreatSettings.travelTime;
    }

    //more setting of stuff, overriden function, see SpellAction.cs
    public override void OnPerformAction()
    {
        travelTimer = travelTime;
        startPosition = castingData.spellCaster.transform.position;
    }

    //more setting of stuff, overriden function, see SpellAction.cs
    public override void OnTick()
    {
        //do the time stuff for the travelling
        travelTimer = travelTimer - Time.deltaTime;
        Retreat();

        //done travelling? end the action
        if (travelTimer < 0f)
        {
            EndAction();
        }
    }

    //override the IsActionable() to instead check if the entire action itself is still running or not. If still going, stop player from controlling, if not you can control them
    //does not apply to reticle movement
    public override bool IsActionable()
    {
        if (IsActionFinished() == false)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    //lerp of the dash to move the player
    //is flawed, doesnt actually update players occupancy states, so no collecting or taking damage from the travel (players original grid takes the damage)
    public void Retreat()
    {
        float lerpTime = 1f - (travelTimer / travelTime);

        castingData.spellCaster.transform.position = Vector2.Lerp(startPosition, castingData.casterGrid, lerpTime);
    }
}
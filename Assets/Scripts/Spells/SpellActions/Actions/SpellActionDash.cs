//author - Jordan -> Nov 08, 2026

using UnityEngine;

//dashing forward
public class SpellActionDash : SpellAction
{
    //time it takes to reach the targetGrid
    //maybe better to replace with a speed, because its always the same duration no matter distance, it looks really slow for closer distance targets
    private float travelTime; 
    private float travelTimer; //timer to help with teh above
    private Vector2 startPosition; //tracks the startingPosition (mostly for chaining dashes, so the the code knows where to do teh Lerp stuff on the player thats already away from origin)

    //set the stuff from the settings. See DashSettings.cs
    public SpellActionDash(DashSettings dashSettings)
    {
        travelTime = dashSettings.travelTime;
    }

    //more setting of stuff, overriden function, see SpellAction.cs
    public override void OnPerformAction()
    {
        SetTargetGrid();
        travelTimer = travelTime;
        startPosition = castingData.spellCaster.transform.position;
    }

    //more setting of stuff, overriden function, see SpellAction.cs
    public override void OnTick()
    {
        //do the time stuff for the travelling
        travelTimer = travelTimer - Time.deltaTime;
        Dash();

        //travelling is done, now do the time stuff for the startupDelay of the slash
        if (travelTimer <= 0f)
        {
            EndAction(); //set end flag
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
    public void Dash()
    {
        float lerpTime = 1f - (travelTimer / travelTime);

        castingData.spellCaster.transform.position = Vector2.Lerp(startPosition, castingData.targetGrid - castingData.direction, lerpTime);
    }
}
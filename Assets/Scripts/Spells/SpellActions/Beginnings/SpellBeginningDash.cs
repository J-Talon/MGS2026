//author - Jordan -> Nov 08, 2026

using UnityEngine;

//dashing forward
public class SpellBeginningDash : SpellActionBeginning
{
    //time it takes to reach the targetGrid
    //maybe better to replace with a speed, because its always the same duration no matter distance, it looks really slow for closer distance targets
    private float travelTime; 
    private float travelTimer; //timer to help with teh above
    private float slashDelay; //time it takes before the slash happens (after the travelling)
    private float slashTimer; //timer to help with the above

    //set the stuff from the settings. See DashSettings.cs
    public SpellBeginningDash(DashSettings dashSettings)
    {
        travelTime = dashSettings.travelTime;
        slashDelay = dashSettings.slashDelay;
    }

    //more setting of stuff, overriden function, see SpellAction.cs and SpellActionBeginning.cs
    public override void OnPerformAction()
    {
        travelTimer = travelTime;
        slashTimer = slashDelay;
    }

    //more setting of stuff, overriden function, see SpellAction.cs
    public override void OnTick()
    {
        //do the time stuff for the travelling
        if (travelTimer > 0)
        {
            travelTimer = travelTimer - Time.deltaTime;
            Dash();
        }

        //travelling is done, now do the time stuff for the startupDelay of the slash
        else
        {
            slashTimer = slashTimer - Time.deltaTime;

            if (slashTimer <= 0f)
            {
                explosion.StartExplosion(castingData.targetGrid, castingData); //call explosion
                EndSpell(); //set end flag
            }
        }
    }

    //lerp of the dash to move the player
    //is flawed, doesnt actually update players occupancy states, so no collecting or taking damage from the travel (players original grid takes the damage)
    public void Dash()
    {
        float lerpTime = 1f - (travelTimer / travelTime);

        castingData.spellCaster.transform.position = Vector2.Lerp(castingData.casterGrid, castingData.targetGrid - castingData.direction, lerpTime);
    }
}
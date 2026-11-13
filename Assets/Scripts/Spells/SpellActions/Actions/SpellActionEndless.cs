//author - Jordan -> Nov 13, 2026

using UnityEngine;

//for a spell that doesnt end. Most likely will only be used for the ShootingStar spells
public class SpellActionEndless : SpellAction
{
    private float timeBetweenMeteors; //delay between meteor drops
    private float meteorTimer; //helps with the above
    private SpellExplosion explosion; //kaboom

    //set the stuff from the settings. See MeteorSettings.cs
    public SpellActionEndless(MeteorSettings meteorSettings)
    {
        timeBetweenMeteors = meteorSettings.timeBetweenMeteors;
        explosion = meteorSettings.explosion;
    }

    //make the spell cancelable on player controlling the character (not counting moving reticle). See SpellAction.cs
    public override bool IsCancelledOnAction()
    {
        return true;
    }

    //set stuff
    public override void OnPerformAction()
    {
        meteorTimer = timeBetweenMeteors;
    }

    //override OnTick() from SpellAction.cs. Timer stuff from travelTime before exploding
    public override void OnTick()
    {
        meteorTimer = meteorTimer - Time.deltaTime;

        //timer done
        if (meteorTimer < 0f)
        {
            SetTargetGrid(); //aim

            explosion.StartExplosion(castingData.targetGrid, castingData);

            meteorTimer = timeBetweenMeteors; //reset timer to con tinue infinitely
        }
    }
}

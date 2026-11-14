//author - Jordan -> Nov 13, 2026

using UnityEngine;

//freezes the player (besides reticle movement)
public class SpellActionWait : SpellAction
{
    public float waitTime; //the delay before the actual spell stuff commences (think of it like a sort of charge up period)
    private float waitTimer = 0f; //timer to help with the above 

    //set the stuff from the settings. See WaitSettings.cs
    public SpellActionWait(WaitSettings waitSettings)
    {
        waitTime = waitSettings.waitTime;
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

    //start the waiting, if timer done, no more waiting
    public override void OnPerformAction()
    {
        waitTimer = waitTime;

        if (waitTimer <= 0f)
        {
            EndAction();
        } 
    }

    //timer stuff, if timer done, no more waiting
    public override void OnTick()
    {
        waitTimer = waitTimer - Time.deltaTime;

        if (waitTimer <= 0f)
        {
            EndAction();
        }
    }
}

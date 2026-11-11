//author - Jordan -> Nov 08, 2026

using Entities.Unit;
using UnityEngine;

public abstract class SpellAction
{
    //an abstract class that is the basis for other SpellActions that will be made.
    //the basis for 2 classes, SpellActionBeginning and SpellActionEnding. See check SpellActionBeginning.cs and SpellActionEnding.cs
    //a spell is composed of a beginning action (mandatory), an explosion (optional), and an ending (optional).
    //if a spell needs a specific fancy thing for it (like melees dashing, or lobbing actions), this is teh place for it (or well, the beginning and ending files that inherit it are)
    //this basis has setters, ticks, and abstract/virtual functions that the beginningAction and endingAction will use to do said fancy stuff
    //check the SpellActions folder for existing SpellAction types that have already been made using this basis

    public SpellCastingData castingData; //relevant info for casting the spell (check SpellCastingData.cs)
    public SpellExplosion explosion; //the explosion (aka the actual legit attack)
    public float startupDelay; //the delay before the actual spell stuff commences (think of it like a sort of charge up period)
    private float startupTimer = 0f; //timer to help with the above 
    private bool isStartupDelayed = false; //checks for the above -> if true = do timer, if false = just do spell
    private bool isStarted = false; //checks if the spell actually started (specifafter the delay if it exists)
    private bool isFinished = false; //says if spell is done being cast (important so the current spell doesnt go nuts + lets other files know they got 1 green light to do other spells)

    //sets casting info for the spell (check SpellCastingData.cs)
    public void SetCastingData(SpellCastingData spellCastingData) 
    {
        castingData = spellCastingData;
    }

    //sets explosion for the spell (check SpellExplosion.cs)
    public void SetExplosion(SpellExplosion spellExplosion) 
    {
        explosion = spellExplosion;
    }

    //sets startupDelay for the spell
    public void SetStartupDelay(float spellStartupDelay) 
    {
        startupDelay = spellStartupDelay;
    }

    //start the Action stuff
    public void PerformAction()
    {
        //if startupDelay exists, set the stuff
        if (startupDelay > 0f)
        {
            startupTimer = startupDelay;
            isStartupDelayed = true;
        }

        //else, do spell
        else
        {
            isStarted = true;
            SetTargetGrid();
            OnPerformAction();
        }
    }

    //get the grid, override in SpellBeginning (maybe this isnt needed and can be restructured to not be overidden in SpellActionBeginning, but meh im lazy to do that)
    public virtual void SetTargetGrid()
    {
        //empty
    }

    //this func is override in the files from the folder SpellActions. These spell action styles implement their own versions of this to do fancy spell stuff
    public abstract void OnPerformAction();

    //when tick is ticked, override in override in the files fromm the folder SpellActionTypes. More of that spell specific thingamajigs
    public virtual void OnTick()
    {
        //empty
    }

    //tick, so time stuff
    public void Tick()
    {
        if (isFinished == true) //spell done cast, exit the tick stuff
        {
            return;
        }

        //count down the timer for startupDelay
        if (isStartupDelayed == true)
        {
            startupTimer = startupTimer - Time.deltaTime;

            //timer done, do spell
            if (startupTimer <= 0f)
            {
                isStartupDelayed = false;
                isStarted = true;

                SetTargetGrid();
                OnPerformAction();
            }

            return;
        }

        //spell started, tick the stuff
        if (isStarted == true)
        {
            OnTick();
        }
    }

    //easy way to flag that the action is done with in other files
    public void EndAction()
    {
        isFinished = true;
    }

    //returns info about if the action is done or no
    public bool IsActionFinished()
    {
        return isFinished;
    }
        
    //tells you if you the player cannot move your character (aka casue startupDelay, but this may get expanded on in future)
    public virtual bool IsMovementLocked()
    {
        if (isStartupDelayed == true)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}

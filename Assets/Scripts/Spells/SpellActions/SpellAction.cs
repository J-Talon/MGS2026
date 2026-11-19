//author - Jordan -> Nov 08, 2026

using Entities.Unit;
using UnityEngine;

public abstract class SpellAction
{
    //an abstract class that is the basis for other SpellActions that will be made.
    //a spell is composed of a list of actions
    //if a spell needs a specific fancy thing for it (like melees dashing, or lobbing actions), this is teh place for it
    //this basis has setters, ticks, and abstract/virtual functions that any new action files that inherit from this can use
    //check the SpellActions folder for existing SpellAction types that have already been made using this basis

    public SpellCastingData castingData; //relevant info for casting the spell (check SpellCastingData.cs)
    private bool isFinished = false; //says if spell is done being cast (important so the current spell doesnt go nuts + lets other files know they got 1 green light to do other spells)

    //sets casting info for the spell (check SpellCastingData.cs)
    public void SetCastingData(SpellCastingData spellCastingData) 
    {
        castingData = spellCastingData;
    }

    //start the Action stuff
    public void PerformAction()
    {
        OnPerformAction();
    }

    //get the grid, can be override if you want it to not aim directly at the reticle
    public virtual void SetTargetGrid()
    {
        castingData.targetGrid = castingData.crosshair.GetPosition();
    }

    //this func is override in the files from the folder SpellActions. These spell action styles implement their own versions of this to do fancy spell stuff
    public abstract void OnPerformAction();

    //when tick is ticked, can be override in override in the files fromm the folder SpellActions. More of that spell specific thingamajigs
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

        OnTick();
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
        
    //tells you if you the player cannot move your character
    public virtual bool IsMovementLocked()
    {
        return false;
    }

    //tells you if you the player cannot control the character (besides the reticle moving)
    public virtual bool IsActionable()
    {
        return true;
    }

    //tells you if you the player cancel a spell (most likely an infinite spell/ShootingStar spell) by making their character do something (besides moving the reticle)
    public virtual bool IsCancelledOnAction()
    {
        return false;
    }
}

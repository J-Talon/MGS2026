//author - Jordan -> Nov 08, 2026

using Entities.Unit;
using UnityEngine;

public class Spell
{
    //this is a spell wrapper thing

    //beginning action. Basically what the player does before the actual attack/explosion happens (like the lob, dash forward, etc). Check SpellActionBeginning.cs and SpellAction.cs
    private SpellActionBeginning beginning;

    //ending action. Basically what the player does after the actual attack/explosion happens (like a dash back, anchoring movement, etc). Check SpellActionEnding.cs and SpellAction.cs
    private SpellActionEnding ending; 

    private SpellAction currentAction; //the current performing action. It will be set to either of the 2 variables above, when its their turn to perform.

    private SpellCastingData castingData; //data relating to teh spellCaster (aka the unit who uses the spells). Check SpellCastingData.cs

    //the makings of a spell.
    //a spell is composed of a beginning action (mandatory), an explosion (optional), and an ending (optional).
    //think of the spell being made up of the windup before the attack, the attack itself, and the accompanying finishing after the fact
    //it uses SpellCastingData to determine certain factors (like direction)
    public Spell(SpellActionBeginning spellBeginning, SpellActionEnding spellEnding, SpellCastingData spellCastingData, SpellExplosion spellExplosion) 
    {
        //set stuff
        beginning = spellBeginning;
        ending = spellEnding;
        castingData = spellCastingData;

        beginning.SetCastingData(castingData);

        if (spellExplosion != null )
        {
            beginning.SetExplosion(spellExplosion);
        }

        if (ending != null)
        {
            ending.SetCastingData(castingData);
            //ending.SetExplosion(spellExplosion);
        }
    }

    //tells the SpellCaster to StartSpell, passing the who and the where (aka who is casting the spell, and the crosshair the spell will aim at if applicable). See SpellCaster.cs
    public void StartSpell(EntityUnit spellCaster, CrosshairController crosshair)
    {
        //set stuff
        castingData.SetSpellCaster(spellCaster, crosshair);

        //sets the currentAction to beginningAction and does it, go figure
        currentAction = beginning;
        currentAction.PerformAction();

        //safeguard cause theres a frame where the ending doesnt get called, leaving a slight gap, so it calls here just in case
        StartEnding();
    }

    public void Tick() //calls the action tick
    {
        currentAction.Tick();
        StartEnding(); //start endingAction when its time for it
    }

    //tells us if spell is finished being cast
    public bool IsSpellFinished() 
    {
        return currentAction.IsSpellFinished();
    }

    //tells us if our movement is locked (aka, we the player cannot move the character)
    public bool IsMovementLocked() 
    {
        return currentAction.IsMovementLocked();
    }

    //checks if the beginning action is done, if so, set the currentActionendingAction to 
    private void StartEnding()
    {
        if (currentAction == beginning && beginning.IsSpellFinished() == true && ending != null)
        {
            currentAction = ending;
            currentAction.PerformAction();
        }
    }
}
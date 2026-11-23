//author - Jordan -> Nov 08, 2026

using Entities.Unit;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    //this is a spell wrapper thing

    private List<SpellAction> actionList; //a list tht holds all the actions
    private int actionIndex = 0; //keeps track of where in the actionList we are
    private SpellAction currentAction; //the current performing action
    private SpellCastingData castingData; //data relating to teh spellCaster (aka the unit who uses the spells). Check SpellCastingData.cs

    //the makings of a spell.
    //a spell is composed of a a List of Actions, see SpellAction.cs and the related folders.
    //think of the spell being split into multiple parts/components where each action is each specific thing the spell does throughout
    //it uses SpellCastingData to determine certain factors (like direction)
    public Spell(List<SpellAction> spellActions, SpellCastingData spellCastingData) 
    {
        //set stuff
        actionList = spellActions;
        castingData = spellCastingData;

        for (int i = 0; i < actionList.Count; i = i + 1)
        {
            actionList[i].SetCastingData(castingData);
        }
    }

    //tells the SpellCaster to StartSpell, passing the who and the where (aka who cast the spell, and crosshair the spell aims at if applicable). See SpellCaster.cs
    public void StartSpell(EntityUnit spellCaster, CrosshairController crosshair)
    {
        //set stuff
        castingData.SetSpellCaster(spellCaster, crosshair);

        actionIndex = 0;
        currentAction = actionList[0]; //sets currentAction to the start of the list
        currentAction.PerformAction();
        StartNextAction();
    }

    public void Tick() //calls the action tick
    {
        currentAction.Tick();
        StartNextAction(); //start endingAction when its time for it
    }

    //tells us if spell is finished being cast
    public bool IsSpellFinished() 
    {
        return currentAction.IsActionFinished();
    }

    //tells us if our movement is locked (aka, we the player cannot move the character)
    public bool IsMovementLocked() 
    {
        return currentAction.IsMovementLocked();
    }

    //tells us if the player can or cannot do stuff with the player character (besides move the reticle). Aka, can they move, shoot, etc.
    public bool IsActionable()
    {
        return currentAction.IsActionable();
    }

    //tells us if the spell can be cancelled via doing anything with the player character, like move, shoot, etc. Does not count moving the reticle
    public void CancelSpell()
    {
        if (currentAction.IsCancelledOnAction() == true)
        {
            currentAction.EndAction();
        }
    }

    //tries to start the next action if the current one is done (assuming there is another action to do)
    public void StartNextAction()
    {
        while (currentAction.IsActionFinished() == true && actionIndex < actionList.Count - 1)
        {
            actionIndex = actionIndex + 1;
            currentAction = actionList[actionIndex];
            currentAction.PerformAction();
        }
    }
}
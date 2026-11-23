//author - Jordan -> Nov 08, 2026
//edited by Jordan - Nov 20, 2026
//edited by Jordan - Nov 22, 2026

using Entities.Unit;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCaster : MonoBehaviour
{
    //an abstract class. Split between PlayerSpellCaster and EnemySpellCaster. So they can do player/enemy specific things. Like Player aim at reticle, Enemy aim wherever the hell
    //see PlayerSpellCaster.cs and EnemySpellCaster.cs

    private EntityUnit spellCaster; //who used the spell
    public List<SpellDeck> decks = new List<SpellDeck>(); //list of decks
    private List<float> deckCooldowns = new List<float>(); //cooldown for each deck (which is set to diff values by each individual spellCard
    private List<Spell> activeSpells = new List<Spell>(); //a list that holds each spell that is currently in action (cause without it, the older active ones malfunction)

    private void Awake()
    {
        spellCaster = GetComponent<EntityUnit>();
    }

    private void Update()
    {
        //lowers the cooldowns of each deck
        for (int i = 0; i < deckCooldowns.Count; i = i + 1)
        {
            if (deckCooldowns[i] > 0f)
            {
                deckCooldowns[i] = deckCooldowns[i] - Time.deltaTime;
            }
        }

        //ticks each active spell, and if its done, remove it from the active list
        for (int i = activeSpells.Count - 1; i >= 0; i = i - 1)
        {
            activeSpells[i].Tick();

            if (activeSpells[i].IsSpellFinished() == true)
            {
                activeSpells.RemoveAt(i);
            }
        }
    }

    //get one of the decks
    public SpellDeck GetDeck(int deckIndex)
    {
        return decks[deckIndex];
    }

    //get a deck cooldown
    public float GetDeckRemainingCooldown(int deckIndex)
    {
        return deckCooldowns[deckIndex];
    }

    //sets the cooldowns -> put the current used deck on normal cooldown, and the other decks on globalCooldowns
    public void SetDeckCooldowns(SpellCard spellCard, int deckIndex)
    {
        deckCooldowns[deckIndex] = spellCard.cooldowns.deckCooldown;

        //start at i = 2 to ignore the basicShot and turrets
        for (int i = 2; i < deckCooldowns.Count; i = i + 1)
        {
            if (deckCooldowns[i] < spellCard.cooldowns.deckCooldown && i != deckIndex)
            {
                deckCooldowns[i] = spellCard.cooldowns.globalCooldown;
            }
        }
    }

    //return the direction the spell should be going
    public Vector2Int GetSpellDirection()
    {
        if (spellCaster.GetIsFriendly() == true)
        {
            return Vector2Int.right;
        }

        else
        {
            return Vector2Int.left;
        }
    }

    //cast a spell from a deck
    public void CastDeckSpell(int deckIndex)
    {
        //if player is not allowed to control their character at the moment, then no spell cast
        if (IsActionable() == false)
        {
            return;
        }

        //deck on cooldown, no spell cast
        if (deckCooldowns[deckIndex] > 0f)
        {
            return;
        }

        //cancel any active spells that are of the endless variant
        CancelEndlessSpells(); 

        //grab the spell we gonna use
        SpellCard spellCard = decks[deckIndex].GetCurrentSpell();

        //set direction, depending on team alignment
        Vector2Int direction = GetSpellDirection();
        
        //build the spell. See SpellBuilder.cs
        Spell spell = SpellBuilder.CreateSpell(spellCard, direction);

        //shift to the next spell in the deck as we built the current one and are about to use it
        decks[deckIndex].ShiftToNextSpell();

        //starts the spell (like actually using the spell)
        spell.StartSpell(spellCaster, GetTargetGrid);

        //set the deck cooldown
        SetDeckCooldowns(spellCard, deckIndex);

        //add to the activeSpells list for tracking
        if (spell.IsSpellFinished() == false)
        {
            activeSpells.Add(spell);
        }
    }

    //sets the decks (used in the temporary SpellDeckInjector.cs)
    public void SetDeck(List<SpellDeck> spellDecks)
    {
        decks = spellDecks;

        //add cooldown placeholders (so each list can have the same length)
        for (int i = 0; i < decks.Count; i = i + 1)
        {
            deckCooldowns.Add(0f);
        }
    }

    //an abstract class the Player/EnemySpellCaster will override to get the target reference
    public abstract Vector2Int GetTargetGrid();
    
    //checks if one of the spells has movementlocking
    public bool IsMovementLocked()
    {
        for (int i = 0; i < activeSpells.Count; i = i + 1)
        {
            if (activeSpells[i].IsMovementLocked() == true)
            {
                return true;
            }
        }

        return false;
    }

    //checks if one of the spells locks player from controlling their character
    public bool IsActionable()
    {
        for (int i = 0; i < activeSpells.Count; i = i + 1)
        {
            if (activeSpells[i].IsActionable() == false)
            {
                return false;
            }
        }

        return true;
    }

    //if there are any active spells that are of the endless variant (most likely a ShootingStar spell), then finish them. See SpellActionEndless.cs
    public void CancelEndlessSpells()
    {
        for (int i = 0; i < activeSpells.Count; i = i + 1)
        {
            activeSpells[i].CancelSpell();
        }
    }
}

/*
Changelog

///
Jordan - Nov 20, 2026

is now an abstract class that is split between PlayerSpellCaster and EnemySpellCaster.
more fancy specific stuff you can do now for each, yay big fun

///
Jordan - Nov 22, 2026

added support for GlobalCooldowns (aka GCDs). Basically, when a spell is used, it will set its own deck on a normal cooldwon, but also set all the other decks on a globalCooldown. 
this prevents mashing of all spells at once (and posssibly breaking the game). 
the globalCooldown applied to the other decks are the same value, so they can all be available at the same time for your selection
if an existing cooldown is already in progress, take the higher of the 2 cooldowns
(ex - deck 1 already has a 6 seconds cooldown, but the GCD applied from using deck 2 is 1.4 seconds. The higher cooldown takes priority, so in this case, its the normal 6 seconds)

also turned some existing code into functions to make it easier to read/call and a bit less bloated (aka, GetSpellDirection() and SetDeckCooldowns())
*/

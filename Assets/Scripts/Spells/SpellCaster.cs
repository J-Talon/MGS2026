//author - Jordan -> Nov 08, 2026

using Entities.Unit;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    private EntityUnit spellCaster; //who used the spell
    public List<SpellDeck> decks = new List<SpellDeck>(); //list of decks
    private List<float> deckCooldowns = new List<float>(); //cooldown for each deck (which is set to diff values by each individual spellCard
    private List<Spell> activeSpells = new List<Spell>(); //a list that holds each spell that is currently in action (cause without it, the older active ones malfunction)
    private CrosshairController crosshair; //the aim reticle

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

    //cast a spell from a deck
    public void CastDeckSpell(int deckIndex)
    {
        //deck on cooldown, no spell cast
        if (deckCooldowns[deckIndex] > 0f)
        {
            return;
        }

        //grab the spell we gonna use
        SpellCard spellCard = decks[deckIndex].GetCurrentSpell();

        //set direction, depending on team alignment
        Vector2Int direction; 

        if (spellCaster.GetIsFriendly() == true)
        {
            direction = Vector2Int.right;
        }

        else
        {
            direction = Vector2Int.left;
        }

        //build the spell. See SpellBuilder.cs
        Spell spell = SpellBuilder.CreateSpell(spellCard, direction);

        //shift to the next spell in the deck as we built the current one and are about to use it
        decks[deckIndex].ShiftToNextSpell();

        //starts the spell (like actually using the spell)
        spell.StartSpell(spellCaster, crosshair);

        //set the deck cooldown
        deckCooldowns[deckIndex] = spellCard.cooldown;

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

    //sets the aim reticle reference
    public void SetCrossHair(CrosshairController crosshairController)
    {
        crosshair = crosshairController;
    }

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
}

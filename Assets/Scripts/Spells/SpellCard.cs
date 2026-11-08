//author - Jordan -> Nov 08, 2026

using System.Collections.Generic;
using UnityEngine;

//holds alot of teh revelant data for the spell. Used by the SpellBuilder.cs to make the spell
public class SpellCard 
{
    public string name = "Lazer"; //spell name
    public SpellClass spellClass = SpellClass.Helianthus; //determines which deck it goes into (a deck only holds 1 class of cards). Check SpellClass.cs + SpellDeck.cs
    public SpellExplosion explosion; //the explosion it uses. See SpellExplosion.cs
    public float cooldown = 2f; //cooldown the spell puts its whole deck on
    public float startupDelay = 0f; //optional startupDelay that freezes the player before the spell does anything actions

    //interface spell settings, check the SpellActionSettings folder -> ISpellBeginningSettings + ISpellEndingSettings
    //these are for the different spells/actions when they have special parameters that need to be set (like LobSettings, DashSettings, etc)
    public ISpellBeginningSettings beginningSettings = null;
    public ISpellEndingSettings endingSettings = null;

    //what goes into a spellCard.
    //StartupDelay defaults to 0 if not specified (though you need to specify it even if its 0, if you have an ISpellEndingSettings, because i suck at architecture) 
    //ISpellEndingSettings defaults to null if not specified
    public SpellCard(string spellName, SpellClass cardSpellClass, ISpellBeginningSettings spellBeginning, SpellExplosion spellExplosion, float spellCooldown, float spellStartupDelay = 0f, ISpellEndingSettings spellEnding = null)
    {
        name = spellName;
        spellClass = cardSpellClass;
        beginningSettings = spellBeginning;
        explosion = spellExplosion;
        cooldown = spellCooldown;
        startupDelay = spellStartupDelay;
        endingSettings = spellEnding;
    }
}

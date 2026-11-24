//author - Jordan -> Nov 08, 2026
//edited - Jordan -> Nov 22, 2026

using System.Collections.Generic;
using UnityEngine;

//holds alot of teh revelant data for the spell. Used by the SpellBuilder.cs to make the spell
public class SpellCard 
{
    public string name; //spell name
    public SpellClass spellClass; //determines which deck it goes into (a deck only holds 1 class of cards). Check SpellClass.cs + SpellDeck.cs
    public SpellCooldown cooldowns; //cooldown the spell puts its whole deck on

    //interface spell settings, check the SpellActionSettings folder
    //this is a list that holds different settings that each have special parameters that need to be set (like LobSettings, DashSettings, etc). See ISpellActionSettings.cs
    public List<ISpellActionSettings> actionSettings;

    //what goes into a spellCard. Now alot less turkey bloated
    public SpellCard(string spellName, SpellClass cardSpellClass, SpellCooldown deckCooldowns, List<ISpellActionSettings> spellActionSettings)
    {
        name = spellName;
        spellClass = cardSpellClass;
        cooldowns = deckCooldowns;
        actionSettings = spellActionSettings;
    }
}

/*
Changelog

///
Jordan - Nov 22, 2026

use a SpellCooldown class for the cooldowns. This is because we now have normal deckCooldown and globalCooldown. See SpellCaster.cs for a better idea of how this works
*/

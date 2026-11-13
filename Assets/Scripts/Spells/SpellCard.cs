//author - Jordan -> Nov 08, 2026

using System.Collections.Generic;
using UnityEngine;

//holds alot of teh revelant data for the spell. Used by the SpellBuilder.cs to make the spell
public class SpellCard 
{
    public string name = "Lazer"; //spell name
    public SpellClass spellClass = SpellClass.Helianthus; //determines which deck it goes into (a deck only holds 1 class of cards). Check SpellClass.cs + SpellDeck.cs
    public float cooldown = 2f; //cooldown the spell puts its whole deck on

    //interface spell settings, check the SpellActionSettings folder
    //this is a list that holds different settings that each have special parameters that need to be set (like LobSettings, DashSettings, etc). See ISpellActionSettings.cs
    public List<ISpellActionSettings> actionSettings;

    //what goes into a spellCard. Now alot less turkey bloated
    public SpellCard(string spellName, SpellClass cardSpellClass, float spellCooldown, List<ISpellActionSettings> spellActionSettings)
    {
        name = spellName;
        spellClass = cardSpellClass;
        cooldown = spellCooldown;
        actionSettings = spellActionSettings;
    }
}

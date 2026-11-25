//author - Jordan -> Nov 22, 2026

using UnityEngine;

public class SpellCooldown
{
    //a class about the 2 different cooldowns. The normal deck cooldown and globalCooldown (GCD)
    //basically, when a spell is used, it will set its own deck on a normal cooldown, but also set all the other decks on a globalCooldown. 
    //this prevents mashing of all spells at once(and posssibly breaking the game). 
    //the globalCooldown applied to the other decks are the same value, so they can all be available at the same time for your selection
    //if an existing cooldown is already in progress, take the higher of the 2 cooldowns
    //ex - deck 1 already has a 6 seconds cooldown, but the GCD applied from using deck 2 is 1.4 seconds.The higher cooldown takes priority, so in this case, its the normal 6 seconds

    public float deckCooldown;

    public float globalCooldown;

    public SpellCooldown(float spellDeckCooldown, float spellGlobalCooldown)
    {
        deckCooldown = spellDeckCooldown;
        globalCooldown = spellGlobalCooldown;
    }
}

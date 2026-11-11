//author - Jordan -> Nov 08, 2026

using UnityEngine;

//this a helper, aka a builder that makes the spells. 
public class SpellBuilder
{
    //the spellCard holding the settings for the beginning, explosion, and ending are built into a usable spell here. Also direction is set)
    public static Spell CreateSpell(SpellCard spellCard, Vector2Int direction)
    {
        //build the beginning - see ISpellBeginningSettings
        SpellActionBeginning beginning = spellCard.beginningSettings.CreateBeginning();


        //build the ending, if it exists - see ISpellEndingSettings
        SpellActionEnding ending = null;

        if (spellCard.endingSettings != null)
        {
            ending = spellCard.endingSettings.CreateEnding();
        }

        //set the startupDelay, and return the built Spell
        beginning.SetStartupDelay(spellCard.startupDelay);
        return new Spell(beginning, ending, new SpellCastingData(direction), spellCard.explosion);
    }
}

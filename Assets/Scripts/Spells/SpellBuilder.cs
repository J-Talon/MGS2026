//author - Jordan -> Nov 08, 2026

using System.Collections.Generic;
using UnityEngine;

//this a helper, aka a builder that makes the spells. 
public class SpellBuilder
{
    //the spellCard holds the list of settings that are then built into a usable spell here. Also direction is set)
    public static Spell CreateSpell(SpellCard spellCard, Vector2Int direction)
    {
        List<SpellAction> actionList = new List<SpellAction>();

        for (int i = 0; i < spellCard.actionSettings.Count; i = i + 1)
        {
            actionList.Add(spellCard.actionSettings[i].CreateAction());
        }

        return new Spell(actionList, new SpellCastingData(direction));
    }
}

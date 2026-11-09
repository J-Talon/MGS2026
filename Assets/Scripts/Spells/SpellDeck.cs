//author - Jordan -> Nov 08, 2026

using System.Collections.Generic;
using UnityEngine;

//spellDeck holds spellCards of a certain class
public class SpellDeck 
{
    public string deckName = "Deck";
    public SpellClass deckClass = SpellClass.Helianthus;
    public List<SpellCard> spellCardList = new List<SpellCard>(); //spellDeck holds a list of SpellCards, go figure

    private int spellIndex = 0; //spellIndex

    //SpellDeck has class (becasue it only holds a cards of a certain class, and the list of cards)
    public SpellDeck (SpellClass spellClass, List<SpellCard> spellCardDeck)
    {
        deckClass = spellClass;
        spellCardList = spellCardDeck;
    }

    //get spell at current index (aka currentSpell from the deck you can use)
    public SpellCard GetCurrentSpell()
    {
        return spellCardList[spellIndex];
    }

    //you used a spell, it goes to the next spell, aka move the index by 1
    public void ShiftToNextSpell()
    {
        if (spellIndex >= spellCardList.Count - 1)
        {
            spellIndex = 0;
        }

        else
        {
            spellIndex = spellIndex + 1;
        }
    }

    //add spell to deck (moreso for later when we have the actual getting new spells, like a shop or reward thingy)
    public void AddSpell(SpellCard spellCard)
    {
        spellCardList.Add(spellCard);
    }
}

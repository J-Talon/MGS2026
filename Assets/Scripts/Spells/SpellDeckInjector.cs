//author - Jordan -> Nov 08, 2026

using System.Collections.Generic;
using UnityEngine;

//temp thing to inject spells into the deck, and inject that deck into teh spellCaster

//format of how to set a SpellDeck and SpellCard

//SpellDeck(spellClass, list of SpellCards
//SpellCard(name, spellClass,
//beginningSettings
//explosion
//cooldown, startupDelay (optional)
//endingSettings) (optional)
public class SpellDeckInjector : MonoBehaviour
{
    private List<SpellDeck> spellDecks = new List<SpellDeck>
    {
        //basicShot deck
        new SpellDeck(SpellClass.Lathyrus, new List<SpellCard>
        {
            new SpellCard("Pea", SpellClass.Lathyrus,
                new ShootSettings(),
                null,
                0.15f)
        }),

        //turret deck
        new SpellDeck(SpellClass.Amorphophallus, new List<SpellCard>
        {
            new SpellCard("PeaShooterTurret", SpellClass.Amorphophallus,
                new PlaceTurretSettings(),
                null,
                2f, 1.5f,
                new AnchorSettings(1.5f))
        }),

        //lazer deck
        new SpellDeck(SpellClass.Helianthus, new List<SpellCard>
        {
            new SpellCard("Lazer", SpellClass.Helianthus,
                new BeamSettings(),
                new SpellExplosion(SpellShapes.Lazer, 1, 10f),
                4f, 0f,
                new AnchorSettings(2f))
        }),

        //bomb deck
        new SpellDeck(SpellClass.Impatiens, new List<SpellCard>
        {
            new SpellCard("PlusBomb", SpellClass.Impatiens,
                new LobSettings(1f, 5f),
                new SpellExplosion(SpellShapes.Cross, 1, 15f),
                1, 1f),

            new SpellCard("DiagonalBomb", SpellClass.Impatiens,
                new LobSettings(0.5f, 2f),
                new SpellExplosion(SpellShapes.Diagonal, 1, 20f),
                1f),

            new SpellCard("QuadBomb", SpellClass.Impatiens,
                new LobSettings(2f, 10f),
                new SpellExplosion(SpellShapes.Quad, 1, 50f),
                1f, 2f)
        }),

        //melee deck
        new SpellDeck(SpellClass.Wisteria, new List<SpellCard>
        {
            new SpellCard("MeleeDiagonalSlash", SpellClass.Wisteria,
                new DashSettings(0.15f, 0f),
                new SpellExplosion(SpellShapes.Diagonal, 1, 50f),
                1f, 0f,
                new RetreatSettings(0.25f, 0.15f))
        }),
    };

    //set the decks
    void Start()
    {
        SpellCaster spellCaster = GetComponent<SpellCaster>();
        spellCaster.SetDeck(spellDecks);
    }
}

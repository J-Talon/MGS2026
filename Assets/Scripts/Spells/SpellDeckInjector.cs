//author - Jordan -> Nov 08, 2026

using System.Collections.Generic;
using UnityEngine;

//temp thing to inject spells into the deck, and inject that deck into teh spellCaster

//format of how to set a SpellDeck and SpellCard

//SpellDeck(spellClass, list of SpellCards
//SpellCard(name, spellClass, deckCooldown,
//  list of Settings))

public class SpellDeckInjector : MonoBehaviour
{
    private List<SpellDeck> spellDecks = new List<SpellDeck>
    {
        //basicShot deck
        new SpellDeck(SpellClass.Lathyrus, new List<SpellCard>
        {
            new SpellCard("Pea", SpellClass.Lathyrus, new SpellCooldown(0.15f, 0.1f),
                new List<ISpellActionSettings>{
                    new ShootSettings()
                }
            )
        }),

        //turret deck
        new SpellDeck(SpellClass.Amorphophallus, new List<SpellCard>
        {
            /*
            new SpellCard("PeaShooterTurret", SpellClass.Amorphophallus, new SpellCooldown(2f, 3f),
                new List<ISpellActionSettings> {
                    new PlaceTurretSettings(),
                    new WaitSettings(1.5f),
                }
            ),
            */
            new SpellCard("CloneTurret", SpellClass.Amorphophallus, new SpellCooldown(2f, 3f),
                new List<ISpellActionSettings> {
                    new PlaceCloneTurretSettings(),
                    new WaitSettings(1.5f),
                }
            )   
        }),
        
        //lazer deck
        new SpellDeck(SpellClass.Helianthus, new List<SpellCard>
        {
            new SpellCard("Lazer", SpellClass.Helianthus, new SpellCooldown(2f, 3f),
                new List<ISpellActionSettings>
                {
                    new WaitSettings(1f),
                    new BeamSettings(new SpellExplosion(SpellShapes.Lazer, 1, 100f)),
                    new WaitSettings(0.5f)
                }
            )
        }),
        
        //bomb deck
        new SpellDeck(SpellClass.Impatiens, new List<SpellCard>
        {
            new SpellCard("PlusBomb", SpellClass.Impatiens, new SpellCooldown(1f, 5f),
                new List<ISpellActionSettings>
                {
                    new WaitSettings(1f),
                    new LobSettings(1f, 5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Cross, 1, 15f))
                }
            ),

            new SpellCard("DiagonalBomb", SpellClass.Impatiens, new SpellCooldown(1f, 3f),
                new List<ISpellActionSettings>
                {
                    new LobSettings(0.5f, 2f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Diagonal, 1, 20f))
                }
            ),

            new SpellCard("QuadBomb", SpellClass.Impatiens, new SpellCooldown(1f, 3f),
                new List<ISpellActionSettings>
                {
                    new WaitSettings(2f),
                    new LobSettings(2f, 10f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Quad, 1, 50f))
                }
            )
        }),
        
        //melee deck
        new SpellDeck(SpellClass.Wisteria, new List<SpellCard>
        {
            new SpellCard("MeleeDiagonalSlash", SpellClass.Wisteria, new SpellCooldown(2f, 3f),
                new List<ISpellActionSettings>
                {
                    new DashSettings(0.15f),
                    new WaitSettings(0.25f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Diagonal, 1, 50f)),
                    new WaitSettings(0.15f),
                    new RetreatSettings(0.25f)
                }
            ),

            new SpellCard("MeleeMultiCrossSlash", SpellClass.Wisteria, new SpellCooldown(3f, 3f),
                new List<ISpellActionSettings>
                {
                    new DashSettings(0.15f),
                    new WaitSettings(0.25f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Cross, 1, 35f)),
                    new DashSettings(0.05f),
                    new WaitSettings(0.5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Cross, 1, 35f)),
                    new DashSettings(0.05f),
                    new WaitSettings(0.5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Cross, 1, 35f)),
                    new WaitSettings(0.15f),
                    new RetreatSettings(0.25f)
                }
            )
        }),

        //extra decks (that we obviously cant fit under 3 decks). Swap the commenting in or out for the decks to test what you want

        
        //shootingStar deck
        new SpellDeck(SpellClass.Dodecatheon, new List<SpellCard>
        {
            new SpellCard("ShootingStar", SpellClass.Dodecatheon, new SpellCooldown(4f, 3f),
                new List<ISpellActionSettings>
                {
                    new WaitSettings(1f),
                    new MeteorSettings(1f, new SpellExplosion(SpellShapes.Diagonal, 1, 30f)) 
                }
            )
        }),
        
        // Root deck
        new SpellDeck(SpellClass.Ficus, new List<SpellCard>
            {
                new SpellCard("RootSnare", SpellClass.Ficus, new SpellCooldown(3f, 3f),
                    new List<ISpellActionSettings>
                    {
                        new ExplodeSettings(new SpellExplosion(SpellShapes.Horizontal, 1, 0f,
                            new MutatorEnsnare()),
                            useCurrentTarget: true)
                    }
            )
            
            /*
            //crazy Spell for testing purposes of chaining a shitload of actions
            new SpellCard("TetoInFortnite", SpellClass.Ficus, new SpellCooldown(3f, 10f),
                new List<ISpellActionSettings>
                {
                    new LobSettings(1f, 5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Quad, 2, 15f)),
                    new DashSettings(0.15f),
                    new WaitSettings(0.25f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Cross, 1, 35f)),
                    new WaitSettings(0.15f),
                    new RetreatSettings(0.25f),
                    new WaitSettings(0.5f),
                    new BeamSettings(new SpellExplosion(SpellShapes.Lazer, 1, 10f)),
                    new DashSettings(0.05f),
                    new WaitSettings(0.5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Cross, 1, 35f)),
                    new DashSettings(0.05f),
                    new WaitSettings(0.5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Diagonal, 1, 35f)),
                    new WaitSettings(0.15f),
                    new RetreatSettings(0.25f),
                    new WaitSettings(0.15f),
                    new LobSettings(1f, 5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Diagonal, 2, 15f)),
                    new WaitSettings(0.15f),
                    new LobSettings(1f, 5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Quad, 2, 15f)),
                    new WaitSettings(0.5f),
                    new DashSettings(0.05f),
                    new WaitSettings(0.15f),
                    new RetreatSettings(0.25f),
                    new BeamSettings(new SpellExplosion(SpellShapes.Lazer, 1, 10f)),
                    new WaitSettings(0.15f),
                    new LobSettings(0.5f, 5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Quad, 2, 15f)),
                    new WaitSettings(0.15f),
                    new LobSettings(0.25f, 5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Cross, 2, 15f)),
                    new WaitSettings(0.15f),
                    new LobSettings(1f, 5f),
                    new ExplodeSettings(new SpellExplosion(SpellShapes.Diagonal, 2, 15f)),
                    new WaitSettings(0.15f),
                    new BeamSettings(new SpellExplosion(SpellShapes.Lazer, 1, 10f)),
                    new WaitSettings(0.25f),
                    new BeamSettings(new SpellExplosion(SpellShapes.Lazer, 2, 10f)),
                    new WaitSettings(0.35f),
                    new BeamSettings(new SpellExplosion(SpellShapes.Lazer, 3, 10f)),
                }
            )
            */
        }),
    };

    //set the decks
    void Start()
    {
        SpellCaster spellCaster = GetComponent<SpellCaster>();
        spellCaster.SetDeck(spellDecks);
    }
}

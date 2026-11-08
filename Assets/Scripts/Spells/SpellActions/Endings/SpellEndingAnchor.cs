//author - Jordan -> Nov 08, 2026

using UnityEngine;

//freezes the player after they finish their attack
public class SpellEndingAnchor : SpellActionEnding
{
    public SpellEndingAnchor(float secondsAnchored)
    {
        SetStartupDelay(secondsAnchored); //func from SpellAction.cs
    }

    public override void OnPerformAction()
    {
        EndSpell();
    }
}

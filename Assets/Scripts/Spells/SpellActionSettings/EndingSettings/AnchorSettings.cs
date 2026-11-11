//author - Jordan -> Nov 08, 2026

using UnityEngine;

//settings for SpellEndingAnchor.cs
public class AnchorSettings : ISpellEndingSettings
{
    public float anchorDuration; //how long the player is anchored for

    public AnchorSettings(float secondsAnchored)
    {
        anchorDuration = secondsAnchored;
    }

    public SpellActionEnding CreateEnding()
    {
        return new SpellEndingAnchor(anchorDuration);
    }
}

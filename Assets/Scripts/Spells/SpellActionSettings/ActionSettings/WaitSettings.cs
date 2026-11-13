//author - Jordan -> Nov 13, 2026

using UnityEngine;

//settings for SpellActionWait.cs
public class WaitSettings : ISpellActionSettings
{
    public float waitTime; //how long to freeze player (think of something like a delay period or charge up period so to say)

    public WaitSettings(float secondsAnchored)
    {
        waitTime = secondsAnchored;
    }

    public SpellAction CreateAction()
    {
        return new SpellActionWait(this);
    }
}

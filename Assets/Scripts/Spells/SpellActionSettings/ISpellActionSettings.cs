//author - Jordan -> Nov 08, 2026

using UnityEngine;

//interface. Fancy spells probs need to make their own thing based off this to hold their special specific parameters
public interface ISpellActionSettings
{
    SpellAction CreateAction(); //create action using this settings
}

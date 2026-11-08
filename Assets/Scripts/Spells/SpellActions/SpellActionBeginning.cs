//author - Jordan -> Nov 08, 2026

using UnityEngine;

//the beginning action base
//when you make a fancy spell startup phase thing, make a file that inherits from this class
public abstract class SpellActionBeginning : SpellAction
{
    public override void SetTargetGrid()
    {
        castingData.targetGrid = castingData.crosshair.GetPosition();
    }
}

//author - Jordan -> Nov 20, 2026

using UnityEngine;

public class PlayerSpellCaster : SpellCaster
{
    //the player specific version of the SpellCaster. See SpellCaster.cs (no really?)

    private CrosshairController reticle;

    //sets the aim reticle reference
    public void SetCrossHair(CrosshairController crosshairController)
    {
        reticle = crosshairController;
    }

    //the override func. Just get reticle position, thats it
    public override Vector2Int GetTargetGrid()
    {
        return reticle.GetPosition();
    }
}

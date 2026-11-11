//author - Jordan -> Nov 08, 2026

using Entities.Unit;
using Particle.Base;
using UnityEngine;

//holds the relevant spellCasting data (like what explosion pattern, who casts, etc)
public class SpellCastingData 
{
    public EntityUnit spellCaster;
    public Vector2Int casterGrid; //the users grid
    public Vector2Int targetGrid; //the grid they are aiming at
    public Vector2Int direction;
    public CrosshairController crosshair;

    //holds some data, makes it easier to call and type
    public SpellCastingData(Vector2Int directionFacing)
    {
        direction = directionFacing;
    }

    //sets stuff
    public void SetSpellCaster(EntityUnit unit, CrosshairController crosshairController)
    {
        spellCaster = unit;
        casterGrid = Vector2Int.RoundToInt(unit.GetPosition());
        crosshair = crosshairController;
    }
}

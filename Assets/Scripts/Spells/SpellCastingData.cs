//author - Jordan -> Nov 08, 2026

using Entities.Unit;
using UnityEngine;

//holds the relevant spellCasting data (who casts, the wheres, etc)
public class SpellCastingData 
{
    public EntityUnit spellCaster; //the user
    public Vector2Int casterGrid; //the users grid
    public Vector2Int targetGrid; //the grid they are aiming at
    public Vector2Int direction; //which way we shooting
    public CrosshairController crosshair; //the reticle

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

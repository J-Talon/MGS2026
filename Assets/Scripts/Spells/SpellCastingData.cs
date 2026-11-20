//author - Jordan -> Nov 08, 2026
//edited by Jordan - Nov 20, 2026

using Entities.Unit;
using UnityEngine;

public delegate Vector2Int SpellTarget(); //to help with setting the target of the spell (ex - helps with calling GetTargetGrid())

//holds the relevant spellCasting data (who casts, the wheres, etc)
public class SpellCastingData 
{
    public EntityUnit spellCaster; //the user
    public Vector2Int casterGrid; //the users grid
    public Vector2Int targetGrid; //the grid they are aiming at
    public Vector2Int direction; //which way we shooting
    public SpellTarget target; //what the user is aiming at

    //holds some data, makes it easier to call and type
    public SpellCastingData(Vector2Int directionFacing)
    {
        direction = directionFacing;
    }

    //sets stuff
    public void SetSpellCaster(EntityUnit unit, SpellTarget spellTarget)
    {
        spellCaster = unit;
        casterGrid = Vector2Int.RoundToInt(unit.GetPosition());
        target = spellTarget;
    }
}

/*
Changelog

///
Jordan - Nov 20, 2026

added the spellTarget thing and target field. 
now they call the targeting stuff instead of a hardcoded reticle only the player can use, so now you can have different targets that the different SpellCasters can use.
See SpellCaster.cs, PlayerSpellCaster.cs, and EnemySpellCaster.cs
*/

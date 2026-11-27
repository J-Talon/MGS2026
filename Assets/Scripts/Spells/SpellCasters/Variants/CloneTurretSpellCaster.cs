//author - Jordan -> Nov 27, 2026

using Event;
using System.Collections.Generic;
using UnityEngine;

public class CloneTurretSpellCaster : SpellCaster
{
    //the clone turret version of the SpellCaster. See SpellCaster.cs (no really?)
    private Vector2Int targetGrid;
    private List<SpellClass> excludedSpellClasses = new List<SpellClass> { SpellClass.Amorphophallus }; //spellClass types that the clone turret will never cast
    private SpellCaster owner; //who owns the clone turret, aka who summoned it (player, enemy, etc)
    private Vector3 originalCastPosition; //original tile the clone turret was on right when they first executed a spell (this is for spells that caused displacement)

    //sets whos the owner, aka who summoned it (player, enemy, etc)
    public void SetOwner(SpellCaster spellCaster)
    {
        owner = spellCaster;
    }

    //event stuff
    private void OnEnable()
    {
        GameplayEvents.Spells.spellCasted.AddEventListener(OnSpellCast);
    }

    private void OnDisable()
    {
        GameplayEvents.Spells.spellCasted.RemoveEventListener(OnSpellCast);
        owner = null;
    }

    private void OnDestroy()
    {
        GameplayEvents.Spells.spellCasted.RemoveEventListener(OnSpellCast);
        owner = null;
    }

    //cast the spell
    private void OnSpellCast((SpellCaster spellCaster, SpellCard spellCard) spellData)
    {
        //wrong or no owner -> do nothing
        if (owner == null || owner != spellData.spellCaster)
        {
            return;
        }

        //spell was cast of a class that the clone turret is excluded from casting
        if (excludedSpellClasses.Contains(spellData.spellCard.spellClass) == true)
        {
            return;
        }

        //set original tile clone turret was on before casting spell, then cast spell
        originalCastPosition = spellCaster.GetPosition();
        CastSpellCard(spellData.spellCard);    
    }

    //the override func. Just get a grid position to target, thats it
    //for now, hits a random grid in the row the clone turret is on
    public override Vector2Int GetTargetGrid()
    {
        List<Vector2Int> hostileCells = new List<Vector2Int>();

        Vector2Int direction = GetSpellDirection();
        Vector2Int position = Vector2Int.RoundToInt(originalCastPosition) + direction;
        IGridSystemView gridSystem = GridManager.instance;
        GridCell grid = gridSystem.GetTileAtPosition(position);

        //grab all hostile grids in the same row as the caster
        while (grid != null)
        {
            if (grid.isFriendly != spellCaster.GetIsFriendly())
            {
                hostileCells.Add(position);
            }

            position = position + direction;
            grid = gridSystem.GetTileAtPosition(position);
        }

        //return a random hostile grid to attack
        return hostileCells[Random.Range(0, hostileCells.Count)];
    }
}

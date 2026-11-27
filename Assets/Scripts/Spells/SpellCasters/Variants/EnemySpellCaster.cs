//author - Jordan -> Nov 20, 2026
//edited - Jordan -> Nov 27, 2026

using Entities.Unit;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpellCaster : SpellCaster
{
    //the enemy specific version of the SpellCaster. See SpellCaster.cs (no really?)
    private Vector2Int targetGrid;

    //the override func. Just get a grid position to target, thats it
    //for now, hits a random grid in the players row
    public override Vector2Int GetTargetGrid()
    {
        List<Vector2Int> hostileCells = new List<Vector2Int>();

        Vector2Int direction = GetSpellDirection();
        Vector2Int position = Vector2Int.RoundToInt(spellCaster.GetPosition()) + direction;
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

/*
CHANGELOG

///
Jordan -> Nov 27, 2026

changed the targeing from 5 spots ahead, to random enemy grid in the same row
*/

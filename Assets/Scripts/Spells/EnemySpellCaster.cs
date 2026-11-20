//author - Jordan -> Nov 20, 2026

using Entities.Unit;
using UnityEngine;

public class EnemySpellCaster : SpellCaster
{
    //the enemy specific version of the SpellCaster. See SpellCaster.cs (no really?)

    private EntityUnit unit;
    private Vector2Int targetGrid;

    private void Start()
    {
        unit = GetComponent<EntityUnit>();
    }

    //the override func. Just get a grid position to target, thats it
    //for now, hardcoded to just be 5 spots ahead. Need actual specific aiming functions in the future
    public override Vector2Int GetTargetGrid()
    {
        Vector2Int currentGrid = Vector2Int.RoundToInt(unit.GetPosition());

        targetGrid = currentGrid + (Vector2Int.left * 5);

        return targetGrid;
    }
}

using UnityEngine;

/*
Grid VFX
The vfx can only be played on the grid.
@author Wilson (1oopy)
*/
public class CellVFX : VisualEffectBase
{
    // Grid manager reference
    IGridSystemView gridManager = GridManager.instance;

    // Attempts to set up a VFX on a grid.
    // Fails if the initial position does not have a grid cell present there.
    public override void Setup(VFXType effectType, Vector2 initPosition)
    {
        Vector2Int fixedPos = Vector2Int.RoundToInt(initPosition);
        GridCell gridCell = gridManager.GetTileAtPosition(fixedPos);

        if (gridCell == null)
        {
            Debug.LogWarning($"Cannot set up at {initPosition}, no grid cell at location {fixedPos}");
            return;
        }

        base.Setup(effectType, initPosition);
    }

    public override void Play()
    {
        base.Play();
    }

    public override void Stop()
    {
        base.Stop();
    }

    // Attempts to find a grid cell present at the newPos location and move to it.
    public override void MoveToSnappy(Vector2 newPos)
    {
        if (!ready) {return;}

        Vector2Int fixedPos = Vector2Int.RoundToInt(newPos);
        GridCell gridCell = gridManager.GetTileAtPosition(fixedPos);

        if (gridCell == null)
        {
            Debug.LogWarning($"Cannot move to {newPos}, no grid cell at location {fixedPos}");
            return;
        }

        base.MoveToSnappy(fixedPos);
    }
}

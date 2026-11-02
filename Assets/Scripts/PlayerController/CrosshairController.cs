using System;
using UnityEngine;

/*
Helps control the player crosshair.

@author Wilson (1oopy)
*/
public class CrosshairController
{
    // References
    protected IGridSystemView gridManager;

    protected static String assetPath = "PlayerController/TestCrosshair";
    protected GameObject _prefab;
    protected CellVFX crosshair;
    protected Transform crosshairTransform;

    public Vector2Int GetPosition() {return new Vector2Int((int)crosshairTransform.position.x, (int)crosshairTransform.position.y);}

    // constructor, sets up the crosshair
    public CrosshairController(Vector2 initPosition)
    {
        _prefab = Resources.Load<GameObject>(assetPath);

        if (_prefab == null)
        {
            Debug.LogWarning($"{assetPath} does not exist (CrossHair)");
            return;
        }

        // Maybe should handle this through the VFX manager.
        crosshair = GameObject.Instantiate(_prefab).GetComponent<CellVFX>();

        if (crosshair == null)
        {
            Debug.LogWarning($"{assetPath} does not have a CellVFX component (CrossHair)");
            return;
        }

        // Setup crosshair
        crosshairTransform = crosshair.gameObject.transform;
        crosshair.Setup(VFXType.GridPersistent, initPosition); // If we spawn the crosshair from the VFX manager we don't need this

        // Set up the gridview
        // We need this so we can only move the crosshair to enemy cells and loop back around.
        gridManager = GridManager.instance;
        
    }

    public void UpdatePosition(Vector2Int playerPos)
    {
        if (Math.Abs(crosshairTransform.position.y - playerPos.y) > 0.01)
        {
            crosshair.MoveToSnappy(new Vector2(crosshairTransform.position.x, playerPos.y));
        }
    }

    // Moves the crosshair when grids are captured only if it lands on a friendly or null cell.
    public void UpdateOnGridCapture()
    {
        // TODO: Implement this when grid capturing is added
    }

    // Moves crosshair left and right
    // If we reach a friendly or null cell, loop back around.
    public void MoveOnRow(Vector2Int dir)
    {
        if (dir != Vector2Int.left && dir != Vector2Int.right) {return;}

        Vector2 crosshairPos = new(crosshairTransform.position.x, crosshairTransform.position.y);
        Vector2 newPos = crosshairPos += dir;

        GridCell gridCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(newPos));

        // If we have reached the end of the enemy grid
        if (gridCell == null || gridCell.isFriendly)
        {
            var enemyBounds = gridManager.GetEnemyGridBounds();

            if (dir == Vector2Int.right)
            {
                // Move to the left of the enemy grid boundary
                newPos = new Vector2(enemyBounds.min.x ,newPos.y);
            }
            else
            {
                // Move to the right of the enemy grid boundary
                newPos = new Vector2(enemyBounds.max.x ,newPos.y);
            }
        }

        crosshair.MoveToSnappy(newPos);
    }
}
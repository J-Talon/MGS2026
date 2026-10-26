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

        if (_prefab is null)
        {
            Debug.LogWarning($"{assetPath} does not exist (CrossHair)");
            return;
        }

        // Maybe should handle this through the VFX manager.
        crosshair = GameObject.Instantiate(_prefab).GetComponent<CellVFX>();

        if (crosshair is null)
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
        if (crosshairTransform.position.y != playerPos.y)
        {
            crosshair.MoveToSnappy(new Vector2(crosshairTransform.position.x, playerPos.y));
        }
    }

    // Moves the crosshair when grids are captured only if it lands on a friendly or null cell.
    public void UpdateOnGridCapture()
    {
        // TODO: Implement this
    }

    // Moves crosshair left and right
    // If we reach a friendly or null cell, loop back around.
    public void MoveOnRow(Vector2Int dir)
    {
        if (dir != Vector2Int.left && dir != Vector2Int.right) {return;}

        Vector2 crosshairPos = new(crosshairTransform.position.x, crosshairTransform.position.y);
        Vector2 newPos = crosshairPos += dir;

        GridCell gridCell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(newPos));
        if (gridCell is null || gridCell.isFriendly)
        {
            // Find the other side of the enemy grid.
            // Iterates through the row until we reach the end (either its null or its friendly)
            // THERE HAS TO BE A BETTER WAY OF DOING THIS. RIGHT NOW THE GRID MANAGER IS HIDING THE GRID STATE WHYYYYYYY???
            // anyways I don't want to touch the grid manager, perhaps this gets fixed later
            // TODO: maybe fix

            Vector2Int testPos = Vector2Int.RoundToInt(crosshairPos);
            Vector2Int step = -dir;

            // scary do while loop. this CANNOT go wrong....
            // Im scared
            int iter = 100;
            int count = 0;
            do
            {
                count++;
                testPos += step;
                gridCell = gridManager.GetTileAtPosition(testPos + step);
            } while ((count < iter) && ((gridCell && !gridCell.isFriendly) && (gridCell != null)));

            crosshair.MoveToSnappy(testPos);
        } 
        else
        {
            crosshair.MoveToSnappy(newPos);
        }
    }
}
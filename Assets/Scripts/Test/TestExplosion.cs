// Author: Jeremy
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Test script for creating explosions in the game.
/// Click on a GridCell to spawn the explosion at that location.
/// </summary>
public class TestExplosion : MonoBehaviour
{
    private GridCell selectedCell;

    void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Camera camera = Camera.main;
        if (camera == null)
            return;

        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        selectedCell = null;
        float nearestDistance = float.MaxValue;

        // Use rendered bounds instead of a physics raycast so cells do not need colliders.
        foreach (GridCell cell in Object.FindObjectsByType<GridCell>(FindObjectsSortMode.None))
        {
            foreach (Renderer cellRenderer in cell.GetComponentsInChildren<Renderer>())
            {
                if (cellRenderer.bounds.IntersectRay(ray, out float distance) && distance < nearestDistance)
                {
                    nearestDistance = distance;
                    selectedCell = cell;
                }
            }
        }

        Debug.Log(selectedCell != null ? $"Selected cell: {selectedCell.name}" : "No cell selected");
        if (selectedCell)
        {
            Vector2Int center = selectedCell.GetCellPosition();

            // Cross 1,1
            // Explosion.CROSS_3x3.Detonate(center);

            // Diagonal 1
            // Explosion.DIAGONAL_3x3.Detonate(center);

            // Laser down 1
            // Explosion.LASER_DOWN_1x2.Detonate(center);

            // Quad 1,1
            Explosion.QUAD_3x3.Detonate(center);

            selectedCell = null;
        }
    }
}

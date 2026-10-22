// Author: Jeremy
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction { Up, Down, Left, Right }
public static class DirectionExtensions
{
    public static Vector2Int ToVector(this Direction d) => d switch
    {
        Direction.Up => Vector2Int.up,
        Direction.Down => Vector2Int.down,
        Direction.Left => Vector2Int.left,
        Direction.Right => Vector2Int.right,
        _ => Vector2Int.zero
    };
}

/// <summary>
/// Test script for creating explosions in the game. This script allows you to specify the source of the explosion, its position, type, size, and damage.
/// Click on a GridCell to spawn the explosion at that location. The explosion will be created based on the specified parameters.
/// </summary>
public class TestExplosion : MonoBehaviour
{
    public GameObject source;
    public Vector2Int position;
    public ExplosionType explosionType;
    public int sizeX = 1;
    public int sizeY = 1;
    public float damage = 1f;
    public Direction direction = Direction.Up;
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
            List<GridCell> affectedCells = Explosion.CreateExplosion(gameObject, selectedCell.GetCellPosition(), explosionType, direction.ToVector(), sizeX, sizeY, damage);
            foreach (var cell in affectedCells)
            {
                Vector2Int cellPosition = cell.GetCellPosition();
                Vector3 spawnTarget = new Vector3(cellPosition.x, cellPosition.y, -1);
                // Debug.Log($"Creating Explosion at {spawnTarget}");
                // Spawn prefabs
                EntityManager.GetInstance().GetEntity(EntityType.ExplosionTest, spawnTarget, false, 0.5f);
            }
            selectedCell = null;
        }
    }
}

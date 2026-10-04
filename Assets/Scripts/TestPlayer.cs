// ===========================================
// Project: Terra Terralis 2026
// File: TestPlayer.cs
// Author: Samyat Gautam (github: FadedBronze)
// Description: Simple player controller script and example on how to use grid system could be expanded on or replaced
// ===========================================

using GridSystem;
using Event;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class TestPlayer : MonoBehaviour
{
    // units^-1
    [SerializeField]
    float speed = 3.0f;

    [SerializeField] 
    GridManager grid;

    private Vector2 GridPosition {
        set {
            // update the transform position
            Vector2 worldPos = grid.GridToWorldPosition(value);
            transform.position = new(worldPos.x, worldPos.y, -0.05f);

            // tells the grid position where the player is currently
            //GameplayEvents.moveGridEntity.CallEvent((gridPosition, value));
            gridPosition = value;
        }
        get {
            return gridPosition;
        }
    }
    
    [SerializeField] 
    private Vector2 gridPosition = new(1, 1);

    private List<Vector2> queuedOffsets;

    void Start()
    { 
        queuedOffsets = new(0);
        GameplayEvents.gridResized.AddEventListener(GridResizedListener);
        GameplayEvents.placeGridEntity.CallEvent(new(gridPosition, gameObject));
    }

    // updates the player scale based on tile size and triggers gridposition setter to rescale position
    void GridResizedListener(ValueTuple _) 
    {
        GridPosition = gridPosition;
        transform.localScale = new(grid.TileSize, grid.TileSize);
    }

    private bool passedTilesEdge = false;

    void Update()
    {
        Vector2 queued = queuedOffsets.Count() < 1 ? Vector2.zero : queuedOffsets.First();
        Vector2 increment = queued * Time.deltaTime * speed;

        Vector2 movement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movement.y += 1;
        else if (Keyboard.current.sKey.isPressed) movement.y -= 1;
        else if (Keyboard.current.aKey.isPressed) movement.x -= 1;
        else if (Keyboard.current.dKey.isPressed) movement.x += 1;

        bool newTile = !GridManager.OnSameTile(GridPosition, GridPosition + increment);
        bool willPassTileCenter = GridManager.PassedTileCenter(GridPosition, GridPosition + increment);

        if (newTile) {
            passedTilesEdge = true;
        }
 
        bool withinBounds = grid.WithinBounds(GridPosition + increment);
        if (withinBounds) {
            GridPosition += increment;
        } else if (queuedOffsets.Count > 0) {
            queuedOffsets.RemoveAt(0);
        }
        
        if (willPassTileCenter && passedTilesEdge) {
            if (queuedOffsets.Count > 0) {
                queuedOffsets.RemoveAt(0);
                passedTilesEdge = false;                
            } 
        }
        
        if (movement != Vector2.zero) {
            if (queuedOffsets.Count == 0 || queuedOffsets.Last() != movement) {
                queuedOffsets.Add(movement);
            }
        }
    }
}

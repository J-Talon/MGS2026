// Authors: [Jacky, Jeremy, Mark]
using Event;

using Particle.Base;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// This script manages the player controller, handling player input and movement within the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Player position variables
    private int positionX = 0;
    private int positionY = 0;

    public void OnEnable()
    {
        GameplayEvents.Input.movementInput.AddEventListener(OnMove);
    }

    private void OnDisable()
    {
        GameplayEvents.Input.movementInput.RemoveEventListener(OnMove);

    }

    /// <summary>
    /// Sets the position of the player on the grid.
    /// </summary>
    /// <param name="x">Position of GridCell in X-axis</param>
    /// <param name="y">Position of GridCell in Y-axis</param>
    public void SetPlayerPosition(int x, int y)
    {
        // Set z to -1 to ensure the player is in front of the grid
        transform.position = new Vector3(x, y, -1);
        positionX = x;
        positionY = y;
    }

    /// <summary>
    /// Handles player movement input for moving around on the grid in all directions. The player can only move to a tile that is friendly.
    /// </summary>
    public void OnMove(Vector2 moveDir)
    {
        // safe guard from Controller Input hack  
        moveDir = Vector2.ClampMagnitude(moveDir, 1.0f); 

        Vector2Int gridMovement = new Vector2Int(  // accounting for Joystick OR Diagonal inputs
            Mathf.RoundToInt(moveDir.x),
            Mathf.RoundToInt(moveDir.y)
        );

        // used to check whether the square tile is friendly
        int newPositionX = positionX + gridMovement.x;
        int newPositionY = positionY + gridMovement.y;

        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(new Vector2(newPositionX, newPositionY));
        if (targetTile != null && targetTile.isFriendly)
        {
            //if (value.isPressed) 
            SetPlayerPosition(newPositionX, newPositionY);
        }
    }
}

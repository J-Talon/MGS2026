// Authors: [Jacky, Jeremy, Mark]
using Event;
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
        GameplayEvents.MovementInput.AddEventListener(OnMove);
    }

    private void OnDisable()
    {
        GameplayEvents.MovementInput.RemoveEventListener(OnMove);

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

    public void OnMove(Vector2 moveDir)
    {
        // safe guard from Controller Input hack  
        moveDir = Vector2.ClampMagnitude(moveDir, 1.0f); 

        Vector2Int gridMovement = new Vector2Int(  // accounting for Joystick/Diagonal inputs
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

    /// <summary>
    /// Handles player movement input for moving up on the grid. The player can only move to a tile that is friendly.
    /// </summary>
    public void OnUp(InputValue value)
    {
        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(new Vector2(positionX, positionY + 1));
        if (targetTile != null && targetTile.isFriendly)
        {
            if (value.isPressed) SetPlayerPosition(positionX, positionY + 1);
        }
    }

    /// <summary>
    /// Handles player movement input for moving down on the grid. The player can only move to a tile that is friendly.
    /// </summary>
    public void OnDown(InputValue value)
    {
        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(new Vector2(positionX, positionY - 1));
        if (targetTile != null && targetTile.isFriendly)
        {
            if (value.isPressed) SetPlayerPosition(positionX, positionY - 1);
        }
    }

    /// <summary>
    /// Handles player movement input for moving left on the grid. The player can only move to a tile that is friendly.
    /// </summary>
    public void OnLeft(InputValue value)
    {
        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(new Vector2(positionX - 1, positionY));
        if (targetTile != null && targetTile.isFriendly)
        {
            if (value.isPressed) SetPlayerPosition(positionX - 1, positionY);
        }
    }

    /// <summary>
    /// Handles player movement input for moving right on the grid. The player can only move to a tile that is friendly.
    /// </summary>
    public void OnRight(InputValue value)
    {
        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(new Vector2(positionX + 1, positionY));
        if (targetTile != null && targetTile.isFriendly)
        {
            if (value.isPressed) SetPlayerPosition(positionX + 1, positionY);
        }
    }
}

// Authors: [Jacky, Jeremy, Mark]

using Particle.Base;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script manages the player controller, handling player input and movement within the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Player position variables
    private int positionX = 0;
    private int positionY = 0;

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

        ParticleType.SPARK.Play(x, y, -1);
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
        ParticleManager.GetInstance().ClearAllParticles();
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

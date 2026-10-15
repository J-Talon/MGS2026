// Authors: [Jacky, Jeremy, Mark]
//edited by - Jordan October 14, 2026
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

    //added by Jordan - the 3 bottom variables (for player movement time related stuff)

    //variables that deal with how long it takes until you can move again to the next tile (to prevent near teleporting/mashing)
    private float movementCooldown = 0.15f; //time needed before a next movement input can be done (prevents crazy teleporting, and helps with some multiple controls buffering/mashing)
    private float movementCooldownTimer = 0f; //a timer that actually keeps track of the remaining time before the next movement (aka, a timer that gets compared to the above variable)

    //holds the direction for a buffered movement (if it exists).
    //aka, if during the movement cooldown the player holds another direction, execute it as soon as the cooldown ends
    private Vector2Int? bufferedMovement; 

    //subscribe the events
    public void OnEnable()
    {
        GameplayEvents.Input.movePressed.AddEventListener(OnMovePressed);
        GameplayEvents.Input.moveReleased.AddEventListener(OnMoveReleased);
    }

    //unsubscribe the events
    private void OnDisable()
    {
        GameplayEvents.Input.movePressed.RemoveEventListener(OnMovePressed);
        GameplayEvents.Input.moveReleased.RemoveEventListener(OnMoveReleased);
        bufferedMovement = null;
    }

    //unsubscribe the events
    private void OnDestroy()
    {
        GameplayEvents.Input.movePressed.RemoveEventListener(OnMovePressed);
        GameplayEvents.Input.moveReleased.RemoveEventListener(OnMoveReleased);
        bufferedMovement = null;
    }

    //added by Jordan
    private void Update()
    {
        //checks the movementCooldownTimer. If the countdown is greater than 0, continue counting down
        if (movementCooldownTimer > 0f)
        {
            movementCooldownTimer = movementCooldownTimer - Time.deltaTime;
        }

        //if the countdown is 0 or less, the players buffered movement direction can be executed
        if (movementCooldownTimer <= 0f && bufferedMovement.HasValue)
        {
            Vector2Int nextMovement = bufferedMovement.Value;
            bufferedMovement = null; //reset buffer for any potential future buffering of movements

            OnMovePressed(nextMovement);
        }
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
    public void OnMovePressed(Vector2Int moveDir)
    {
        //movementCooldownTimer and buffer stuff added by Jordan

        //player inputs a buffered movement (aka pressing and holding a movement during movement cooldown)
        //exits the function after setting the buffer direction as the player cant move yet
        if (movementCooldownTimer > 0f)
        {
            bufferedMovement = moveDir;
            return;
        }

        // used to check whether the square tile is friendly
        int newPositionX = positionX + moveDir.x;
        int newPositionY = positionY + moveDir.y;

        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(new Vector2Int(newPositionX, newPositionY));
        if (targetTile != null && targetTile.isFriendly)
        {
            SetPlayerPosition(newPositionX, newPositionY);
            movementCooldownTimer = movementCooldown; //reset movementCooldownTimer back to full as the player has now just moved
        }
    }

    //added by Jordan
    //if the player is buffering a movement during cooldown, but lets go of it before the movementCooldownTimer finishes, cancel the future movement
    private void OnMoveReleased(Vector2Int direction)
    {
        if (bufferedMovement == direction)
        {
            bufferedMovement = null;
        }
    }
}

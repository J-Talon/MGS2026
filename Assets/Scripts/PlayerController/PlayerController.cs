// Authors: [Jacky, Jeremy, Mark]
// edited by - Jordan October 14, 2026
// Edit by Jeremy October 15, 2026
using Event;

using Particle.Base;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// This script manages the player controller, handling player input and movement within the game.
/// </summary>
[RequireComponent(typeof(EntityUnit))]
public class PlayerController : MonoBehaviour
{
    protected EntityUnit parentUnit;
    private float movementCooldownTimer = 0f; //a timer that actually keeps track of the remaining time before the next movement

    void Start()
    {
        parentUnit = GetComponent<EntityUnit>();
    }

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
            movementCooldownTimer -= Time.deltaTime;
        }

        // if the countdown is 0 or less, the players buffered movement direction can be executed
        if (movementCooldownTimer <= 0f && bufferedMovement.HasValue)
        {
            Vector2Int nextMovement = bufferedMovement.Value;
            bufferedMovement = null; //reset buffer for any potential future buffering of movements
            OnMovePressed(nextMovement);
        }
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
        Vector2Int currentPosition = Vector2Int.RoundToInt(parentUnit.GetPosition());
        Vector2Int newPosition = currentPosition + moveDir;

        IGridSystemView gridView = GridManager.instance;
        GridCell targetTile = gridView.GetTileAtPosition(newPosition);
        if (targetTile != null && targetTile.isFriendly)
        {
            movementCooldownTimer = parentUnit.GetMovementTickTimer();
            parentUnit.MoveUnitSnappy(newPosition - currentPosition); // Move the player to the new position
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

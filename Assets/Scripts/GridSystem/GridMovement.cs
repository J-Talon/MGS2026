/*
author: Kian Reilly 
github: kareilly
discrd: kuushi

This GridMovement component can be attached to any object
and will work out of the box. A grid object that doesn't move
can also use the component to anchor/register itself without
issue. 

The movement is independent of any specific input system, so
more preliminary work needs to be done by a given object before
calling TryMove(), but a player and enemy object can use the same
unmodified GridMovement component with no issue.
*/
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = -1f; // negative => instant
    [SerializeField] private bool isEnemy = false; // if this entity is not friendly
    private Vector2Int currentTile;
    private Vector2 currentTarget;
    private Vector2Int moveBuffer;
    private bool isMoving = false;

    private void Start()
    {
        // snaps to tile on start
        currentTile = GridManager.Instance.PosToCoord(transform.position);
        transform.position = GridManager.Instance.CoordToPos(currentTile);

        GridManager.Instance.SetTileAvailable(currentTile, false);

    }


    private void Update()
    {
        var targetWorldPosition = new Vector3(currentTarget.x,currentTarget.y,transform.position.z);

        if (!isMoving)
        {
            // any future idle logic here
            return;
        }

        if(moveSpeed < 0f) // instant move on negative movespeed    
        {                  // this should ideally be handled in TryMove() to avoid delay

            transform.position = targetWorldPosition;
            isMoving = false;
        }
        else
        {
            if (transform.position != targetWorldPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);   
            }
            else
            {
                OnMoveFinished();
            }
        }
    }

    // attempt to move entity
    public void TryMove(Vector2Int move)
    {
        Vector2Int targetTile = currentTile + move;

        // checks if this entity owns the tile (player/enemy alligned)
        bool isPlayerOwned = GridManager.Instance.IsTileOwned(targetTile);
        bool isOwned = (isPlayerOwned && !isEnemy) || (!isPlayerOwned && isEnemy); // XOR condition with isEnemy

        if (!isOwned) // dont make the move if the tile isnt owned
        {
            OnMoveFailed();
            return;
        }

        // check if the tile is occupied
        bool isAvailable = GridManager.Instance.IsTileAvailable(targetTile);

        if (!isAvailable)
        {
            OnMoveFailed();
            return;
        }

        // trying to move while moving loads the move into the buffer
        // the buffer will execute another TryMove when the current move ends
        // buffer size is one, so multiple buffered inputs override themselves
        if (isMoving)
        {
            moveBuffer = move;
            return;
        }

        // gets the position of the tile being moved to
        Vector2 tilePosition = GridManager.Instance.GetTilePosition(targetTile);

        isMoving = true;
        OnMoveStart(tilePosition);
    }

    private void OnMoveStart(Vector2 targetPosition)
    {    
        currentTarget = targetPosition;
        Vector2Int targetTile = GridManager.Instance.PosToCoord(targetPosition);

        // update tile availability
        // target tile must be set as unavailable so that two objects can't move to the same tile
        GridManager.Instance.SetTileAvailable(currentTile);
        GridManager.Instance.SetTileAvailable(targetTile, false);
    }

    private void OnMoveFinished()
    {
        isMoving = false;
        currentTarget = Vector2.zero;

        // update current tile when reached
        currentTile = GridManager.Instance.PosToCoord(transform.position);

        // call the move in the move buffer
        if (moveBuffer != Vector2Int.zero)
        {
            TryMove(moveBuffer);
            moveBuffer = Vector2Int.zero;
        }
    } 
    // currently empty, but use this to emit an event for when a move fails
    // will be useful for animations/interactions and player input feedback
    private void OnMoveFailed()
    {
    }
}
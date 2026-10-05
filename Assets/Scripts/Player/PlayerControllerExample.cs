/*
author: Kian Reilly 
github: kareilly
discrd: kuushi

This is entirely a placeholder, the implementation of the input system
here is very questionable. I think the feel and result of the input recognition
is pretty great all things considered but this would still need to be rewritten.
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GridMovement _gridMovement; // reference to movement component

    private Vector2Int lastMoveDir;
    private Vector2 lastMovement;

    // set up grid movement component
    public void Start()
    {
        _gridMovement = GetComponent<GridMovement>();

        if (_gridMovement == null)
        {
            _gridMovement = gameObject.AddComponent<GridMovement>();
        }
    }
    public void OnMove(InputValue value)
    {
        // movement (float vec) is converted to normalized move direction (int vec)
        Vector2 movement = value.Get<Vector2>();
        Vector2Int moveDirection = Vector2Int.zero;

        // this block handles discarding release inputs
        if (movement == Vector2.zero) // dont move on null direction input
        {
            return;
        }
        else if (lastMovement.x != 0 && lastMovement.y != 0) // dont move when the input is a key release
        {

            if (movement.x == 0 || movement.y == 0)
            {
                lastMovement = movement;
                return;     
            }

        }

        // cases for diagonal input, determining dominant axis
        bool compareX = Mathf.Abs(movement.x) > Mathf.Abs(movement.y);
        bool compareY = Mathf.Abs(movement.y) > Mathf.Abs(movement.x);
        
        if (Mathf.Abs(movement.x) == Mathf.Abs(movement.y)) // handle true diagonal input
        {
            compareY = Mathf.Abs(lastMoveDir.x) > Mathf.Abs(movement.x); // if last was pure x, y next
            compareX = Mathf.Abs(lastMoveDir.y) > Mathf.Abs(movement.y); // if last was pure y, x next
        }

        if (compareX)
        {
            moveDirection = new Vector2Int(movement.x > 0 ? 1 : -1, 0); // pick 1 if x > 0 : else pick -1 
        }
        else if (compareY)
        {
            moveDirection = new Vector2Int(0, movement.y > 0 ? 1 : -1); // pick 1 if y > 0 : else pick -1 
        }
        else // case if no compare conditions not met, just discards for now
        {
            return;
        }

        if (_gridMovement != null && moveDirection != Vector2Int.zero)
        {
            //print($"\nlast: " + lastMoveDir.ToString() + $", this: " + moveDirection.ToString());
            _gridMovement.TryMove(moveDirection); 
        }

        // used for determining diagonal/release cases
        lastMoveDir = moveDirection;
        lastMovement = movement;
    }
}
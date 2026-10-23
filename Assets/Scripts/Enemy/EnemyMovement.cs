/// Author: Angad 
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Base enemy movement, randomly generated depending on valid directions 
/// </summary>
[RequireComponent(typeof(EntityUnit))]
public class EnemyMovement : MonoBehaviour
{
    protected EntityUnit parentUnit;

    private float movementCooldownTimer = 0f; //a timer that actually keeps track of the remaining time before the next movement

    private List<Vector2Int> totalDirections = new List<Vector2Int>(); // holds the directions an enemy can move with all free spaces
    private List<Vector2Int> validDirections = new List<Vector2Int>(); // holds the directions an enemy can move freely

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentUnit = GetComponent<EntityUnit>();

        totalDirections.Add(Vector2Int.up);
        totalDirections.Add(Vector2Int.right);
        totalDirections.Add(Vector2Int.down);
        totalDirections.Add(Vector2Int.left);
    }

    // Update is called once per frame
    void Update()
    {
        //checks the movementCooldownTimer. If the countdown is greater than 0, continue counting down
        if (movementCooldownTimer > 0f)
        {
            movementCooldownTimer -= Time.deltaTime;
        }
        else
        {
            // validate where the enemy can actually go
            validDirections = ValidateDirections(totalDirections);

            // perform the random move given the valid moves
            RandomMove(validDirections);
        }
    }

    // validates which directions the enemy can go based on all directions
    public List<Vector2Int> ValidateDirections(List<Vector2Int> allDirections)
    {
        List<Vector2Int> validatedDirections = new List<Vector2Int>();

        Vector2Int currentPosition = Vector2Int.RoundToInt(parentUnit.GetPosition());

        // checks which directions is valid
        foreach(Vector2Int dir in allDirections)
        {
            Vector2Int newPosition = currentPosition + dir;   

            IGridSystemView gridView = GridManager.instance;
            GridCell targetTile = gridView.GetTileAtPosition(newPosition);
                 
            if (targetTile != null && !targetTile.isFriendly && !targetTile.isOccupied)
            {
                validatedDirections.Add(dir);
                // Debug.Log("Valid move: " + dir.dir);
            }
        }
        return validatedDirections;
    }

    /// Performs the random move by selecting randomly from a list of valid 
    public void RandomMove(List<Vector2Int> validatedDirections)
    {
        // foreach (Vector2Int dir in validatedDirections)
        // {
        //     Debug.Log("Valid move: " + dir.dir);
        // }

        Vector2Int moveDir = validatedDirections[Random.Range(0,validatedDirections.Count)];
        // Debug.Log("Chosen Direction: " + moveDir);
        Vector2Int currentPosition = Vector2Int.RoundToInt(parentUnit.GetPosition());
        Vector2Int newPosition = currentPosition + moveDir;   

        // reset cooldown
        movementCooldownTimer = parentUnit.GetMovementTickTimer();
        parentUnit.MoveUnitSnappy(newPosition - currentPosition); // Move the enemy to the new position      

        // Debug.Log("moved");                                                     
    }
}

// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Base projectile class. Will make use of composition for specific behaviours
/// </summary>
public class EntityProjectile : Entity, IProjectileMove
{

    public void MoveProjectile(Vector2 deltaMovement)
    {
        gameObject.transform.position = gameObject.transform.position + (Vector3) deltaMovement;
        //call validation method here or smth
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

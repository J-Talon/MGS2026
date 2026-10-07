// Authors: [Jacky, Jeremy, Mark]
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class ProjectileMove : MonoBehaviour
{
    /// <summary>
    /// reference for controlling the projectile
    /// </summary>
    EntityProjectile parentProjectile;

    #region projectile instance data

    /// <summary>
    /// 
    /// </summary>
    private float timeLeft;

    /// <summary>
    /// General 
    /// </summary>
    public bool isFreindly;

    #endregion

    /// <summary>
    /// Setup internal cooldown tracker and interface getting
    /// </summary>
    void Start()
    {
        parentProjectile = GetComponent<EntityProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        movementRoutine();        
    }

    /// <summary>
    /// W
    /// </summary>
    void movementRoutine()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = parentProjectile.movementTickTimer;

            Vector2 deltaVector = new Vector2(0,0);
            if (isFreindly == true)
            {
                deltaVector.x += 1;
            }
            else
            {
                deltaVector.x -= 1;
            }

            parentProjectile.MoveProjectile(deltaVector); //move projectile by 1 left or right depending on fired entity unity 
        } 
    }
}

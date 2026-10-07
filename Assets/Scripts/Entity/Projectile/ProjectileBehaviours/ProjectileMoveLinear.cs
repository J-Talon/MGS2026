using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileMoveLinear : MonoBehaviour
{
    /// <summary>
    /// reference interface for controlling the projectile
    /// </summary>
    IProjectileMove parentProjectile;

    /// <summary>
    /// The generic data for this projectile
    /// </summary>
    ProjectileData parentProjectileData;

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
        parentProjectile = GetComponent<IProjectileMove>();
        timeLeft = parentProjectileData.movementTickTimer;
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
            timeLeft = parentProjectileData.movementTickTimer;;

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

// Authors: [Jacky, Jeremy, Mark]

using Entities.Abstract;
using Entities.Collectable;
using Entities.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   // public Transform PlayerPrefab;
    // private EntityUnit unit;
    // private EntityUnit demonUnit;
    // private CollectableEntity collectable;
    
    private EntityManager entityManager;


    void Start()
    {
        entityManager = EntityManager.GetInstance();
        entityManager.GetEntity(EntityType.Player, new Vector3(0,0,-1), true);
        
        entityManager.GetEntity(EntityType.UnitTest, new Vector3(5, 2, -1), false, 0f);
        entityManager.GetEntity(EntityType.UnitTestDemon, new Vector3(4, 0, -1), false, 0f);
        entityManager.GetEntity(EntityType.CollectableTest, new Vector3(3, 3, -1), true, 0f);
    }

    // void Update()
    // {
    //     if (Keyboard.current.yKey.wasReleasedThisFrame) //remove row on the bottom
    //     {
    //         unit.TakeDamage(25f);
    //     }
    // }
}

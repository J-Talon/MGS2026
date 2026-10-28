// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform PlayerPrefab;
    private EntityUnit unit;
    private EntityUnit demonUnit;
    private CollectableEntity collectable;

    public void SpawnPlayer(Vector3 position)
    {
        Instantiate(PlayerPrefab, position, Quaternion.identity);
    }

    void Start()
    {
        // Spawn the player at the origin with a z-position of -1 to ensure it is in front of the grid
        SpawnPlayer(new Vector3(0, 0, -1));
        unit = (EntityUnit)EntityManager.GetInstance().GetEntity(EntityType.UnitTest, new Vector3(5, 2, -1), false, 0f);
        demonUnit = (EntityUnit)EntityManager.GetInstance().GetEntity(EntityType.UnitTestDemon, new Vector3(4, 0, -1), false, 0f);
        collectable = (CollectableEntity)EntityManager.GetInstance().GetEntity(EntityType.CollectableTest, new Vector3(3, 3, -1), true, 0f);
    }

    void Update()
    {
        if (Keyboard.current.yKey.wasReleasedThisFrame) //remove row on the bottom
        {
            unit.TakeDamage(25f);
        }
    }
}

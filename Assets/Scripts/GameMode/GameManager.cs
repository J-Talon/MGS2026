// Authors: [Jacky, Jeremy, Mark]

using Entities.Abstract;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private EntityManager entityManager;

    void Start()
    {
        entityManager = EntityManager.GetInstance();
        entityManager.GetEntity(EntityType.Player, new Vector3(0, 0, -1), true);

        // entityManager.GetEntity(EntityType.UnitTest, new Vector3(5, 2, -1), false, 0f);
        // entityManager.GetEntity(EntityType.UnitTestDemon, new Vector3(4, 0, -1), false, 0f);
        // entityManager.GetEntity(EntityType.CollectableTest, new Vector3(3, 3, -1), true, 0f);

        // AI moveset test
        // entityManager.GetEntity(EntityType.UnitTestDodgeRow, new Vector3(5, 2, -1));
        // entityManager.GetEntity(EntityType.UnitTestDodgeCol, new Vector3(5, 1, -1));
        // entityManager.GetEntity(EntityType.UnitTestDodgeAll, new Vector3(5, 3, -1));
        // entityManager.GetEntity(EntityType.UnitTestMoveInward, new Vector3(5, 4, -1));

        // Space out test
        entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(6, 4, -1));
        entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(6, 2, -1));
        entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(7, 2, -1));
        entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(8, 0, -1));
        entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(9, 3, -1));
        entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(9, 4, -1));

        // Test Unit with all behaviours
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(6, 4, -1));
    }
}

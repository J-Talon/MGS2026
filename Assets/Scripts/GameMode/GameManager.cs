// Authors: [Jacky, Jeremy, Mark]

using Entities.Abstract;
using UnityEngine;
using Gameplay.Core;
using Gameplay.Wave; // remove this when level are implemented.

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private EntityManager entityManager;

    // delete when level is implemented
    WaveQueue myWave;

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
        // entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(6, 4, -1));
        // entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(6, 2, -1));
        // entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(7, 2, -1));
        // entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(8, 0, -1));
        // entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(9, 3, -1));
        // entityManager.GetEntity(EntityType.UnitTestMoveSpaceOut, new Vector3(9, 4, -1));

        // Test Unit with all behaviours
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(5, 2, -1));
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(5, 3, -1));
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(6, 4, -1));
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(6, 1, -1));
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(7, 1, -1));
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(8, 1, -1));
        // entityManager.GetEntity(EntityType.UnitTestBuddha, new Vector3(9, 1, -1));

        // Test waves (THIS SHOULD ALL BE MANAGED BY A LEVEL MANAGER!!!)
        WaveManager waveManager = new WaveManager();

        tokenedEntity UnitTestDodgeRow = new tokenedEntity(EntityType.UnitTestDodgeRow, 1, 10);
        tokenedEntity UnitTestDodgeCol = new tokenedEntity(EntityType.UnitTestDodgeCol, 1, 10);
        tokenedEntity UnitTestDodgeAll = new tokenedEntity(EntityType.UnitTestDodgeAll, 1, 10);
        tokenedEntity UnitTestMoveInward = new tokenedEntity(EntityType.UnitTestMoveInward, 1, 10);
        tokenedEntity UnitTestBuddha = new tokenedEntity(EntityType.UnitTestBuddha, 1, 10);

        WeightedRandomEntity weightedEntities = new WeightedRandomEntity();
        weightedEntities.Add(UnitTestDodgeRow);
        weightedEntities.Add(UnitTestDodgeCol);
        weightedEntities.Add(UnitTestDodgeAll);
        weightedEntities.Add(UnitTestMoveInward);
        weightedEntities.Add(UnitTestBuddha);

        myWave = new WaveQueue(weightedEntities, 5);

        myWave.getWaveEndedEvent().AddListener(onWaveEnded);

        // This is kinda pointless but itll make sense with more than one wave
        waveManager.addWave(myWave);
        waveManager.getNextWave().Play();
    }

    // TESTING I will delete this when I or someone else implements levels
    private void onWaveEnded()
    {
        Debug.Log("Wave is over");
    }

    // TESTING I will delete this when I or someone else implements levels
    private void Update()
    {
        myWave.FixedUpdate();
    }
}

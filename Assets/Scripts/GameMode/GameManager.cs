// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform PlayerPrefab;

    public void SpawnPlayer(Vector3 position)
    {
        Instantiate(PlayerPrefab, position, Quaternion.identity);
    }

    void Start()
    {
        // Spawn the player at the origin with a z-position of -1 to ensure it is in front of the grid
        SpawnPlayer(new Vector3(0, 0, -1));
        EntityUnit unit = (EntityUnit) EntityManager.GetInstance().GetEntity(EntityType.UnitTest, new Vector3(5, 2, -1), false, 0f);
    }
}

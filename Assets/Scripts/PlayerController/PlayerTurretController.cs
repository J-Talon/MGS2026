// Authors: [Jacky]
using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Event;
using Unity.VisualScripting;

/// <summary>
/// This script manages player turret spawning
/// currently just a behaviour kind of script (no management) and can be plug and played
/// on top of player controller
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerTurretController : MonoBehaviour
{
    protected EntityUnit parentUnit;

    void Start()
    {
        parentUnit = GetComponent<EntityUnit>();
    }


    //subscribe the events
    public void OnEnable()
    {
        GameplayEvents.Input.spawnTurretPressed.AddEventListener(OnSpawnTurret);
    }

    //unsubscribe the events
    private void OnDisable()
    {
        GameplayEvents.Input.spawnTurretPressed.RemoveEventListener(OnSpawnTurret);
    }

    /// <summary>
    /// Perform check of turret spawn
    /// 
    /// 1. check the 1 grid cell in front of player, Conditions:
    ///     must not be enemy territory
    ///     must be a valid tile (i.e not outside of map boundaries)
    ///     must be not be occupied by another 'unit' (or another turret)
    /// 2. use entity factory to spawn a 'turret' entity unit
    /// 
    /// </summary>
    /// <param name="_"></param>
    private void OnSpawnTurret(ValueTuple _)
    {
        //print("SPAWNING");
        IGridSystemView gridView = GridManager.instance;
        Vector2Int frontOfPlayer = Vector2Int.RoundToInt(parentUnit.transform.position) + Vector2Int.right;
        GridCell targetTile = gridView.GetTileAtPosition(frontOfPlayer);
        if (targetTile != null)
        {
            if (targetTile.isOccupied == false && targetTile.isFriendly == true)
            {
                EntityUnit spawnedTurret = (EntityUnit)EntityManager.GetInstance().GetEntity(EntityType.UnitTestTurret, new Vector3(frontOfPlayer.x, frontOfPlayer.y, -1), true, 0f);
            }
            else
            {
                print("no spawn allowed");
            }
        }
    }

}
using UnityEngine;

// A collectable entity.
public class CollectableEntity : Entity
{
    // references
    protected ICollectableOnInteract behaviour;
    protected IGridSystemView gridManager = GridManager.instance;   
    protected GridCell cell;

    new void Start()
    {
        base.Start();
        CollectableSetup();
    }

    void CollectableSetup()
    {
        cell = gridManager.GetTileAtPosition(Vector2Int.RoundToInt(gameObject.transform.position));

        if (cell == null)
        {
            // probably should just destroy itself
            // Destroy(gameObject);
            Despawn();
            return;
        }

        // occupy cell
        if (cell.occupiedCollectable == null)
        {
            cell.CollectableOccupy(this);
        } 
        else
        {
            Despawn();
            return;
        }

        // set up behaviour with fallback
        behaviour = gameObject.GetComponent<ICollectableOnInteract>();
        if (behaviour == null)
        {
            behaviour = gameObject.AddComponent<CollectableSimple>();
        }
    }

    // Call the behaviour function and despawns itself
    void OnCollect()
    {
        behaviour.OnCollect();
        cell.CollectableDespawn();
        Despawn();
    }

    public void OnInteract(Entity entity)
    {
        // TODO: Change this when we have a player entity ID or type!
        // Right now the player entity is created first so it will always have an id of 1. This may not be the case in the future.
        int playerID = 1;
        if (entity.GetEntityID() == playerID)
        {
            OnCollect();
        }
        else
        {
            behaviour.OnInteract(entity);
        }
    }

    // Update is called once per frame
    public override void Tick() { }
}
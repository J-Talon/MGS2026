using Entities.Abstract;
using Entities.Collectable.Behaviour;
using Entities.Collectable.Interfaces;
using Entities.Unit;
using UnityEngine;

// A collectable entity.
namespace Entities.Collectable
{
    public class CollectableEntity : Entity
    {
        // references
        //by the way this really isn't necessary, I may refactor this in the future
        protected ICollectableOnInteract behaviour;   
        protected IGridSystemView gridManager = GridManager.instance;   
        protected GridCell cell;

        public override void OnStart()
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
        void OnCollect(EntityUnit player)
        {
            behaviour.OnCollect(player);
            cell.CollectableDespawn();
            Despawn();
        }

        public void OnInteract(Entity entity)
        {
            
            
            if (EntityType.Player.Equals(entity.GetEntityType()))
            {
                OnCollect((EntityUnit)entity);
            }
            else
            {
                behaviour.OnInteract(entity);
            }
        }

        // Update is called once per frame
        public override void Tick() { }
    }
}
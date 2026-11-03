using Entities.Abstract;
using Entities.Collectable.Interfaces;
using Entities.Unit;
using UnityEngine;

// simple collectable, removes itself and does nothing when touched.
namespace Entities.Collectable.Behaviour
{
    public class CollectableSimple: MonoBehaviour, ICollectableOnInteract
    {
        public void OnCollect(EntityUnit player)
        {
            // nothing
        }

        public void OnInteract(Entity entity)
        {
            // nothing
        }
    }
}
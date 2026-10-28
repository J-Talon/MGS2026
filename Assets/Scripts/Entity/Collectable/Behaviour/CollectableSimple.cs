using UnityEngine;

// simple collectable, removes itself and does nothing when touched.
public class CollectableSimple: MonoBehaviour, ICollectableOnInteract
{
    public void OnCollect()
    {
        // nothing
    }

    public void OnInteract(Entity entity)
    {
        // nothing
    }
}
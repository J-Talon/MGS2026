using UnityEngine;

// To be implemented by collectable behaviours.
public interface ICollectableOnInteract
{
    // Touched by player entity
    public void OnCollect();

    // Interaction between other entities
    public void OnInteract(Entity entity);
}
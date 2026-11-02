using UnityEngine;

// To be implemented by collectable behaviours.
public interface ICollectableOnInteract
{
    /// <summary>
    /// This function should be called when the player interacts or collects the collectable.
    /// Implementation of this function can include: changing player stats, giving the player an effect, etc.
    /// </summary>
    public void OnCollect();

    /// <summary>
    /// This function should be called when an entity interacts with the collectable.
    /// If needed, the collectable can be interacted with by entities other than the player.
    /// Ex: A projectile destroys a collectable, an enemy steals a collectable.
    /// </summary>
    public void OnInteract(Entity entity);
}
using Entities.Abstract;
using Entities.Unit;

// To be implemented by collectable behaviours.
namespace Entities.Collectable.Interfaces
{
    public interface ICollectableOnInteract
    {
        /// <summary>
        /// This function should be called when the player interacts or collects the collectable.
        /// Implementation of this function can include: changing player stats, giving the player an effect, etc.
        /// </summary>
        /// <param name="player"></param>
        public void OnCollect(EntityUnit player);

        /// <summary>
        /// This function should be called when an entity interacts with the collectable.
        /// If needed, the collectable can be interacted with by entities other than the player.
        /// Ex: A projectile destroys a collectable, an enemy steals a collectable.
        ///
        /// Addendum 1 (Talon J)
        /// When a grid locked entity moves onto the grid cell that the collectable is on, if the
        /// entity is of type player, it will call OnCollect, else, it will call OnInteract
        /// We can probably condense the logic into 1 single method that is called instead of 2.
        /// 
        /// </summary>
        public void OnInteract(Entity entity);
    }
}
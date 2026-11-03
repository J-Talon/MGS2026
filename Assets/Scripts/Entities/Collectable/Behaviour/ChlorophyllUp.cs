using Entities.Abstract;
using Entities.Collectable.Interfaces;
using Entities.Unit;
using UnityEngine;

// very simple behaviour, when touched increase chlorophyll count by chloroGain
//good, but might be better using subclasses to define specific collectables
namespace Entities.Collectable.Behaviour
{
    public class ChlorophyllUp: MonoBehaviour, ICollectableOnInteract
    {
        [field: SerializeField] protected float chloroGain = 10;
        public void OnCollect(EntityUnit player)
        {
            // chloro up
            Debug.Log($"Chloro up {chloroGain}");
        }

        public void OnInteract(Entity entity)
        {
            // do nothing
        }
    }
}
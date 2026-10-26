using UnityEngine;

// very simple behaviour, when touched increase chlorophyll count by chloroGain
public class ChlorophyllUp: MonoBehaviour, ICollectableOnInteract
{
    [field: SerializeField] protected float chloroGain = 10;
    public void OnCollect()
    {
        // chloro up
        Debug.Log($"Chloro up {chloroGain}");
    }

    public void OnInteract(Entity entity)
    {
        // do nothing
    }
}
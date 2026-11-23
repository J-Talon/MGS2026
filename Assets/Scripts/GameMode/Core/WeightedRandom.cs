// Author - Wilson -> Nov 14 2026

using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.Core
{
    // Taken from Talon's java code in W7 Async task - Level Logic
    // All this does is create a pool of weighted items and randomly pull one based on its weight
    // Added:
    // - Add
    // - getNextItem
    public class WeightedRandom<T,R> where R : WeightedItem<T>
    {
        private readonly List<R> items;
        private double totalWeight = 0;
        public int Count() {return items.Count;}
        public IReadOnlyList<R> GetItems() {return items;}

        public WeightedRandom(List<R> items = null)
        {
            this.items = items ?? new();
        }

        // Add into the weighted random
        public void Add(R item)
        {
            if (item == null) {return;}

            items.Add(item);
            totalWeight += item.GetWeight();

        }

        public R GetNext(){
            if (items.Count == 0) {return default;}

            double weight = totalWeight;

            for (int slot=0;slot<items.Count;slot++)
            {
                double itemWeight = items[slot].GetWeight();
                if (itemWeight <= 0)
                    continue;

                // If the roll succeeds or this is the last item, return it
                if (slot == items.Count - 1 || Random.Range(0f, 1f) * weight < itemWeight)
                {
                    return items[slot];
                }

                weight -= itemWeight;
            }
            return default;
        }

        public T GetNextItem(){
            if (items.Count == 0) {return default;}

            double weight = totalWeight;

            for (int slot=0;slot<items.Count;slot++)
            {
                double itemWeight = items[slot].GetWeight();
                if (itemWeight <= 0)
                    continue;

                // If the roll succeeds or this is the last item, return it
                if (slot == items.Count - 1 || Random.Range(0f, 1f) * weight < itemWeight)
                {
                    return items[slot].GetItem();
                }

                weight -= itemWeight;
            }
            return default;
        }
    }

    public class WeightedItem<T>
    {
        private readonly T item;
        private double weight;

        public WeightedItem(T item, double weight){
            this.item = item;
            SetWeight(weight);
        }

        public T GetItem() {
            return item;
        }

        public double GetWeight() {
            return weight;
        }

        public void SetWeight(double weight){
            if (weight < 0) {weight = 0;} // if someone puts negative weight.
            this.weight = weight;
        }

    }
}
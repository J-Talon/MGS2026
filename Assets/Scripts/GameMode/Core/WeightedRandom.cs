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
    public class WeightedRandom<T>
    {
        private readonly List<WeightedItem<T>> items;
        private double totalWeight = 0;
        public int Count() {return items.Count;}
        public IReadOnlyList<WeightedItem<T>> GetItems() {return items;}

        public WeightedRandom(List<WeightedItem<T>> items = null)
        {
            this.items = items ?? new();
        }

        // Add into the weighted random
        public void Add(WeightedItem<T> item)
        {
            if (item == null) {return;}

            items.Add(item);
            totalWeight += item.getWeight();

        }
        // overloaded because it will get annoying.
        public void Add(T item, double weight)
        {
            if (item == null) {return;}
            
            WeightedItem<T> i = new WeightedItem<T>(item, weight);
            Add(i);
        }

        public WeightedItem<T> getNext(){
            if (items.Count == 0) {return default;}

            double weight = totalWeight;

            for (int slot=0;slot<items.Count;slot++)
            {
                double itemWeight = items[slot].getWeight();
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

        public T getNextItem(){
            if (items.Count == 0) {return default;}

            double weight = totalWeight;

            for (int slot=0;slot<items.Count;slot++)
            {
                double itemWeight = items[slot].getWeight();
                if (itemWeight <= 0)
                    continue;

                // If the roll succeeds or this is the last item, return it
                if (slot == items.Count - 1 || Random.Range(0f, 1f) * weight < itemWeight)
                {
                    return items[slot].getItem();
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
            setWeight(weight);
        }

        public T getItem() {
            return item;
        }

        public double getWeight() {
            return weight;
        }

        public void setWeight(double weight){
            if (weight < 0) {weight = 0;} // if someone puts negative weight.
            this.weight = weight;
        }

    }
}
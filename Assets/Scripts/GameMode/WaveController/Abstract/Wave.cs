// Author - Wilson -> Nov 14 2026

using UnityEngine;
using Entities.Unit;
using Gameplay.Core;
using System.Collections.Generic;
using UnityEngine.Events;
using Entities.Abstract;

namespace Gameplay.Wave
{
    public abstract class Wave
    {
        // This list will be filled with entities to be spawned
        // Why is this not a queue??? Because game design may change how spawning works.
        protected List<EntityType> spawnList;
        protected List<Entity> activeEntities;
        protected int maxEntities;
        protected bool isPlaying = false; 
        // Tells any listener that the wave has ended.
        private UnityEvent waveEnded;
        public UnityEvent getWaveEndedEvent() {return waveEnded;}

        protected EntityManager entityManager;
        
        // Takes in a list of weighted entities to spawn, the tokens, and optionally change the maxEntities (default is 4 as described in the GDD)
        // A token system means that the wave is given a budget of X credits. 
        // Each enemy type that the wave could spawn is given a cost of Y credits. 
        // The wave will then use an algorithm to buy enemies to spawn into the game.
        // When the wave runs out of credits to buy enemies with, and all enemies of the wave have been killed, then the wave is then finished. 
        // IMPORTANT: in this case, token is the MAX TOTAL WEIGHT.
        public Wave(WeightedRandom<EntityType> weightedEntities, double tokens, int newMaxEntities = 4)
        {
            // set up!
            maxEntities = newMaxEntities;
            activeEntities = new();
            waveEnded = new UnityEvent();
            entityManager = EntityManager.GetInstance();

            // Build the wave

            // Get the smallest possible weight (we want to not waste tokens)
            double? smallestWeight = null;
            foreach (var entity in weightedEntities.GetItems())
            {
                double weight = entity.getWeight();
                if ((weight < smallestWeight || smallestWeight == null) && weight != 0)
                {
                    smallestWeight = weight;
                }
            }

            // Make sure we dont screw this up.
            if (smallestWeight == null || smallestWeight <= 0)
            {
                throw new System.Exception($"Failed to build wave");
            }
            
            // Stop when the remaining tokens cannot pull another entity
            double remainingTokens = tokens;
            while (remainingTokens <= smallestWeight)
            {
                WeightedItem<EntityType> entityRolled = weightedEntities.getNext();
                
                if (entityRolled == null)
                    break;

                if (entityRolled.getWeight() <= remainingTokens)
                {
                    spawnList.Add(entityRolled.getItem());
                    remainingTokens -= entityRolled.getWeight();
                }
            }
            // We can shuffle the spawnQueue if we wanted to.
        }

        // Plays the wave
        public void Play()
        {
            for (int i = 0; i < maxEntities; i++)
            {
                var next = getNextSpawn();
                if (next != null)
                {
                    activeEntities.Add(next);
                }
            }
            isPlaying = true;
        }

        // End the wave
        private void End()
        {
            isPlaying = false;
            waveEnded.Invoke();
            waveEnded.RemoveAllListeners();
        }

        // Clears the wave
        public void Clear()
        {
            // End the wave first so no more enemies spawn
            End();

            foreach (EntityUnit entity in activeEntities)
            {
                entity.Despawn();
            }

            activeEntities.Clear();
            spawnList.Clear();
        }

        // Gets the next entity to be spawned
        protected abstract Entity getNextSpawn();

        protected Entity spawnEntityAt(Vector3 postion, EntityType entity)
        {
            return entityManager.GetEntity(entity, postion);
        }

        public void Tick()
        {
            if (isPlaying){
                // Will change if I find a better way of knowing if an entity has perished or not.
                foreach (Entity entity in activeEntities)
                {
                    if (!entity.isActiveAndEnabled)
                    {
                        activeEntities.Remove(entity);
                        var next = getNextSpawn();
                        if (next != null)
                        {
                            activeEntities.Add(next);
                        }
                    }
                }
                
                if (activeEntities.Count == 0 && spawnList.Count == 0)
                {
                    End();
                }
            }
        }

        public void FixedUpdate()
        {
            this.Tick();
        }
    }
}

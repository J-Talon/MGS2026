// Author - Wilson -> Nov 14 2026
// Takes in a list of weighted entities to spawn, the tokens, and optionally change the maxEntities (default is 4 as described in the GDD)
// A token system means that the wave is given a budget of X credits. 
// Each enemy type that the wave could spawn is given a cost of Y credits. 
// The wave will then use an algorithm to buy enemies to spawn into the game.
// When the wave runs out of credits to buy enemies with, and all enemies of the wave have been killed, then the wave is then finished. 

using UnityEngine;
using Entities.Unit;
using Gameplay.Core;
using System.Collections.Generic;
using System.Linq;
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
        protected List<double> enemiesToSpawn = new();
        protected double spawnDelay;
        // Tells any listener that the wave has ended.
        private UnityEvent waveEnded;
        public UnityEvent getWaveEndedEvent() {return waveEnded;}

        protected EntityManager entityManager;
    
        public Wave(WeightedRandomEntity weightedEntities, int tokens, int newMaxEntities = 4, double spawnDelay = 0.5)
        {
            // set up!
            maxEntities = newMaxEntities;
            this.spawnDelay = spawnDelay; 
            activeEntities = new();
            spawnList = new();
            waveEnded = new UnityEvent();
            entityManager = EntityManager.GetInstance();

            if (tokens < 0) {tokens = 0;}

            // Build the wave (build the spawn list)

            // Get the smallest possible token cost (we want to not waste tokens)
            int? smallestCost = null;
            foreach (var entity in weightedEntities.GetItems())
            {
                tokenedEntity tokenedEntity = (tokenedEntity)entity;
                int cost = tokenedEntity.getToken();
                if (cost < smallestCost || smallestCost == null)
                {
                    smallestCost = cost;
                }
            }

            // Make sure we dont screw this up.
            if (smallestCost == null || smallestCost < 0)
            {
                throw new System.Exception($"Failed to build wave");
            }
            
            // Stop when the remaining tokens cannot pull another entity
            int remainingTokens = tokens;
            while (remainingTokens >= smallestCost)
            {
                // roll a random entity
                tokenedEntity entityRolled = (tokenedEntity)weightedEntities.getNext();
                
                // if somehow it returns null we gotta stop
                if (entityRolled == null)
                    break;

                var entityData = entityRolled.unwrap();
                // entity data consists of entity (the entity type) and token (the token cost)

                Debug.Log(entityData.token);

                if (entityData.token <= remainingTokens)
                {
                    // Add to the spawn list and remove the token cost from the remaining tokens
                    spawnList.Add(entityData.entity);
                    remainingTokens -= entityData.token;
                }
            }
            Debug.Log(spawnList.Count);
            // We can shuffle the spawnQueue if we wanted to.
        }

        // Plays the wave
        public void Play()
        {
            for (int i = 0; i < maxEntities; i++)
            {
                var next = getNextSpawn();
                if (next.type != null)
                {
                    activeEntities.Add(spawnEntityAt(next.pos, next.type));
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

        // Gets the next entity to be spawned and its location
        protected abstract (EntityType type, Vector2Int pos) getNextSpawn();

        protected Entity spawnEntityAt(Vector2Int postion, EntityType entity)
        {
            // may be nice to implement a spawn delay...
            Vector3 spawnPos = new Vector3(postion.x, postion.y, -1);
            return entityManager.GetEntity(entity, spawnPos);
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

                        enemiesToSpawn.Add(spawnDelay);
                    }
                }

                // bit of a crude way to implement spawn delays
                if (enemiesToSpawn.Count > 0)
                {
                    for (int i = enemiesToSpawn.Count - 1; i >= 0; i--)
                    {
                        if (enemiesToSpawn[i] <= 0f)
                        {
                            var next = getNextSpawn();
                            if (next.type != null)
                            {
                                activeEntities.Add(spawnEntityAt(next.pos, next.type));
                            }
                            enemiesToSpawn.RemoveAt(i);
                        }
                        else
                        {
                            enemiesToSpawn[i] -= Time.deltaTime;
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

    // WRAPPER
    // it would be quite annoying to write out a bunch of weighted items and add the token cost on top
    // wow this looks ugly, but it works
    public class tokenedEntity : WeightedItem<EntityType>
    {
        private EntityType entity;
        private int tokenCost;

        public EntityType getEntity() {return entity;}
        public int getToken() {return tokenCost;}
        public (EntityType entity, int token) unwrap() {return (entity, tokenCost);}

        public tokenedEntity(EntityType entity, int tokenCost, double weight) : base(entity, weight)
        {
            this.entity = entity;
            this.tokenCost = tokenCost;
        }
    }

    public class WeightedRandomEntity : WeightedRandom<EntityType>
    {
        public WeightedRandomEntity(List<tokenedEntity> items = null) : base(items?.Cast<WeightedItem<EntityType>>().ToList()) {}
    }
}

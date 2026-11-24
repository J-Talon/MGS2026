// Author - Wilson -> Nov 14 2026
// Takes in a list of weighted entities to spawn, the tokens, and optionally change the maxEntities (default is 4 as described in the GDD)
// A token system means that the wave is given a budget of X credits. 
// Each enemy type that the wave could spawn is given a cost of Y credits. 
// The wave will then use an algorithm to buy enemies to spawn into the game.
// When the wave runs out of credits to buy enemies with, and all enemies of the wave have been killed, then the wave is then finished. 

using UnityEngine;
using Entities.Unit;
using Gameplay.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Abstract;
using Event;

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
        protected EntityManager entityManager;
    
        public Wave(WeightedRandomEntity weightedEntities, int tokens, int maxEntities = 4, double spawnDelay = 0.5)
        {
            // set up!
            if (maxEntities < 1) {maxEntities = 1;}
            if (spawnDelay < 0) {spawnDelay = 0;}
            if (tokens < 0) {tokens = 0;}

            this.maxEntities = maxEntities;
            this.spawnDelay = spawnDelay; 
            activeEntities = new();
            spawnList = new();
            entityManager = EntityManager.GetInstance();

            // Build the wave (build the spawn list)

            // Get the smallest possible token cost (we want to not waste tokens)
            int? smallestCost = null;
            foreach (var entity in weightedEntities.GetItems())
            {
                TokenedEntity tokenedEntity = entity;
                int cost = tokenedEntity.GetToken();
                if (cost < smallestCost || smallestCost == null)
                {
                    smallestCost = cost;
                }
            }

            // Make sure we dont screw this up.
            if (smallestCost == null || smallestCost <= 0)
            {
                throw new System.Exception($"Failed to build wave");
            }
            
            // Stop when the remaining tokens cannot pull another entity
            int remainingTokens = tokens;
            while (remainingTokens >= smallestCost)
            {
                // roll a random entity
                TokenedEntity entityRolled = weightedEntities.GetNext();
                
                // if somehow it returns null we gotta stop
                if (entityRolled == null)
                    break;

                var entityData = entityRolled.Unwrap();
                // entity data consists of entity (the entity type) and token (the token cost)

                if (entityData.token <= remainingTokens)
                {
                    // Add to the spawn list and remove the token cost from the remaining tokens
                    spawnList.Add(entityData.entity);
                    remainingTokens -= entityData.token;
                }
            }
            // We can shuffle the spawnList if we wanted to.
        }

        // Plays the wave
        public void Play()
        {
            // initial spawns
            for (int i = 0; i < maxEntities; i++)
            {
                var next = GetNextSpawn();
                if (next.type != null && next.pos != null)
                {
                    activeEntities.Add(SpawnEntityAt(next.pos, next.type));
                }
            }
            isPlaying = true;
        }

        // End the wave
        private void End()
        {
            isPlaying = false;
            // Call the wave ended event
            GameplayEvents.Level.waveEnded.CallEvent(ValueTuple.Create());
        }

        // Clears the wave
        public void Clear()
        {
            // End the wave first so no more enemies spawn
            End();

            foreach (EntityUnit entity in activeEntities)
            {
                entity.Despawn(); // despawn or kill? change if the enemies need to die instead (grim...)
            }

            activeEntities.Clear();
            spawnList.Clear();
        }

        // Gets the next entity to be spawned and its location
        protected abstract (EntityType type, Vector2Int pos) GetNextSpawn();

        protected Entity SpawnEntityAt(Vector2Int postion, EntityType entity)
        {
            // TODO: Add modifiers to the entity here
            // ...GetEntity().AddModifier(); <- probably not going to look like this
            Vector3 spawnPos = new Vector3(postion.x, postion.y, -1);
            return entityManager.GetEntity(entity, spawnPos);
        }

        protected void OnEntityDeath(Entity entity)
        {
            activeEntities.Remove(entity);

            enemiesToSpawn.Add(spawnDelay);
        }

        public void Tick()
        {
            if (isPlaying){
                // Will change if I find a better way of knowing if an entity has perished or not.
                for (int i = activeEntities.Count - 1; i >= 0; i--)
                {
                    Entity entity = activeEntities[i];

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
                            var next = GetNextSpawn();
                            if (next.type != null && next.pos != null)
                            {
                                activeEntities.Add(SpawnEntityAt(next.pos, next.type));
                            }
                            enemiesToSpawn.RemoveAt(i);
                        }
                        else
                        {
                            enemiesToSpawn[i] -= Time.deltaTime;
                        }
                    }
                }
                
                // end if there are no entities left to spawn and no active entities
                if (activeEntities.Count == 0 && spawnList.Count == 0)
                {
                    End();
                }
                
            }
        }

        // Have something call this every tick please
        public void FixedUpdate()
        {
            this.Tick();
        }
    }

    // WRAPPER
    // it would be quite annoying to write out a bunch of weighted items and add the token cost on top
    // wow this looks ugly, but it works
    public class TokenedEntity : WeightedItem<EntityType>
    {
        private int tokenCost;

        public EntityType GetEntity() {return GetItem();}
        public int GetToken() {return tokenCost;}
        public (EntityType entity, int token) Unwrap() {return (GetEntity(), GetToken());}

        public TokenedEntity(EntityType entity, int tokenCost, double weight) : base(entity, weight)
        {
            if (tokenCost <= 0)
            {
                throw new Exception("Attempted to create a TokenedEntity with tokenCost <= 0 (Please rethink the token cost if you need it to be 0)");
            }
            this.tokenCost = tokenCost;
        }
    }
    // This one makes sure you dont mix and match types. thats a big no no
    public class WeightedRandomEntity : WeightedRandom<EntityType, TokenedEntity>
    {
        public WeightedRandomEntity(List<TokenedEntity> items = null) : base(items) {}
    }
}

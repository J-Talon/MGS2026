// Author - Wilson -> Nov 15 2026
// I am the wave manager. I manage the waves.

/*
Wave creating tutorial!!

How to create a wave:
1. Create your tokened entities!
    ex:
    TokenedEntity myTokenedEntity = new TokenedEntity(EntityType type, int tokenCost, double weight);
    - type is the type of entity you will be spawning
    - tokenCost is the token cost of the entity (you can read about tokens in the GDD or in Wave.cs)
    - weight is the chance the entity will be picked into the list of entities to spawn (chance is weight/TOTAL WEIGHT - the total weight is calculated from the sum of all weights added to a wave)
2. Add all the tokened entities to a randomizer!
    ex:
    WeightedRandomEntity weightedEntities = new WeightedRandomEntity();
    weightedEntities.Add(myTokenedEntity1);
    weightedEntities.Add(myTokenedEntity2);
    weightedEntities.Add(myTokenedEntity3);

    add as many tokened entities as you'd like
3. Create your wave!
    I created a simple wave for you guys to use, you can change it or create your own
    ex:
        WaveQueue myWave = new WaveQueue(WeightedRandomEntity weightedEntities, int maxTokens, int maxEntities, double spawnDelay);
        - weightedEntities is the randomizer we created in the last step
        - maxTokens is the maximum amount of tokens the wave can "spend" (you can read about tokens in the GDD or in Wave.cs)
        - maxEntities is the maximum entities that can be on the grid at once (default is 4)
        - spawnDelay is the time it takes for a new entity to spawn after an entity is despawned
4. Play the wave!
    ex:
    myWave.Play(); - simple!

    myWave.fixedUpdate() must be called every tick, handle it in a manager.

    you can get an event that tells you when the wave has ended using:
    GameplayEvents.Level.waveEnded.AddEventListener(MY_FUNCTION);
*/

using UnityEngine;
using Gameplay.Core;
using Entities.Abstract;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Gameplay.Wave
{
    public class WaveManager
    {
        private Queue<Wave> waves;
        public WaveManager(Queue<Wave> waves = null)
        {
            this.waves = waves ?? new();
        }

        public void AddWave(Wave wave)
        {
            waves.Enqueue(wave);
        }

        public Wave GetNextWave()
        {
            return waves.Dequeue();
        }

        // just in case.
        public void ClearWaves()
        {
            waves.Clear();
        }
    }
}
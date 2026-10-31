// Author: Jeremy
using System.Collections.Generic;
using UnityEngine;
using Particle.Base;

/*
HOW TO USE EXPLOSION VFX
Pass any of these as the vfx of an explosion:
1) A preset from the bottom of this class
    new Explosion(pattern, center, ExplosionVFX.SPARK_CHAIN, mutator, damage).Detonate();
2) A single particle, which plays on PostMutate
    new Explosion(pattern, center, ParticleType.SPARK, mutator, damage).Detonate();
3) A custom sequence. If you reuse it, add it as a preset instead
    var vfx = new ExplosionVFX()
        .Pre(ParticleType.SPARK)
        .Post(ParticleType.FIRE)
        .Post(ParticleType.SPARK, 0.2f);

Particles on the same step play in the order they were added.
Delay is the wait after the previous particle on the same step, so delays stack into a sequence.

See Explosion.cs
*/

/// <summary>
/// Explosion step a particle plays on
/// </summary>
public enum ExplosionVFXStep
{
    PreMutate,  // cells before the mutator modifies them
    PostMutate, // cells after the mutator modifies them
}

/// <summary>
/// Wrapper around ParticleType. Holds multiple particles and plays them per explosion step, in sequence.
/// </summary>
public class ExplosionVFX
{
    private const float VfxDepth = -1f; // For particle system. -1 ensures it renders above the grid

    private struct Entry
    {
        public ParticleType Particle;
        public ExplosionVFXStep Step;
        public float Delay;
    }

    private readonly List<Entry> _entries = new();

    /// <summary>
    /// Add a particle to play on a step
    /// </summary>
    /// <param name="particle">Particle to play on each cell</param>
    /// <param name="step">Explosion step to play on</param>
    /// <param name="delay">Seconds to wait after the previous particle on the same step</param>
    /// <returns>This, for chaining</returns>
    public ExplosionVFX Add(ParticleType particle, ExplosionVFXStep step, float delay = 0f)
    {
        _entries.Add(new Entry { Particle = particle, Step = step, Delay = delay });
        return this;
    }

    /// <summary>
    /// Add a particle to play on the PreMutate step
    /// </summary>
    public ExplosionVFX Pre(ParticleType particle, float delay = 0f) => Add(particle, ExplosionVFXStep.PreMutate, delay);

    /// <summary>
    /// Add a particle to play on the PostMutate step
    /// </summary>
    public ExplosionVFX Post(ParticleType particle, float delay = 0f) => Add(particle, ExplosionVFXStep.PostMutate, delay);

    /// <summary>
    /// Lets a single ParticleType be passed anywhere an ExplosionVFX is expected. Plays on PostMutate.
    /// </summary>
    public static implicit operator ExplosionVFX(ParticleType particle) => new ExplosionVFX().Post(particle);

    /// <summary>
    /// Play every particle on the given step at each cell
    /// </summary>
    public void Play(ExplosionVFXStep step, List<GridCell> cells)
    {
        // Snapshot positions now, cells may change before delayed particles play
        var positions = new List<Vector2Int>(cells.Count);
        foreach (GridCell cell in cells)
            positions.Add(cell.GetCellPosition());

        PlaySequence(step, positions);
    }

    // async void so exceptions are logged by Unity instead of being swallowed
    private async void PlaySequence(ExplosionVFXStep step, List<Vector2Int> positions)
    {
        foreach (Entry entry in _entries)
        {
            if (entry.Step != step) continue;
            if (entry.Delay > 0f) await Awaitable.WaitForSecondsAsync(entry.Delay);

            foreach (Vector2Int position in positions)
                entry.Particle.Play(position.x, position.y, VfxDepth);
        }
    }

    //==============================

    // Example preset. Call anywhere like so - `ExplosionVFX.SPARK_CHAIN`
    public static readonly ExplosionVFX SPARK_CHAIN = new ExplosionVFX()
        .Pre(ParticleType.SPARK)
        .Post(ParticleType.SPARK, 1f)
        .Post(ParticleType.SPARK, 1f);

    // Example preset for single effect
    public static readonly ExplosionVFX SPARK_SINGLE = ParticleType.SPARK;
}

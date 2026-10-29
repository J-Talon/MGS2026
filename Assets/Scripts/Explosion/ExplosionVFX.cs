// Author: Jeremy
using System.Collections.Generic;
using UnityEngine;
using Particle.Base;

/*
HOW TO USE EXPLOSION VFX
1) Create an ExplosionVFX and add particles with the step they play on and an optional delay
    var vfx = new ExplosionVFX()
        .Add(ParticleType.SPARK, ExplosionVFXStep.PreMutate)
        .Add(ParticleType.FIRE, ExplosionVFXStep.PostMutate)
        .Add(ParticleType.SPARK, ExplosionVFXStep.PostMutate, 0.2f);
2) Pass it to an explosion:
    new Explosion(pattern, center, vfx, mutator, damage, source).Detonate();

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
}

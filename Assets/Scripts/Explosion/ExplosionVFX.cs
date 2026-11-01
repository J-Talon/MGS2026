// Author: Jeremy
using System.Collections.Generic;
using UnityEngine;
using Particle.Base;

/*
HOW TO USE EXPLOSION VFX
Pass any of these as the vfx of an explosion:
1) A preset from the bottom of this class
    new Explosion(pattern, ExplosionVFX.SPARK_CHAIN, mutator, damage).Detonate(center);
2) A single particle, which plays on PostMutate
    new Explosion(pattern, ParticleType.SPARK, mutator, damage).Detonate(center);
3) A custom sequence of any effects. If you reuse it, add it as a preset instead
    var vfx = new ExplosionVFX()
        .Pre(ParticleType.SPARK)
        .Post(new ExplosionEffectSound(boomClip))
        .Post(new ExplosionEffectScreenShake(0.3f, 0.2f))
        .Post(ParticleType.FIRE, 0.2f);

Effects on the same step play in the order they were added.
Delay is the wait after the previous effect on the same step, so delays stack into a sequence.

See Explosion.cs and ExplosionEffect.cs
*/

/// <summary>
/// Explosion step an effect plays on
/// </summary>
public enum ExplosionVFXStep
{
    PreMutate,  // cells before the mutator modifies them
    PostMutate, // cells after the mutator modifies them
}

/// <summary>
/// Holds multiple effects and plays them per explosion step, in sequence.
/// </summary>
public class ExplosionVFX
{
    private struct Entry
    {
        public ExplosionEffect Effect;
        public ExplosionVFXStep Step;
        public float Delay;
    }

    private readonly List<Entry> _entries = new();

    /// <summary>
    /// Add an effect to play on a step
    /// </summary>
    /// <param name="effect">Effect to play</param>
    /// <param name="step">Explosion step to play on</param>
    /// <param name="delay">Seconds to wait after the previous effect on the same step</param>
    /// <returns>This, for chaining</returns>
    public ExplosionVFX Add(ExplosionEffect effect, ExplosionVFXStep step, float delay = 0f)
    {
        _entries.Add(new Entry { Effect = effect, Step = step, Delay = delay });
        return this;
    }

    /// <summary>
    /// Add an effect to play on the PreMutate step
    /// </summary>
    public ExplosionVFX Pre(ExplosionEffect effect, float delay = 0f) => Add(effect, ExplosionVFXStep.PreMutate, delay);

    /// <summary>
    /// Add an effect to play on the PostMutate step
    /// </summary>
    public ExplosionVFX Post(ExplosionEffect effect, float delay = 0f) => Add(effect, ExplosionVFXStep.PostMutate, delay);

    /// <summary>
    /// Lets a single ParticleType be passed anywhere an ExplosionVFX is expected. Plays on PostMutate.
    /// </summary>
    public static implicit operator ExplosionVFX(ParticleType particle) => new ExplosionVFX().Post(particle);

    /// <summary>
    /// Lets a single ExplosionEffect be passed anywhere an ExplosionVFX is expected. Plays on PostMutate.
    /// </summary>
    public static implicit operator ExplosionVFX(ExplosionEffect effect) => new ExplosionVFX().Post(effect);

    /// <summary>
    /// Play every effect on the given step
    /// </summary>
    public void Play(ExplosionVFXStep step, Explosion explosion)
    {
        // Snapshot now, the explosion may change or detonate again before delayed effects play
        var positions = new List<Vector2Int>(explosion.Cells.Count);
        foreach (GridCell cell in explosion.Cells)
            positions.Add(cell.GetCellPosition());

        PlaySequence(step, explosion.Center, positions);
    }

    // async void so exceptions are logged by Unity instead of being swallowed
    private async void PlaySequence(ExplosionVFXStep step, Vector2Int center, List<Vector2Int> positions)
    {
        foreach (Entry entry in _entries)
        {
            if (entry.Step != step) continue;
            if (entry.Delay > 0f) await Awaitable.WaitForSecondsAsync(entry.Delay);

            entry.Effect.Play(center, positions);
        }
    }

    //==============================

    // EXPERIMENTAL PRESET
    public static readonly ExplosionVFX TEST = new ExplosionVFX()
        .Pre(ParticleType.SPARK)
        // .Post(new ExplosionEffectSound(boomClip))
        .Post(new ExplosionEffectScreenShake(0.3f, 0.2f))
        .Post(ParticleType.FIRE, 0.2f);

    // Example preset. Call anywhere like so - `ExplosionVFX.SPARK_CHAIN`
    public static readonly ExplosionVFX SPARK_CHAIN = new ExplosionVFX()
        .Pre(ParticleType.SPARK)
        .Post(ParticleType.SPARK, 1f)
        .Post(ParticleType.SPARK, 1f);

    // Example preset for single effect
    public static readonly ExplosionVFX SPARK_SINGLE = ParticleType.SPARK;
}

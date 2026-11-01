// Author: Jeremy
using System.Collections.Generic;
using Particle.Base;
using UnityEngine;

/*
HOW TO CREATE A NEW EXPLOSION EFFECT
1) Create a class inheriting ExplosionEffect, e.g. ExplosionEffectXXX
2) Override Play and use the center and cells however you need
3) Add it to an ExplosionVFX:
    new ExplosionVFX().Post(new ExplosionEffectXXX(...));

Presets share their effects between explosions, so do not keep per-explosion state in fields.

See ExplosionVFX.cs
*/

/// <summary>
/// Something an explosion plays for feedback, e.g. a particle, sound or screen shake
/// </summary>
public abstract class ExplosionEffect
{
    /// <param name="center">Grid position of the explosion center</param>
    /// <param name="cells">Grid positions of the affected cells</param>
    public abstract void Play(Vector2Int center, List<Vector2Int> cells);

    /// <summary>
    /// Lets a ParticleType be passed anywhere an ExplosionEffect is expected
    /// </summary>
    public static implicit operator ExplosionEffect(ParticleType particle) => new ExplosionEffectParticle(particle);
}

/// <summary>
/// Plays a particle on every cell
/// </summary>
public class ExplosionEffectParticle : ExplosionEffect
{
    private const float VfxDepth = -1f; // For particle system. -1 ensures it renders above the grid

    private readonly ParticleType _particle;

    public ExplosionEffectParticle(ParticleType particle)
    {
        _particle = particle;
    }

    public override void Play(Vector2Int center, List<Vector2Int> cells)
    {
        foreach (Vector2Int cell in cells)
            _particle.Play(cell.x, cell.y, VfxDepth);
    }
}

/// <summary>
/// Plays a sound once at the explosion center
/// </summary>
public class ExplosionEffectSound : ExplosionEffect
{
    private readonly AudioClip _clip;
    private readonly float _volume;

    public ExplosionEffectSound(AudioClip clip, float volume = 1f)
    {
        _clip = clip;
        _volume = volume;
    }

    public override void Play(Vector2Int center, List<Vector2Int> cells)
    {
        AudioSource.PlayClipAtPoint(_clip, (Vector2)center, _volume);
    }
}

/// <summary>
/// Shakes the main camera, fading out over the duration
/// </summary>
public class ExplosionEffectScreenShake : ExplosionEffect
{
    private readonly float _duration;
    private readonly float _strength;

    public ExplosionEffectScreenShake(float duration, float strength)
    {
        _duration = duration;
        _strength = strength;
    }

    public override void Play(Vector2Int center, List<Vector2Int> cells)
    {
        Shake(Camera.main);
    }

    // Moves the camera by the change in offset instead of to a saved position, so overlapping shakes add together
    private async void Shake(Camera camera)
    {
        Vector3 offset = Vector3.zero;
        for (float elapsed = 0f; elapsed < _duration; elapsed += Time.deltaTime)
        {
            if (camera == null) return;
            Vector3 nextOffset = Random.insideUnitCircle * (_strength * (1f - elapsed / _duration));
            camera.transform.position += nextOffset - offset;
            offset = nextOffset;
            await Awaitable.NextFrameAsync();
        }
        if (camera != null) camera.transform.position -= offset;
    }
}

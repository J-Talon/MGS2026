using UnityEngine;
using Particle.Base;
using System;

/// <summary>
/// Peashooter Spell
/// </summary>
public class PeaBullet : EntityProjectile
{
    public float wiggleFactor;
    public float wiggleSpeed;
    private float randPhase = 0;

    public new void Start() {
        base.Start();
        randPhase = UnityEngine.Random.Range(0, (float)Math.PI*2);
    }

    public override void Die()
    {
        ParticleType.SPARK.Play(transform.position.x, transform.position.y, -1.0f);
        base.Die();
    }

    public void Update() {
        base.Tick();

        var dtime = (double)Time.time;

        // slight perpindicular movement to make the bullet look more interesting
        Vector2 wiggle = new(0, 
            (float)Math.Sin(
                dtime*Math.PI*2*speed*wiggleSpeed + randPhase
            )*
            Math.Clamp(wiggleFactor, 0, 0.5f)
        );

        MoveProjectileSmooth(new Vector2(1, 0)*speed+wiggle);

        // check if enemy is at position and damage them and disappear if it is
        var entity = GridManager.instance.GetEntityAtTilePosition(new Vector2Int((int)transform.position.x, (int)transform.position.y));

        if (entity != null && !entity.GetIsFriendly()) {
            entity.TakeDamage(damage);
            Die();
        }
    }
}

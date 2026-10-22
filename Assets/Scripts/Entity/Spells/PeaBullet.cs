using UnityEngine;
using Particle.Base;
using System;

public class PeaBullet : EntityProjectile
{
    public Vector2 direction;
    public float wiggleFactor;
    public float wiggleSpeed;
    private float randPhase = 0;

    void Start() {
        base.Start();
        randPhase = UnityEngine.Random.Range(0, (float)Math.PI*2);
        //ParticleType.FIRE.Play(transform, -1);
    }

    void Update() {
        var dtime = (double)Time.time;

        Vector2 wiggle = new Vector2(0, 
            (float)Math.Sin(
                dtime*Math.PI*2*speed*wiggleSpeed + randPhase
            )*
            Math.Clamp(wiggleFactor, 0, 0.5f)
        );

        MoveProjectileSmooth(direction*speed+wiggle);

        var entity = GridManager.instance.GetEntityAtTilePosition(new Vector2Int((int)transform.position.x, (int)transform.position.y));

        if (entity != null && !entity.GetIsFriendly()) {
            entity.TakeDamage(damage);
        }
    }
}

using UnityEngine;
using Particle.Base;

public class PeaBullet : EntityProjectile
{
    public Vector2 direction;

    void Start() {
        base.Start();
        base.Setup(true, EntityType.ProjectilePea);
        //ParticleType.FIRE.Play(transform, -1);
    }

    void Update() {
        base.Tick();
        MoveProjectileSmooth(direction*speed);
    }
}

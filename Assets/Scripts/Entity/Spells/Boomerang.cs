using UnityEngine;
using Particle.Base;
using System;

public class Boomerang : EntityProjectile
{
    public Vector2 direction;
    [SerializeField]
    private float speedScale = 1;
    [SerializeField]
    private float speedDecay = 0.5f;

    private int bounces = 0;

    public new void Start() {
        base.Start();
    }

    Vector2Int Gridpos(Vector3 vec) {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }

    public override void ResetToOriginal()
    {
        base.ResetToOriginal();
        speedScale = 1;
        bounces = 0;
    }

    public void Update() {
        base.Tick();

        IGridSystemView view = GridManager.instance;

        float correction = 0.0f;

        if (speedScale < 0.1) {
            correction = speedScale;
        }

        var delta = new Vector2(
            (float)(transform.position.x - Math.Floor(transform.position.x)), 
            (float)(transform.position.y - Math.Floor(transform.position.y))
        );
        
        var correctionVec = correction * delta;

        MoveProjectileSmooth(direction*speedScale*speed+correctionVec);

        if (speedScale > 0) {
            speedScale -= speedDecay * Time.deltaTime;
        } else {
            speedScale = 0;
        }

        var entity = view.GetEntityAtTilePosition(Gridpos(transform.position));

        if (entity && !entity.GetIsPlayer()) {
            ParticleType.SPARK.Play(transform.position.x, transform.position.y, -1);
            entity.TakeDamage(damage);
        }

        if (bounces > 0) {
            if (entity && entity.GetIsPlayer()) {
                entity.SetIsHoldingBoomerang(true);
                base.Die();
            }
        }

        if (!view.WithinGridSmooth(transform.position)) {
            direction = -direction;
            bounces += 1;
        }

        //var entity = GridManager.instance.GetEntityAtTilePosition(new Vector2Int((int)transform.position.x, (int)transform.position.y))
    }
}

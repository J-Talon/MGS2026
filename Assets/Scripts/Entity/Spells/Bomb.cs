using UnityEngine;
using System;

class Bomb : EntityProjectile {
    public Vector2Int target;
    public Vector2Int start;
    public float height;
    private float nextHeight;

    void Start() {
        var h = UnityEngine.Random.Range(height, height/2);

        if (UnityEngine.Random.Range(0, 1) > 0.5) {
            h *= -1;
        }

        nextHeight = h;

        base.Start();
    }

    Vector2 CalculateTrajectory(Vector2Int start, Vector2Int end, float height, float time) {
        var startVec = new Vector2(start.x, start.y);
        var endVec = new Vector2(end.x, end.y);
        var delta = endVec - startVec;

        var tangent = Vector2.Perpendicular(delta.normalized);

        // at 0 its 0 at 0.5 its 1 and at 1 its 0 again
        var timeCurved = -4 * Math.Pow(time - 0.5, 2)+1;

        Debug.Assert(timeCurved != double.NaN);
        Debug.Log($"{delta}, {tangent}, {timeCurved}, {height}, {time}, {startVec}, {endVec}");

        return tangent * (float)timeCurved * height + delta * time + startVec;
    }

    void Update() {
        var lifeSpan = Time.time - spawnTime;

        Debug.Assert(maxLifeSpan != 0);

        var deathPercent = lifeSpan / maxLifeSpan;

        var traj = CalculateTrajectory(start, target, nextHeight, deathPercent);
        transform.position = new Vector3(traj.x, traj.y, -1);

        base.Tick();
    }
}

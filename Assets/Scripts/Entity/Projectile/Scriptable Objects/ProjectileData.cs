using System.Data.Common;
using UnityEngine;

/// <summary>
/// This scriptable object holds the configuration data for the grid manager, including grid size, starting friendly columns, and bounding box limits.
/// </summary>
[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/Entity/Projectile/ProjectileData", order = 1)]
public class ProjectileData : ScriptableObject
{
    public float movementTickTimer = 1f;
}

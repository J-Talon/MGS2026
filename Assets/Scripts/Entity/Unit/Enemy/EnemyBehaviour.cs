/// Author: Angad 
using UnityEngine;

// base behaviour abstract class, other's inherit from this
// requires EnemyBase to be attatched
[RequireComponent(typeof(EnemyBase))]
public abstract class EnemyBehaviour : MonoBehaviour
{
    // tick function all behaviours will ened
    public virtual void Tick(EnemyBase enemy){}
}

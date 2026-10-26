/// Author: Angad 
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Base enemy movement, randomly generated depending on valid directions 
/// </summary>
public class EnemyMovement : EnemyBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        
    }

    void Update()
    {
        Tick();
    }

    public override void Tick()
    {
        base.Tick();

        ProcessAIMovement(this);
    }
}

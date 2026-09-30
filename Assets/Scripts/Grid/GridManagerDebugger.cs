using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class should be invoked only by the grid manager itself in awake (in the same game object as well) 
/// </summary>
public class GridManagerDebugger : MonoBehaviour
{
    //Public interface references to the manager
    IGridSystemControl manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = gameObject.GetComponent<IGridSystemControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasReleasedThisFrame) //increase player control
        {
            manager.SingleColControlUpdate();
        }
        if (Keyboard.current.qKey.wasReleasedThisFrame) //decrease player control
        {
            manager.SingleColControlUpdate(false);
        }
    }
}

// Authors: [Jacky, Jeremy, Mark]
using Event;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class should be invoked only by the grid manager itself in awake (in the same game object as well) 
/// </summary>
public class GridManagerDebugger : MonoBehaviour
{
    //Public interface references to the manager
    IGridSystemControl managerControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        managerControls = gameObject.GetComponent<IGridSystemControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasReleasedThisFrame) //increase player control
        {
            managerControls.SingleColControlUpdate();
        }
        if (Keyboard.current.qKey.wasReleasedThisFrame) //decrease player control
        {
            managerControls.SingleColControlUpdate(false);
        }

        //event checking
        if (Keyboard.current.rKey.wasReleasedThisFrame) //increase player control
        {
            GameplayEvents.gridColUpdateEvent.CallEvent(true);
        }
        if (Keyboard.current.tKey.wasReleasedThisFrame) //decrease player control
        {
            GameplayEvents.gridColUpdateEvent.CallEvent(false);
        }
    }
}

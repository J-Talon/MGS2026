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

        //grid size checking
        if (Keyboard.current.zKey.wasReleasedThisFrame) //add column on the left
        {
            managerControls.AddGridColumn(GridSide.Left);
        }
        if (Keyboard.current.xKey.wasReleasedThisFrame) //remove column on the left
        {
            managerControls.RemoveGridColumn(GridSide.Left);
        }
        if (Keyboard.current.cKey.wasReleasedThisFrame) //remove column on the right
        {
            managerControls.RemoveGridColumn(GridSide.Right);
        }
        if (Keyboard.current.vKey.wasReleasedThisFrame) //add column on the right
        {
            managerControls.AddGridColumn(GridSide.Right);
        }
        if (Keyboard.current.iKey.wasReleasedThisFrame) //add row on the top
        {
            managerControls.AddGridRow(GridRowSide.Top);
        }
        if (Keyboard.current.kKey.wasReleasedThisFrame) //remove row on the top
        {
            managerControls.RemoveGridRow(GridRowSide.Top);
        }
        if (Keyboard.current.oKey.wasReleasedThisFrame) //add row on the bottom
        {
            managerControls.AddGridRow(GridRowSide.Bottom);
        }
        if (Keyboard.current.lKey.wasReleasedThisFrame) //remove row on the bottom
        {
            managerControls.RemoveGridRow(GridRowSide.Bottom);
        }
    }
}

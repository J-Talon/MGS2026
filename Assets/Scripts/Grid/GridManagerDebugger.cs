// Authors: [Jacky, Jeremy, Mark]

using UnityEngine;
using UnityEngine.InputSystem;

namespace Grid
{
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
            managerControls = gameObject.GetComponent<GridManager>().GetManipulator();
        }

        // Update is called once per frame
        void Update()
        {
            if (Keyboard.current.hKey.wasReleasedThisFrame) //increase player control
            {
                Debug.Log("Debug h key");
                managerControls.AdvanceTerritory(true,3);
            }
            if (Keyboard.current.gKey.wasReleasedThisFrame) //decrease player control
            {
                Debug.Log("Debug g key");
                managerControls.AdvanceTerritory(false,3);
            }

        }
    }
}

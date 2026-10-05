// Contributors: devlenko (creator)
// Handles player movement along a grid.

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private Vector2 pos = new Vector2(3, 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            TileController tileController = tileManager.GetTileControllerAt((int)(pos.y + direction.y), (int)(pos.x + direction.x));

            if (tileController != null && tileController.IsSafe)
            {
                pos.Set(pos.x + direction.x, pos.y + direction.y);
                transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, 0);
            }
        }
    }

    /// <summary>
    /// Moves the player position to (0, 0). Useful when resizing the grid during runtime to sync player logic.
    /// </summary>
    [ContextMenu("Reset Player Position")]
    public void ResetPlayerPosition()
    {
        pos = Vector2.zero;
        transform.position = tileManager.GetTileControllerAt(0, 0).transform.position;
    }
}

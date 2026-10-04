// Contributors: devlenko (creator)
// Handles tile logic, such as if the tile is safe or not

using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite safeTileImage;
    [SerializeField] private Sprite unsafeTileImage;

    [SerializeField] private bool isSafe;
    public bool IsSafe
    {
        get { return isSafe; }
        set
        {
            isSafe = value;
            UpdateTileImg();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTileImg();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateTileImg()
    {
        spriteRenderer.sprite = isSafe ? safeTileImage : unsafeTileImage;
    }

    /// <summary>
    /// Toggles the tile's safety. Kept for debugging.
    /// </summary>
    [ContextMenu("Toggle Tile Safety")]
    public void ToggleSafety()
    {
        isSafe = !isSafe;
        UpdateTileImg();
    }
}

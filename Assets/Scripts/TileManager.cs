// Contributors: devlenko (creator)
// Handles all grid tiles

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 8;
    [SerializeField] private int safeColumns = 4;
    private Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Create New Grid")]
    private void CreateGrid()
    {
        float rowOffset = rows / 2f - 0.5f;
        float colOffset = columns / 2f - 0.5f;

        // Destroy the previous grid and create a new one
        foreach (Vector2 key in tiles.Keys)
        {
            Destroy(tiles[key]);
        }
        tiles.Clear();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Instantiate the tile so that the grid is centered to the camera
                // Rows and columns are positioned based on a Cartesian plane; (0, 0) is bottom left
                GameObject tile = Instantiate(tilePrefab, new Vector3((float)(j - colOffset), (float)(i - rowOffset), 0), Quaternion.identity);
                tile.name = $"Tile ({i}, {j})";

                tile.GetComponent<TileController>().Init(j + 1 <= safeColumns);

                tiles[new Vector2(i, j)] = tile;
            }
        }
    }

    public TileController GetTileControllerAt(int row, int col)
    {
        if (tiles.TryGetValue(new Vector2(row, col), out GameObject tile)) return tile.GetComponent<TileController>();
        return null;
    }
}

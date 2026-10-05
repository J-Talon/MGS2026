/*  
author: Kian Reilly 
github: kareilly
discrd: kuushi

This is just for rendering the example grid, it isn't extendable or efficient.
*/

using System;
using System.Collections;
using UnityEngine;

public class GridExample : MonoBehaviour
{
    
    // adds a sprite object for each tile on start
    void Start()
    {
        StartCoroutine(AtEndOfFrame());
    }
    IEnumerator AtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        
        transform.position = GridManager.Instance.GetGridOrigin();
        Vector2Int gridDimensions = GridManager.Instance.GetGridDimensions();

        print(gridDimensions.ToString());

        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector2 tileSize = GridManager.Instance.GetTileDimensions();
                Vector2 tilePosition = GridManager.Instance.GetTilePosition(new Vector2Int(x,y));

                String uniqueObjName = "tileSprite" + x.ToString() + y.ToString();

                GameObject tileSpriteObj = new GameObject(uniqueObjName);
                tileSpriteObj.transform.position = tilePosition;
                tileSpriteObj.transform.SetParent(transform, true);
                tileSpriteObj.transform.localScale = tileSize * 0.9f;
                print(x.ToString() + ", " + y.ToString());

                SpriteRenderer spriteRenderer = tileSpriteObj.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = -1;

                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.green);

                if (!GridManager.Instance.IsTileOwned(new Vector2Int(x,y)))
                {
                    texture.SetPixel(0, 0, Color.purple);
                }

                texture.Apply();

                Sprite squareSprite = Sprite.Create(texture, new Rect(new Vector2(0,0), new Vector2(1,1)), new Vector2(0.5f,0.5f), 1f);

                spriteRenderer.sprite = squareSprite;

            }
        }
    }
}

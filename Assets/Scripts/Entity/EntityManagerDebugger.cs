// Authors: [Jacky, Jeremy, Mark]
using Event;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// CLASS SHOULD BE ENABLED IN A SCENE WHEN DEBUGGING ENTITY MANAGER STUFF
/// taken from a previous project i did, uses the shitter IMGUI for debugging for ease of use
/// </summary>
public class EntityManagerDebugger : MonoBehaviour
{
    EntityManager manager;

    void Start()
    {
        manager = EntityManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasReleasedThisFrame) //spawn a projectile to test
        {
            manager.GetEntity(EntityType.ProjectileTestZiggedSmooth, Vector3.zero, true);
        }
    }

    /// <summary>
    /// TEMP Debug IMGUI
    /// </summary>
    void OnGUI()
    {

            
        //base sizings
        const int BASE_FONT_SIZE = 16;

        string tempMenuText = "All Characters (regardless of active or pooled)" + manager.GetAllEntitiesCount() + "\n";
        tempMenuText += "(Stats) ---: " + "\n";
        tempMenuText += "Inactive Pooled: " + manager.GetTotalPooledEntitiesCount() + "\n";
        tempMenuText += "Active: " + manager.GetActiveEntitiesCount() + "\n";
        
        GUIStyle tempStyle = new GUIStyle();
        Vector2 tempSize = new Vector2(0,0);
        tempStyle.fontSize = BASE_FONT_SIZE;
        tempStyle.normal.background = Texture2D.whiteTexture;
        tempSize = tempStyle.CalcSize(new GUIContent(tempMenuText));

        GUI.Label(new Rect(Screen.width - tempSize.x, 0, tempSize.x, tempSize.y), tempMenuText, tempStyle);
    
        

    
    }
}

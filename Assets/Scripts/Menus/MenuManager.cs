using System;
using System.Collections.Generic;
using Event;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is the UI Menu version of the 'InputEvents.cs' script.
/// 
/// Author: Logan (LogChiCha)
/// </summary>
public class MenuManager : MonoBehaviour
{
    private InputSystemActions m_UIManager;
    public static MenuManager Instance { get; private set; }
    
    [Header("Debug Settings")]
    [Tooltip("Log directional movement input values.")]
    [SerializeField] private bool debugMovement; // do you want console to output the movement data

    [Tooltip("Log button action activations.")]
    [SerializeField] private bool debugActions;  // do you want console to output the button data

    private BaseMenu activeMenu;
    private Stack<BaseMenu> OpenedMenus = new Stack<BaseMenu>(); // Stack of menus. So we can backtrack between opened menus.

    private Dictionary<InputAction, Vector2Int> movementDirections; //holds the 4 directions we want to move. Grabs the InputAction from the inputactions file and associates it with a direction



    [Header("Menu Prefab References Canvas")]
    [SerializeField] private GameObject mainMenu;

   // [Header("Menu Prefab References")]
    //[SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_UIManager = new InputSystemActions();

        movementDirections = new Dictionary<InputAction, Vector2Int>
        {
            {m_UIManager.UI.MoveUp, new Vector2Int(0, 1) },
            {m_UIManager.UI.MoveDown, new Vector2Int(0, -1) },
            {m_UIManager.UI.MoveLeft, new Vector2Int(-1, 0) },
            {m_UIManager.UI.MoveRight, new Vector2Int(1, 0) },
        };

        // Instantiate the menus!!!
      //  pauseMenu = Instantiate(pauseMenu);

        //DontDestroyOnLoad(pauseMenu);

       // pauseMenu.GetComponent<BaseMenu>().Close();
      
    }

      private void Start()
    {
        m_UIManager.Enable();
        SetCurrentMenu(mainMenu);
    }
    private void OnDestroy()
    {
        m_UIManager?.Dispose();
    }

    // Subscribe while enabled to avoid processing input when inactive.
    void OnEnable()
    {
        foreach (InputAction movementAction in movementDirections.Keys)
        {
            movementAction.performed += OnMove;
            movementAction.canceled += OnMove;
        }
        m_UIManager.UI.Submit.performed += HandleSubmit;
        m_UIManager.UI.Cancel.performed += HandleCancel;

        m_UIManager.Enable();
    }

    // Unsubscribe while enabled to avoid processing input when inactive.
    void OnDisable()
    {
        foreach (InputAction movementAction in movementDirections.Keys)
        {
            movementAction.performed -= OnMove;
            movementAction.canceled -= OnMove;
        }
        m_UIManager.UI.Submit.performed -= HandleSubmit;
        m_UIManager.UI.Cancel.performed -= HandleCancel;

        m_UIManager.Disable();
    }


// ADD OTHER MENUS HERE
    public void OpenPause()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GridTestScene") //Change to whatever scene game is on
        {
           // OpenMenu(pauseMenu);
        }
    }

    /* EXAMPLE
    public void OpenSettings()
    {
        OpenMenu(settingsMenu);
    }*/




    public void SetCurrentMenu(GameObject menu)
    {
        BaseMenu menuToOpen = menu.GetComponent<BaseMenu>();

        if (menuToOpen == null)
        {
            Debug.LogError($"Menu prefab {menu.name} is missing a BaseMenu component.");
            return;
        }

        if (activeMenu != null)
        {
            OpenedMenus.Pop();
            activeMenu.Close();
        } else
        {
           m_UIManager.Enable();
        }

        activeMenu = menuToOpen;
        OpenedMenus.Push(activeMenu);
        activeMenu.Open();
    }

    public void OpenMenu(GameObject menuPrefab)
    {
        // Get the menu we want to open
        BaseMenu menuToOpen = menuPrefab.GetComponent<BaseMenu>();

        if (menuToOpen == null) // If it's not a menu don't open it
        {
            Debug.LogError($"Menu prefab {menuPrefab.name} is missing a BaseMenu component.");
            return;
        }

        // Hide the previous active menu
        if (activeMenu != null)
        {
            activeMenu.Hide();
        }

        // Setting the active menu to be the one we want opened
        activeMenu = menuToOpen;
        OpenedMenus.Push(activeMenu);
        activeMenu.Open(); // Hey we finally opened the menu

       if (OpenedMenus.Count == 1) m_UIManager.Enable(); // if this is the first menu opened lets enable ui inputs
    }
    public void CloseMenu()
    {
        if (activeMenu == null) return;

        // Close the active menu
        OpenedMenus.Pop();
        activeMenu.Close();

        //Destroy(activeMenu.gameObject); // destroy

        // Set the active menu to the menu behind it, if it exists.
        if (OpenedMenus.Count >= 1)
        {
            activeMenu = OpenedMenus.Peek();
            activeMenu.Show();
        }
        else
        {
            activeMenu = null;
            if (InputEvents.Instance != null)
                InputEvents.Instance.OnEnable();
            m_UIManager.Disable();
        }
    }

    // This method just closes all the opened menus
    public void CloseAllMenus()
    {
        if (activeMenu == null) return;

        foreach (BaseMenu openedMenu in OpenedMenus)
        {
            openedMenu.Close();
        }

        OpenedMenus.Clear();
        if (InputEvents.Instance != null)
            InputEvents.Instance.OnEnable();
        m_UIManager.Disable();
        activeMenu = null;
    }
    protected void OnMove(InputAction.CallbackContext obj)
    {
        //check the players input and gets the associated direction to move
        Vector2Int direction = movementDirections[obj.action];

        if (obj.performed)
        {
            if (debugMovement == true)
            {
                Debug.Log($"Movement pressed - {direction}");
            }

            if (activeMenu != null)
                activeMenu.HandleNavigate(direction); //calls the event for the appropriate movement direction
        }

        else if (obj.canceled)
        {
            if (debugMovement == true)
            {
                Debug.Log($"Movement released - {direction}");
            }

            //check if the player releases the movement button they were holding (for things like changing what movement direction they want to do/buffer)
            if (activeMenu != null)
                activeMenu.HandleNavigate(direction); //calls the event for the appropriate movement direction
        }
    }

    private void HandleSubmit(InputAction.CallbackContext context)
    {
        activeMenu?.HandleSubmit();
    }

    private void HandleCancel(InputAction.CallbackContext context)
    {
        if (activeMenu != null)
        {
            activeMenu.HandleCancel();
        }
        else
        {
           // OpenPause(); // fallback — close menu or open pause
        }
    }
}
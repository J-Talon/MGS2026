// Authors: Farbod
//edited by - Jordan October 14, 2026
// edited by - Wilson October 25, 2026
//edited by - Jordan Nov 08 2026

using System;
using System.Collections.Generic;
using Event;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Bridges Unity's Input System with the game's event system
/// by forwarding input actions to GameplayEvents.
/// </summary>
public class InputEvents : MonoBehaviour
{ 
    private InputSystemActions m_InputManager;
    public static InputEvents Instance { get; private set; }
    
    [Header("Debug Settings")]
    [Tooltip("Log directional movement input values.")]
    [SerializeField] private bool debugMovement; // do you want console to output the movement data

    [Tooltip("Log button action activations.")]
    [SerializeField] private bool debugActions;  // do you want console to output the button data

    private Dictionary<InputAction, Vector2Int> movementDirections; //holds the 4 directions we want to move. Grabs the InputAction from the inputactions file and associates it with a direction
    private Dictionary<InputAction, int> spellDecks;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        m_InputManager = new InputSystemActions();

        //makes an item for the dict with the InputAction and the direction
        movementDirections = new Dictionary<InputAction, Vector2Int>
        {
            {m_InputManager.Player.MoveUp, new Vector2Int(0, 1) },
            {m_InputManager.Player.MoveDown, new Vector2Int(0, -1) },
            {m_InputManager.Player.MoveLeft, new Vector2Int(-1, 0) },
            {m_InputManager.Player.MoveRight, new Vector2Int(1, 0) },
        };

        spellDecks = new Dictionary<InputAction, int>
        {
            {m_InputManager.Player.BasicShot, 0},
            {m_InputManager.Player.SpawnTurret, 1},
            {m_InputManager.Player.Spell1, 2},
            {m_InputManager.Player.Spell2, 3},
            {m_InputManager.Player.Spell3, 4}
        };
    }

    private void OnDestroy()
    {
        m_InputManager?.Dispose();
    }

    // Subscribe while enabled to avoid processing input when inactive.
    public void OnEnable()
    {
        m_InputManager.Player.Interact.performed += OnInteract;

        foreach (InputAction spellAction in spellDecks.Keys)
        {
            spellAction.performed += OnSpell;
        }

        m_InputManager.Player.SpawnTurret.performed += OnSpawnTurret;

        foreach (InputAction movementAction in movementDirections.Keys)
        {
            movementAction.performed += OnMove;
            movementAction.canceled += OnMove;
        }

        m_InputManager.Player.MoveCrosshairLeft.performed += OnCrosshairLeft;
        m_InputManager.Player.MoveCrosshairRight.performed += OnCrosshairRight;

        m_InputManager.Enable();
    }

    // Unsubscribe while enabled to avoid processing input when inactive.
    public void OnDisable()
    {
        m_InputManager.Player.Interact.performed -= OnInteract;

        foreach (InputAction spellAction in spellDecks.Keys)
        {
            spellAction.performed -= OnSpell;
        }

        m_InputManager.Player.SpawnTurret.performed -= OnSpawnTurret;

        foreach (InputAction movementAction in movementDirections.Keys)
        {
            movementAction.performed -= OnMove;
            movementAction.canceled -= OnMove;
        }

        m_InputManager.Player.MoveCrosshairLeft.performed -= OnCrosshairLeft;
        m_InputManager.Player.MoveCrosshairRight.performed -= OnCrosshairRight;

        m_InputManager.Disable();
    }

    //modified by Jordan
    private void OnMove(InputAction.CallbackContext obj)
    {
        //check the players input and gets the associated direction to move
        Vector2Int direction = movementDirections[obj.action];

        if (obj.performed)
        {
            if (debugMovement == true)
            {
                Debug.Log($"Movement pressed - {direction}");
            }

            GameplayEvents.Input.movePressed.CallEvent(direction); //calls the event for the appropriate movement direction
        }

        else if (obj.canceled)
        {
            if (debugMovement == true)
            {
                Debug.Log($"Movement released - {direction}");
            }

            //check if the player releases the movement button they were holding (for things like changing what movement direction they want to do/buffer)
            GameplayEvents.Input.moveReleased.CallEvent(direction); //calls the event for the appropriate movement direction
        }
        
    }

    private void OnCrosshairLeft(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("CrosshairLeft Activated");
        GameplayEvents.Input.crosshairLeftPressed.CallEvent(ValueTuple.Create());
    }

    private void OnCrosshairRight(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("CrosshairRight Activated");
        GameplayEvents.Input.crosshairRightPressed.CallEvent(ValueTuple.Create());
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Interact Activated");
        GameplayEvents.Input.interactPressed.CallEvent(ValueTuple.Create());
    }

    private void OnSpell(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("A Spell Activated");

        int deckNumber = spellDecks[context.action];
        GameplayEvents.Input.spellPressed.CallEvent(deckNumber);
    }

    private void OnSpawnTurret(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spawn Turret Pressed");
        GameplayEvents.Input.spawnTurretPressed.CallEvent(ValueTuple.Create());
    }
}

//CHANGELOG
//
//Jordan - Nov 08, 2026
//simplified the movement event stuff so its easier to do stuff here with. Aka the list
//
//Jordan - Nov 08, 2026
//simplified the spell event stuff so its easier if we need more spellDeck and buttons. Aka, the same thing i did for the Oct 14 2026 movement stuff, but for spells. Aka the list
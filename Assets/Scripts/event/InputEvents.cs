// Authors: Farbod
//edited by - Jordan October 14, 2026
// edited by - Wilson October 25, 2026

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
    }
    private void OnDestroy()
    {
        m_InputManager?.Dispose();
    }

    // Subscribe while enabled to avoid processing input when inactive.
    public void OnEnable()
    {
        m_InputManager.Player.Interact.performed += OnInteract;

        m_InputManager.Player.Spell1.performed += OnSpell1;
        m_InputManager.Player.Spell2.performed += OnSpell2;
        m_InputManager.Player.Spell3.performed += OnSpell3;
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

        m_InputManager.Player.Spell1.performed -= OnSpell1;
        m_InputManager.Player.Spell2.performed -= OnSpell2;
        m_InputManager.Player.Spell3.performed -= OnSpell3;

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

    private void OnSpell1(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spell1 Activated");
        GameplayEvents.Input.spell1Pressed.CallEvent(ValueTuple.Create());
    }

    private void OnSpell2(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spell2 Activated");
        GameplayEvents.Input.spell2Pressed.CallEvent(ValueTuple.Create());
    }

    private void OnSpell3(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spell3 Activated");
        GameplayEvents.Input.spell3Pressed.CallEvent(ValueTuple.Create());
    }

    private void OnSpawnTurret(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spawn Turret Pressed");
        GameplayEvents.Input.spawnTurretPressed.CallEvent(ValueTuple.Create());
    }
}

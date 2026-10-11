// Authors: Farbod

using System;
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
    
    [Tooltip("Broadcast a zero movement vector when movement input is canceled.")]
    [SerializeField] private bool broadcastMovementCanceled;

    [Header("Debug Settings")]
    [Tooltip("Log directional movement input values.")]
    [SerializeField] private bool debugMovement; // do you want console to output the movement data

    [Tooltip("Log button action activations.")]
    [SerializeField] private bool debugActions;  // do you want console to output the button data

    private void Awake()
    {
        m_InputManager = new InputSystemActions();
    }
    private void OnDestroy()
    {
        m_InputManager?.Dispose();
    }

    // Subscribe while enabled to avoid processing input when inactive.
    void OnEnable()
    {
        m_InputManager.Player.Interact.performed += onInteract;

        m_InputManager.Player.Spell1.performed += onSpell1;
        m_InputManager.Player.Spell2.performed += onSpell2;
        m_InputManager.Player.Spell3.performed += onSpell3;
        m_InputManager.Player.PlaceTurret.performed += onPlaceTurret;

        m_InputManager.Player.Move.performed += onMove;
        if (broadcastMovementCanceled) m_InputManager.Player.Move.canceled += onMove;

        m_InputManager.Enable();
    }

    // Unsubscribe while enabled to avoid processing input when inactive.
    void OnDisable()
    {
        m_InputManager.Player.Interact.performed -= onInteract;

        m_InputManager.Player.Spell1.performed -= onSpell1;
        m_InputManager.Player.Spell2.performed -= onSpell2;
        m_InputManager.Player.Spell3.performed -= onSpell3;

        m_InputManager.Player.PlaceTurret.performed -= onPlaceTurret;

        m_InputManager.Player.Move.performed -= onMove;
        if (broadcastMovementCanceled) m_InputManager.Player.Move.canceled -= onMove;

        m_InputManager.Disable();
    }
    private void onMove(InputAction.CallbackContext obj)
    {
        Vector2 move = obj.ReadValue<Vector2>();

        if (debugMovement) Debug.Log($"{obj.control.ToString()} => move = {{ {move[0]} , {move[1]} }};");
        GameplayEvents.Input.movementInput.CallEvent(move);
    }
    private void onInteract(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Interact Activated");
        GameplayEvents.Input.interactPressed.CallEvent(ValueTuple.Create());
    }

    private void onSpell1(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spell1 Activated");
        GameplayEvents.Input.spell1Pressed.CallEvent(ValueTuple.Create());
    }

    private void onSpell2(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spell2 Activated");
        GameplayEvents.Input.spell2Pressed.CallEvent(ValueTuple.Create());
    }

    private void onSpell3(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Spell3 Activated");
        GameplayEvents.Input.spell3Pressed.CallEvent(ValueTuple.Create());
    }

    private void onPlaceTurret(InputAction.CallbackContext context)
    {
        if (debugActions) Debug.Log("Place Turret Pressed");
        GameplayEvents.Input.placeTurretPressed.CallEvent(ValueTuple.Create());
    }
}

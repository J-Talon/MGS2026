// Authors: Farbod

using System;
using Event;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputEvents : MonoBehaviour
{
    private InputSystem_Actions m_InputManager;
    [SerializeField] private bool debugOutput;
    [SerializeField] private bool listenForMoveCanceled;


    private void Awake()
    {
        m_InputManager = new InputSystem_Actions();
    }

    void OnEnable()
    {
        m_InputManager.Player.Interact.performed += onInteract;

        m_InputManager.Player.Spell1.performed += onSpell1;
        m_InputManager.Player.Spell2.performed += onSpell2;
        m_InputManager.Player.Spell3.performed += onSpell3;
        m_InputManager.Player.PlaceTurret.performed += onPlaceTurret;

        m_InputManager.Player.Move.performed += onMove;
        if (listenForMoveCanceled) m_InputManager.Player.Move.canceled += onMove;

        m_InputManager.Enable();
    }

    void OnDisable()
    {
        m_InputManager.Player.Interact.performed -= onInteract;

        m_InputManager.Player.Spell1.performed -= onSpell1;
        m_InputManager.Player.Spell2.performed -= onSpell2;
        m_InputManager.Player.Spell3.performed -= onSpell3;

        m_InputManager.Player.PlaceTurret.performed -= onPlaceTurret;

        m_InputManager.Player.Move.performed -= onMove;
        if (listenForMoveCanceled) m_InputManager.Player.Move.canceled -= onMove;

        m_InputManager.Disable();
    }
    private void onMove(InputAction.CallbackContext obj)
    {
        Vector2 move = obj.ReadValue<Vector2>();

        if (debugOutput) Debug.Log($"{obj.control.ToString()} => move = {{ {move[0]} , {move[1]} }};");
        GameplayEvents.MovementInput.CallEvent(move);
    }
    private void onInteract(InputAction.CallbackContext context)
    {
        if (debugOutput) Debug.Log("Interact Activated");
        GameplayEvents.InteractPressed.CallEvent(ValueTuple.Create());
    }

    private void onSpell1(InputAction.CallbackContext context)
    {
        if (debugOutput) Debug.Log("Spell1 Activated");
        GameplayEvents.Spell1Pressed.CallEvent(ValueTuple.Create());
    }

    private void onSpell2(InputAction.CallbackContext context)
    {
        if (debugOutput) Debug.Log("Spell2 Activated");
        GameplayEvents.Spell2Pressed.CallEvent(ValueTuple.Create());
    }

    private void onSpell3(InputAction.CallbackContext context)
    {
        if (debugOutput) Debug.Log("Spell3 Activated");
        GameplayEvents.Spell3Pressed.CallEvent(ValueTuple.Create());
    }

    private void onPlaceTurret(InputAction.CallbackContext context)
    {
        if (debugOutput) Debug.Log("Turret Placed");
        GameplayEvents.PlaceTurretPressed.CallEvent(ValueTuple.Create());
    }
}

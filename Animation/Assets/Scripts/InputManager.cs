using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    GameControls _gameControls;

    public static InputManager Instance {get; private set; }

    //Controls Events
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action AttackEvent;
    public event Action SprintStartEvent;
    public event Action SprintReleasedEvent;
    public event Action<Vector2> LookEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this; 
        else
            Destroy(this.gameObject);
            
        _gameControls = new GameControls();

        _gameControls.Player.Enable();

        _gameControls.Player.Move.performed += (InputAction.CallbackContext context) => MoveEvent?.Invoke(context.ReadValue<Vector2>());
        _gameControls.Player.Move.canceled += (InputAction.CallbackContext context) => MoveEvent?.Invoke(context.ReadValue<Vector2>());
        _gameControls.Player.Jump.performed += (InputAction.CallbackContext context) => JumpEvent?.Invoke();
        _gameControls.Player.Attack.performed += (InputAction.CallbackContext context) => AttackEvent?.Invoke();
        _gameControls.Player.Sprint.performed += (InputAction.CallbackContext context) => SprintStartEvent?.Invoke();
        _gameControls.Player.Sprint.canceled += (InputAction.CallbackContext context) => SprintReleasedEvent?.Invoke();
        
        
        _gameControls.Player.Look.performed += (InputAction.CallbackContext context) => LookEvent?.Invoke(context.ReadValue<Vector2>());
    }
}

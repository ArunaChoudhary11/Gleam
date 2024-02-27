using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInput _input;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        _input = new PlayerInput();
    }
    public Vector2 c;
    void Update()
    {

    }
    private void OnEnable()
    {
        _input.Enable();
    }
    private void OnDisable()
    {
        _input.Disable();
    }
    public List<string> bindingNames = new List<string>();
    public void Movement(InputAction.CallbackContext ctx)
    {
        c = ctx.ReadValue<Vector2>();

        Debug.Log(ctx.control.device.displayName);
    }
    
    public bool Jump()
    {
        return _input.Player.Jump.triggered;
    }
    
    public bool Attack()
    {
        return _input.Player.Fire.triggered;
    }
    public void ControlsChanged()
    {
        Debug.Log("Controls Changed to: " + _input.devices.Value);
    }
}
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
    private void OnEnable()
    {
        _input.Enable();
    }
    private void OnDisable()
    {
        _input.Disable();
    }
    public bool Jump()
    {
        return _input.Player.Jump.triggered;
    }
    
    public bool Attack()
    {
        return _input.Player.Fire.triggered;
    }
}
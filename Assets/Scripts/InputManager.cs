using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputSystem_Actions Input;
    private void Awake()
    {
        Input = new();
    }
    private void OnEnable()
    {
        Input.Enable();
    }
    private void OnDisable()
    {
        Input.Disable();
    }
}

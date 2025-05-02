using UnityEngine;
using UnityEngine.InputSystem;

public static class PlayerInput
{
    private static InputSystem_Actions _inputActions;
    private static Vector2 _moveDirection = Vector2.zero;
    
    // [SerializeField] private MonoBehaviour playerMovement;
    // [SerializeField] private MonoBehaviour playerAttack;
    
    private static void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable();
    }

    private static void OnEnable()
    {
        _inputActions.Player.Move.performed += moving => { _moveDirection = moving.ReadValue<Vector2>(); };
        _inputActions.Player.Move.canceled += movingStopped => { _moveDirection = Vector2.zero; };
        
        _inputActions.Player.Jump.started += OnJumpPressed;
        
        _inputActions.Player.Crouch.started += OnCrouchPressed;

        _inputActions.Player.Attack.performed += OnAttackUsed;
        _inputActions.Player.Skill.performed += OnSkillUsed;
        _inputActions.Player.Ultimate.performed += OnUltimateUsed;
        
        _inputActions.Player.Enable();
    }

    private static void OnDisable()
    {
        _inputActions.Player.Jump.performed -= OnJumpPressed;
        
        _inputActions.Player.Crouch.started -= OnCrouchPressed;

        _inputActions.Player.Attack.performed -= OnAttackUsed;
        _inputActions.Player.Skill.performed -= OnSkillUsed;
        _inputActions.Player.Ultimate.performed -= OnUltimateUsed;
        
        _inputActions.Player.Disable();
    }

    private static void FixedUpdate()
    {
        if (_moveDirection.x != 0)
            Debug.Log("Player is moving");
            // playerMovement.Move(moveDirection);
    }
    
    private static void OnJumpPressed(InputAction.CallbackContext obj)
    {
        // playerMovement.Jump();
        Debug.Log("Player is jumping");
    }
    
    private static void OnCrouchPressed(InputAction.CallbackContext obj)
    {
        // playerMovement.Crouch();
        Debug.Log("Player is crouching");
    }
    
    private static void OnAttackUsed(InputAction.CallbackContext obj)
    {
        // playerAttack.Attack();
        Debug.Log("Player is attacking");
    }
    
    private static void OnSkillUsed(InputAction.CallbackContext obj)
    {
        // playerAttack.UseSkill();
        Debug.Log("Player is using skill");
    }
    
    private static void OnUltimateUsed(InputAction.CallbackContext obj)
    {
        // playerAttack.UseUlt();
        Debug.Log("Player is using ultimate");
    }
}
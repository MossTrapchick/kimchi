using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    [SerializeField] float Speed, JumpForceMax, JumpForceMin;
    [SerializeField][Range(0f, 1f)] float jumpBuffer;
    private bool isGrounded = false, IsJumping = false;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InputManager.Input.Player.Jump.performed += startJumpWithBuffer;
        InputManager.Input.Player.Jump.canceled += CancelJump;
    }

   
    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        if (InputManager.Input.Player.Move.IsPressed())
        {
            float direction = InputManager.Input.Player.Move.ReadValue<float>();
            rb.linearVelocity = new Vector2(direction * Time.fixedDeltaTime * Speed * 100f, rb.linearVelocityY);
        }
    }
    Coroutine jump;
    void startJumpWithBuffer(InputAction.CallbackContext ctx)
    {
        if (!IsOwner)
        {
            return;
        }
        if (jump!=default) StopCoroutine(jump);
        jump = StartCoroutine(JumpWithBuffer(jumpBuffer));
    }
    IEnumerator JumpWithBuffer(float controlBuffer)
    {
        float time = 0f;
        while (time <= controlBuffer)
        {
            if (isGrounded)
            {
                Jump();
                break;
            }
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
    }
    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * JumpForceMax,ForceMode2D.Impulse);
            IsJumping= true;
        }
    }
    void CancelJump(InputAction.CallbackContext ctx)
    {
        if (!IsOwner)
        {
            return;
        }
        if (IsJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY / JumpForceMin);
            IsJumping = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            IsJumping = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground")) isGrounded = false;
    }
    private void OnDisable()
    {
        InputManager.Input.Player.Jump.performed -= startJumpWithBuffer;
        InputManager.Input.Player.Jump.canceled -= CancelJump;
    }
}

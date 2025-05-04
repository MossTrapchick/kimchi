using System.Collections;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    [SerializeField] float Speed, JumpForceMax, JumpForceMin;
    [SerializeField][Range(0f, 1f)] float jumpBuffer;
    [SerializeField] Transform character;
    [SerializeField] Animator anim;
    private bool isGrounded = false, IsJumping = false;
    Rigidbody2D rb;
    Coroutine stunCor;
    public void Stun(float time, ulong id)
    {
        if (stunCor != default) StopCoroutine(stunCor);
        stunCor = StartCoroutine(stun(time));
    }
    IEnumerator stun(float time)
    {
        anim.SetBool("IsStaned", true);
        if (IsOwner) InputManager.Input.Disable();
        yield return new WaitForSeconds(time);
        anim.SetBool("IsStaned", false);
        if (IsOwner) InputManager.Input.Enable(); //if ! game over
    }
    void Start()
    {
        if (!IsOwner) return;
        if (transform.position.x > 0) DirRpc(-1, OwnerClientId);
        FindFirstObjectByType<CinemachineCamera>().Target.TrackingTarget = transform;
        rb = GetComponent<Rigidbody2D>();
        InputManager.Input.Player.Move.performed += ChangeDirection;
        InputManager.Input.Player.Jump.performed += startJumpWithBuffer;
        InputManager.Input.Player.Jump.canceled += CancelJump;
    }
    void ChangeDirection(InputAction.CallbackContext ctx)
    {
        float direction = InputManager.Input.Player.Move.ReadValue<float>();
        DirRpc(direction < 0 ? -1 : 1, OwnerClientId);
    }
    [Rpc(SendTo.Everyone)]
    void DirRpc(float x, ulong id)
    {
        if (id != OwnerClientId) return;
        character.localScale = new Vector3(x, 1, 1);
    }
   
    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if (InputManager.Input.Player.Move.IsPressed())
        {
            anim.SetBool("IsWalking", true);
            float direction = InputManager.Input.Player.Move.ReadValue<float>();
            rb.linearVelocity = new Vector2(direction * Time.fixedDeltaTime * Speed * 100f, rb.linearVelocityY);
        }
        else anim.SetBool("IsWalking", false);
    }
    Coroutine jump;
    void startJumpWithBuffer(InputAction.CallbackContext ctx)
    {
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
            anim.SetTrigger("Jump");
            rb.AddForce(Vector2.up * JumpForceMax,ForceMode2D.Impulse);
            IsJumping= true;
        }
    }
    void CancelJump(InputAction.CallbackContext ctx)
    {
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
            anim.SetBool("IsGrounded", true);
            isGrounded = true;
            IsJumping = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            anim.SetBool("IsGrounded", false);
            isGrounded = false;
        }
    }
    private void OnDisable()
    {
        if(!IsOwner) return;
        InputManager.Input.Player.Move.performed -= ChangeDirection;
        InputManager.Input.Player.Jump.performed -= startJumpWithBuffer;
        InputManager.Input.Player.Jump.canceled -= CancelJump;
    }
}

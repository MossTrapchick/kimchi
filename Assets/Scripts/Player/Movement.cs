using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float Speed;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (InputManager.Input.Player.Move.IsPressed())
        {
            float direction = InputManager.Input.Player.Move.ReadValue<float>();
            rb.linearVelocity = new Vector2(direction * Time.fixedDeltaTime * Speed * 100f, rb.linearVelocityY);
        }
    }
}

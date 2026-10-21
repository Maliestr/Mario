using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 10f;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    
    public float MoveSpeed => moveSpeed;
    public float JumpForce => jumpForce;
    public bool IsGrounded => isGrounded;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Move(float direction)
    {
        if (rb != null)
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }
    
    public void Jump()
    {
        if (rb != null && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }
    
    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}
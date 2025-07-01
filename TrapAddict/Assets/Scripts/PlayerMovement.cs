using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f; 
    public int maxJumps = 1; 
    private int currentJumps; 

    [Header("Ground Check Settings")]
    public Transform groundCheck; 
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer; 

    [Header("Debug Settings")]
    public bool showGroundCheckGizmo = true; 

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentJumps = maxJumps;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateGroundStatus();
    }

    void HandleMovement()
    {
       
        float moveInput = Input.GetAxisRaw("Horizontal");
       
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
    
        if (Input.GetKeyDown(KeyCode.Space) && currentJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
            currentJumps--; 
        }

        
        if (isGrounded && currentJumps < maxJumps)
        {
            currentJumps = maxJumps;
        }
    }

    void UpdateGroundStatus()
    {
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }


    private void OnDrawGizmos()
    {
        if (showGroundCheckGizmo && groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

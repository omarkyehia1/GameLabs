using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Player movement
    public float moveSpeed = 5f;
    public float jumpHeight = 8f;

    // Keyboard controls
    public KeyCode Spacebar = KeyCode.Space;
    public KeyCode L = KeyCode.LeftArrow;
    public KeyCode R = KeyCode.RightArrow;

    // Ground checking
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    private Animator anim;
   
    private bool grounded;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Movement variables
    private float horizontal;
    private bool jumpRequested;

    void Start()
    {
        anim = GetComponent<Animator>();
       
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Prevent player from rotating
        rb.freezeRotation = true;
    }

    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        anim.SetFloat("Height", GetComponent<Rigidbody2D>().velocity.y);
        anim.SetBool("Grounded", grounded);

        // Horizontal movement
        horizontal = 0f;

        if (Input.GetKey(L))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(R))
        {
            horizontal = 1f;
        }

        // Flip character based on direction
        if (spriteRenderer != null)
        {
            if (horizontal < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (horizontal > 0f)
            {
                spriteRenderer.flipX = false;
            }
        }

        // Capture input here; validate it against the next physics ground check.
        if (Input.GetKeyDown(Spacebar))
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Check if player is touching ground
        if (groundCheck != null)
        {
            // Allow tiny upward velocities produced by resting collision contacts.
            grounded = rb.velocity.y <= 0.1f && Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                whatIsGround
            );
        }
        else
        {
            grounded = false;
        }

        // Move player horizontally
        rb.velocity = new Vector2(
            horizontal * moveSpeed,
            rb.velocity.y
        );

        // Perform jump
        if (jumpRequested && grounded)
        {
            Jump();
            grounded = false;
        }
        jumpRequested = false;
    }

    void OnDisable()
    {
        horizontal = 0f;
        jumpRequested = false;
        grounded = false;
    }

    void Jump()
    {
        rb.velocity = new Vector2(
            rb.velocity.x,
            jumpHeight
        );
    }

    // Visualize ground check in Scene view
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}

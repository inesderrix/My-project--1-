using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    [Header("Paramètres de mouvement")]
    [SerializeField] private float vitesse = 5f;

    private const string MOVE_ACTION = "Move";

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float dernierHorizontal = 0f;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private PlayerDash playerDash;

    void Start()
    {
        // Initialize input to zero
        moveInput = Vector2.zero;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction(MOVE_ACTION);
        playerDash = GetComponent<PlayerDash>();
    }

    void Update()
    {
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }

        if (moveInput.magnitude > 0.1f)
        {
            // Store last direction for attack animations
            lastMoveDirection = moveInput.normalized;
            DeplacerPersonnage();
            UpdateDirectionalAnimations();
        }
        else
        {
            UpdateIdleAnimation();
        }
        
        FlipSprite();
    }

    void DeplacerPersonnage()
    {
        // Skip if dashing
        if (playerDash != null && playerDash.IsDashing())
        {
            return;
        }
        
        // Calculate move direction
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);

        if (moveInput.x != 0)
        {
            dernierHorizontal = moveInput.x;
        }

        // Apply movement
        transform.position += direction * vitesse * Time.deltaTime;

        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }
    }

    void FlipSprite()
    {
        // Flip based on direction
        if (spriteRenderer != null && dernierHorizontal != 0)
        {
            spriteRenderer.flipX = dernierHorizontal < 0;
        }
    }
    
    void UpdateDirectionalAnimations()
    {
        if (animator == null) return;
        
        // Determine direction
        float absX = Mathf.Abs(moveInput.x);
        float absY = Mathf.Abs(moveInput.y);
        
        // Send Direction parameter to animator
        // 0 = down (run-vert), 1 = up (run-vert-up), 2 = horizontal (run-hor)
        if (absY > absX)
        {
            if (moveInput.y > 0)
            {
                animator.SetInteger("Direction", 1);
            }
            else
            {
                animator.SetInteger("Direction", 0);
            }
        }
        else
        {
            animator.SetInteger("Direction", 2);
        }
        
        animator.SetBool("isMoving", true);
    }
    
    void UpdateIdleAnimation()
    {
        if (animator == null) return;
        
        animator.SetBool("isMoving", false);
    }
    
    public Vector2 GetFacingDirection()
    {
        return lastMoveDirection.magnitude > 0.1f ? lastMoveDirection : new Vector2(dernierHorizontal, 0);
    }
}
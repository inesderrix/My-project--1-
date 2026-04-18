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

    private PlayerInput playerInput;
    private InputAction moveAction;
    private PlayerDash playerDash;

    void Start()
    {
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

        // Move character
        DeplacerPersonnage();
        // Flip sprite direction
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
            bool isMoving = moveInput.magnitude > 0.1f;
            animator.SetBool("isMoving", isMoving);
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
}
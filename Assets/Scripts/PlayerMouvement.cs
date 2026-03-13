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

    }

    void Update()
    {
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }

        DeplacerPersonnage();
        FlipSprite();
    }

    void DeplacerPersonnage()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);

        if (moveInput.x != 0)
        {
            dernierHorizontal = moveInput.x;
        }

        transform.position += direction * vitesse * Time.deltaTime;

        if (animator != null)
        {
            bool isMoving = moveInput.magnitude > 0.1f;
            animator.SetBool("isMoving", isMoving);
        }
    }

    void FlipSprite()
    {
        if (spriteRenderer != null && dernierHorizontal != 0)
        {
            spriteRenderer.flipX = dernierHorizontal < 0;
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    [Header("Paramètres de mouvement")]
    [SerializeField] private float vitesse = 5f;

    // Noms des actions (en constantes)
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

        // Récupère le Player Input
        playerInput = GetComponent<PlayerInput>();

        // Cherche l'action "Move" avec la constante
        moveAction = playerInput.actions.FindAction(MOVE_ACTION);

        if (moveAction == null)
        {
            Debug.LogError($"Action '{MOVE_ACTION}' introuvable dans Input Actions !");
        }
        else
        {
            Debug.Log($"✅ Action '{MOVE_ACTION}' trouvée et liée !");
        }
    }

    void Update()
    {
        // Lit la valeur de l'action directement
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
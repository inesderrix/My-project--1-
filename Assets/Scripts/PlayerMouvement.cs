using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    [Header("Paramètres de mouvement")]
    [SerializeField] private float vitesse = 5f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float dernierHorizontal = 0f;

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
    }

    void Update()
    {
        DeplacerPersonnage();
        FlipSprite();
    }

    void DeplacerPersonnage()
    {
        
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                vertical = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                vertical = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontal = 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontal = -1;
        }

    
        Vector3 direction = new Vector2(horizontal, vertical).normalized;

    
        if (horizontal != 0)
        {
            dernierHorizontal = horizontal;
        }

     
        transform.position += direction * vitesse * Time.deltaTime;

        if (animator != null)
        {
            bool isMoving = direction.magnitude > 0.1f;
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
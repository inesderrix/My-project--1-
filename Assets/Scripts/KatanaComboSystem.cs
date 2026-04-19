using UnityEngine;
using UnityEngine.InputSystem;

public class KatanaComboSystem : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private float comboWindow = 0.8f;
    [SerializeField] private float portee = 1.5f;

    private Animator anim;
    private PlayerInput input;
    private InputAction attackAction;
    private int comboStep = 0;
    private float lastClickTime;
    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
        attackAction = input.actions.FindAction("Attack");
    }

    void Update()
    {
        // Check attack input
        if (attackAction.WasPressedThisFrame())
        {
            OnAttackClicked();
        }

        // Check combo timeout
        if (isAttacking && Time.time - lastClickTime > comboWindow)
        {
            ResetCombo();
        }
    }

    void OnAttackClicked()
    {
        // Record attack time
        lastClickTime = Time.time;
        
        // Get attack direction first to check if combo allowed
        Vector2 attackDir = GetAttackDirection();
        bool isVertical = Mathf.Abs(attackDir.y) > Mathf.Abs(attackDir.x);

        if (!isAttacking)
        {
            isAttacking = true;
            comboStep = 0;
            PlayAttack();
        }
        // Continue combo (only for horizontal attacks)
        else if (Time.time - lastClickTime < comboWindow && !isVertical)
        {
            comboStep++;
            if (comboStep > 2) comboStep = 0;
            PlayAttack();
        }
    }

    void PlayAttack()
    {
        anim.SetBool("isAttacking", true);
        anim.SetInteger("ComboAttack", comboStep);
        
        Vector2 attackDirection = GetAttackDirection();

        string animName = GetAttackAnimationName(attackDirection, comboStep);
        anim.Play(animName, 0, 0f);

        DetecterEnnemisAvecDirection(attackDirection);
    }
    
    Vector2 GetAttackDirection()
    {
        PlayerMouvement movement = GetComponent<PlayerMouvement>();
        if (movement != null)
        {
            return movement.GetFacingDirection();
        }
        
        // Default to facing right
        return Vector2.right;
    }
    
    string GetAttackAnimationName(Vector2 direction, int combo)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);
        
        if (absY > absX)
        {
            // Vertical attacks - no combo
            if (direction.y > 0)
            {
                return "vert-katana-up";
            }
            else
            {
                return "vert-katana";
            }
        }
        else
        {
            // Horizontal attack - full combo
            string suffix = (combo + 1).ToString();
            return "katana" + suffix;
        }
    }

    void DetecterEnnemisAvecDirection(Vector2 attackDir)
    {
        // Find enemies in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, portee);
        Transform closestEnemyTransform = null;
        float closestDistance = float.MaxValue;

        // Check all colliders
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // Check if enemy is in attack direction
                Vector2 toEnemy = (hit.transform.position - transform.position).normalized;
                float dotProduct = Vector2.Dot(toEnemy, attackDir);
                
                // Only hit enemies in front (dot > 0.5 = within 60 degrees)
                if (dotProduct > 0.5f)
                {
                    float distance = Vector2.Distance(transform.position, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemyTransform = hit.transform;
                    }
                }
            }
        }

        if (closestEnemyTransform != null)
        {
            var enemyAI = closestEnemyTransform.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.Mourir();
            }
        }
    }

    void ResetCombo()
    {
        // Reset combo state
        isAttacking = false;
        comboStep = 0;
        anim.SetBool("isAttacking", false);
        anim.SetInteger("ComboAttack", 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, portee);
    }
}
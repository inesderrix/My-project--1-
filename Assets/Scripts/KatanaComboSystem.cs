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
        if (attackAction.WasPressedThisFrame())
        {
            OnAttackClicked();
        }

        if (isAttacking && Time.time - lastClickTime > comboWindow)
        {
            ResetCombo();
        }
    }

    void OnAttackClicked()
    {
        lastClickTime = Time.time;

        if (!isAttacking)
        {
            isAttacking = true;
            comboStep = 0;
            PlayAttack();
        }
        else if (Time.time - lastClickTime < comboWindow)
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

        anim.Play("katana" + (comboStep + 1), 0, 0f);

        DetecterEnnemis();
    }

    void DetecterEnnemis()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, portee);
        Transform closestEnemyTransform = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemyTransform = hit.transform;
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
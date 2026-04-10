using UnityEngine;
using UnityEngine.InputSystem;

public class KatanaComboSystem : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private float comboWindow = 0.8f; // Temps pour enchaîner
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
        // 1. Détection du clic
        if (attackAction.WasPressedThisFrame())
        {
            OnAttackClicked();
        }

        // 2. Reset automatique si on attend trop longtemps
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
            // Premier coup
            isAttacking = true;
            comboStep = 0;
            PlayAttack();
        }
        else if (Time.time - lastClickTime < comboWindow)
        {
            // Enchaînement (Combo)
            comboStep++;
            if (comboStep > 2) comboStep = 0; // On boucle le combo 1-2-3
            PlayAttack();
        }
    }

    void PlayAttack()
    {
        // On force les paramètres de TON animator
        anim.SetBool("isAttacking", true);
        anim.SetInteger("ComboAttack", comboStep);

        // On force le lancement de l'anim pour éviter les transitions bloquées
        anim.Play("katana" + (comboStep + 1), 0, 0f);

        // On lance la détection de dégâts direct
        DetecterEnnemis();
    }

    void DetecterEnnemis()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, portee);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // On essaie de trouver le script peu importe l'orthographe
                var target = hit.GetComponent<MonoBehaviour>();
                target.Invoke("Mourir", 0);
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
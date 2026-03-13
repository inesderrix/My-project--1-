using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class KatanaComboSystem : MonoBehaviour
{
    [Header("Paramètres du Combo")]
    [SerializeField] private float comboTimeWindow = 1f;
    [SerializeField] private float attackDuration = 0.5f;

    private const string ATTACK_ACTION = "Attack";

    private Animator animator;
    private int comboStep = 0;
    private float lastAttackTime = -999f;
    private bool isAttacking = false;
    private bool canCombo = false;

    private PlayerInput playerInput;
    private InputAction attackAction;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions.FindAction(ATTACK_ACTION);
    }

    void Update()
    {
        if (attackAction != null && attackAction.WasPressedThisFrame())
        {
            if (canCombo)
            {
                comboStep++;
                canCombo = false;
                PerformAttack();
            }
            else if (!isAttacking)
            {
                comboStep = 0;
                PerformAttack();
            }
        }

        if (Time.time - lastAttackTime > comboTimeWindow && isAttacking)
        {
            EndCombo();
        }
    }

    void PerformAttack()
    {
        if (comboStep > 2)
        {
            EndCombo();
            return;
        }

        animator.SetInteger("ComboAttack", comboStep);
        animator.SetBool("isAttacking", true);

        switch (comboStep)
        {
            case 0:
                animator.Play("katana1", 0, 0f);
                break;
            case 1:
                animator.Play("katana2", 0, 0f);
                break;
            case 2:
                animator.Play("katana3", 0, 0f);
                break;
        }

        lastAttackTime = Time.time;
        isAttacking = true;

        StopAllCoroutines();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackDuration * 0.6f);
        canCombo = true;

        yield return new WaitForSeconds(attackDuration * 0.4f);
        canCombo = false;

        if (Time.time - lastAttackTime >= comboTimeWindow)
        {
            EndCombo();
        }
    }

    void EndCombo()
    {
        isAttacking = false;
        canCombo = false;
        comboStep = 0;
        animator.SetBool("isAttacking", false);
        animator.SetInteger("ComboAttack", 0);
    }

    public void ForceResetCombo()
    {
        StopAllCoroutines();
        EndCombo();
    }
}
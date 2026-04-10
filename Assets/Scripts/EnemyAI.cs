using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Poursuite")]
    [SerializeField] private float vitesse = 3f;
    [SerializeField] private float distanceDetection = 5f;
    [SerializeField] private float distanceAttaque = 1f;
    
    [Header("Combat")]
    [SerializeField] private bool peutAttaquer = true;
    [SerializeField] private float cooldownAttaque = 1f;
    
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private bool isDead = false;
    private float derniereAttaque = 0f;
    private bool estProche = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (player == null)
        {
            Debug.LogError("bug player pas trouve");
        }
    }
    
    void Update()
    {
        if (isDead || player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        estProche = distance <= distanceDetection;
        
        if (estProche)
        {
            PoursuivreJoueur(distance);
        }
        else
        {
            ArretMouvement();
        }
    }
    
    void PoursuivreJoueur(float distance)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        
        if (distance > distanceAttaque)
        {
            rb.linearVelocity = direction * vitesse;
            
            if (animator != null)
            {
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking", false);
            }
            
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
        else
        {
            ArretMouvement();
            
            if (peutAttaquer && Time.time > derniereAttaque + cooldownAttaque)
            {
                Attaquer();
            }
        }
    }
    
    void ArretMouvement()
    {
        rb.linearVelocity = Vector2.zero;
        
        if (animator != null && !isDead)
        {
            animator.SetBool("isMoving", false);
        }
    }
    
    void Attaquer()
    {
        derniereAttaque = Time.time;
        
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
            
            int attackType = Random.Range(1, 4);
            animator.SetInteger("ComboAttack", attackType);
            animator.Play("katana" + attackType, 0, 0f);
            
            Invoke("DetecterJoueur", 0.3f);
            
            Invoke("ResetAttaque", 0.8f);
        }
    }
    
    void DetecterJoueur()
    {
        if (isDead) return;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, distanceAttaque);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Mourir();
                }
                else
                {
                    hit.SendMessage("Mourir", SendMessageOptions.DontRequireReceiver);
                }
                break;
            }
        }
    }
    
    void ResetAttaque()
    {
        if (animator != null)
        {
            animator.SetBool("isAttacking", false);
            animator.SetInteger("ComboAttack", 0);
        }
    }
    
    public void Mourir()
    {
        if (isDead) return;
        
        isDead = true;
        ArretMouvement();
        
        if (animator != null)
        {
            animator.SetBool("isDead", true);
            animator.Play("Death", 0, 0f);
        }
        
        var colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        Destroy(gameObject, 2f);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceDetection);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceAttaque);
    }
}

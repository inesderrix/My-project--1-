using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Poursuite")]
    [SerializeField] private float vitesse = 3f;
    [SerializeField] private float distanceDetection = 999f;
    
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool aToucheJoueur = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
        
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (player == null)
        {
            Debug.LogError("pas de player");
        }
        
    }
    
    void Update()
    {
        if (player == null || aToucheJoueur) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance <= distanceDetection)
        {
            PoursuivreJoueur();
        }
        else
        {
            ArretMouvement();
        }
    }
    
    void PoursuivreJoueur()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * vitesse;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }
    
    void ArretMouvement()
    {
        rb.linearVelocity = Vector2.zero;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            aToucheJoueur = true;
            ArretMouvement();
            
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Mourir();
            }
        }
    }
    
    
    public void Mourir()
    {
        if (aToucheJoueur) return;
        
        aToucheJoueur = true;
        ArretMouvement();
        
        Debug.Log("mort");
        
        var colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        Destroy(gameObject, 1f);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceDetection);
    }
}

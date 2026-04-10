using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private bool isDead = false;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMouvement playerMouvement;
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMouvement = GetComponent<PlayerMouvement>();
    }
    
    public void Mourir()
    {
        if (isDead) return;
        
        isDead = true;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
        
        if (playerMouvement != null)
        {
            playerMouvement.enabled = false;
        }
        
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
        
        Debug.Log("Le joueur est mort!");
        
        Destroy(gameObject, 2f);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Mourir();
        }
    }
}

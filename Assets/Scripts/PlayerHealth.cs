using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
        
        // Stop physics movement
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
        
        if (playerMouvement != null)
        {
            playerMouvement.enabled = false;
        }
        
        // Play death animation
        if (animator != null)
        {
            animator.SetBool("isDead", true);
            //create animation death
        }
        
        var colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        if (GameManager.Instance != null)
        {
            StartCoroutine(DelayedDefeat());
        }
        
        Destroy(gameObject, 2f);
    }
    
    IEnumerator DelayedDefeat()
    {
        // Wait for animation if we had one (I leave if ever)
        yield return new WaitForSeconds(1.5f);
        // Call game over
        GameManager.Instance.OnPlayerDeath();
    }
    
    void Update()
    {
       
    }
}

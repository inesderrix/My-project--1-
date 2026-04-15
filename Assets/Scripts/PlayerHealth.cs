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
        
        if (GameManager.Instance != null)
        {
            StartCoroutine(DelayedDefeat());
        }
        
        Destroy(gameObject, 2f);
    }
    
    IEnumerator DelayedDefeat()
    {
        yield return new WaitForSeconds(1.5f); // Délai de 1.5 secondes
        GameManager.Instance.OnPlayerDeath();
    }
    
    void Update()
    {
       
    }
}

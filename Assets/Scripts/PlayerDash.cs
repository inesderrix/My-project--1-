using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [Header("Paramètres du Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private bool isDashing = false;
    private float lastDashTime = 0f;
    private Vector2 dashDirection;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }
    }
    
    void Update()
    {
        if (playerInput != null && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            if (playerInput.actions["ButtonU"].WasPressedThisFrame())
            {
                StartDash();
            }
        }
    }
    
    void StartDash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        
        Vector2 moveInput = Vector2.zero;
        if (playerInput != null)
        {
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        }
        
        if (moveInput.magnitude < 0.1f)
        {
            isDashing = false;
            return;
        }
        
        dashDirection = moveInput.normalized;
        
        rb.linearVelocity = dashDirection * dashSpeed;
        
        Invoke("StopDash", dashDuration);
    }
    
    void StopDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector2.zero;
    }
    
    public bool IsDashing()
    {
        return isDashing;
    }
}

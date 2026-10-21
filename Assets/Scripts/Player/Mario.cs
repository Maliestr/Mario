using UnityEngine;

public class Mario : Player
{
    [Header("Component References")]
    [SerializeField] private MovementHandler movementHandler;
    [SerializeField] private AnimationHandler animationHandler;
    [SerializeField] private AttackHandler attackHandler;
    
    [Header("PowerUp States")]
    private bool isBig = false;
    private bool hasFireballs = false;
    
    [Header("PowerUp Settings")]
    private Vector2 normalScale = Vector2.one;
    private Vector2 bigScale = new Vector2(1.5f, 1.5f);
    private BoxCollider2D boxCollider;
    private Vector2 normalColliderSize = new Vector2(0.6f, 0.8f);
    private Vector2 bigColliderSize = new Vector2(0.8f, 1.2f);
    private Vector2 normalColliderOffset = new Vector2(0, 0);
    private Vector2 bigColliderOffset = new Vector2(0, 0.2f);
    
    [Header("Fireball Settings")]
    private GameObject fireballPrefab;
    private float fireballCooldown = 0.5f;
    private float fireballTimer = 0f;
    
    [Header("Death")]
    [SerializeField] private GameObject gameOverScreen;
    
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private bool isGrounded;
    
    // Публичные свойства для доступа из других классов
    public bool IsBig => isBig;
    public bool HasFireballs => hasFireballs;
    public GameObject GameOverScreen => gameOverScreen;
    
    protected override void Start()
    {
        base.Start();
        
        // Автоматический поиск компонентов, если не назначены в Inspector
        if (movementHandler == null) movementHandler = GetComponent<MovementHandler>();
        if (animationHandler == null) animationHandler = GetComponent<AnimationHandler>();
        if (attackHandler == null) attackHandler = GetComponent<AttackHandler>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        
        normalScale = transform.localScale;
        if (boxCollider != null)
        {
            normalColliderSize = boxCollider.size;
            normalColliderOffset = boxCollider.offset;
        }
        
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }
    
    private void Update()
    {
        if (isDead) return;
        
        // ===== ДВИЖЕНИЕ =====
        float move = Input.GetAxis("Horizontal");
        movementHandler?.Move(move);
        isGrounded = movementHandler?.IsGrounded ?? false;
        
        // ===== АНИМАЦИИ =====
        animationHandler?.UpdateMovement(Mathf.Abs(move) > 0.1f, isGrounded, rb.velocity.y);
        
        // ===== ПОВОРОТ СПРАЙТА =====
        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
        // ===== ПРЫЖОК =====
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        
        // ===== АТАКА МЕЧОМ =====
        if (Input.GetKeyDown(KeyCode.Q))
        {
            attackHandler?.TryAttack();
        }
        
        // ===== ОГНЕННЫЕ ШАРЫ =====
        if (hasFireballs && Input.GetKeyDown(KeyCode.E) && !isDead)
        {
            ShootFireball();
        }
        
        // ===== ТАЙМЕР ОГНЕННЫХ ШАРОВ =====
        if (fireballTimer > 0f)
            fireballTimer -= Time.deltaTime;
    }
    
    // ========== ВЫЗОВЫ ИЗ ANIMATION EVENTS ==========
    public void PerformSwordHit()
    {
        attackHandler?.ApplySwordHit();
    }
    
    public void EndAttack()
    {
        // Анимация атаки закончилась (можно добавить логику при необходимости)
    }
    
    // ========== ОГНЕННЫЕ ШАРЫ ==========
    public void EnableFireballs(GameObject prefab, float cooldown)
    {
        hasFireballs = true;
        fireballPrefab = prefab;
        fireballCooldown = cooldown;
        Debug.Log($"Fireballs ENABLED! hasFireballs = {hasFireballs}");
    }
    
    private void ShootFireball()
    {
        if (fireballPrefab == null)
        {
            Debug.LogError("fireballPrefab is NULL!");
            return;
        }
        
        if (fireballTimer > 0f) return;
        
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 spawnPos = (Vector2)transform.position + direction * 0.8f + Vector2.up * 0.3f;
        
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Fireball fb = fireball.GetComponent<Fireball>();
        if (fb != null) fb.Initialize(direction, 12f);
        
        fireballTimer = fireballCooldown;
    }
    
    // ========== УСИЛЕНИЯ ==========
    public void BecomeBig(Vector2 bigScaleValue, Vector2 normalScaleValue, int extraHealth)
    {
        isBig = true;
        AddHealth(extraHealth);
        transform.localScale = bigScaleValue;
        
        if (boxCollider != null)
        {
            boxCollider.size = bigColliderSize;
            boxCollider.offset = bigColliderOffset;
        }
        
        animationHandler?.SetBig(true);
        Debug.Log($"Mario became BIG! Health: {GetHealth()}");
    }
    
    public void Shrink()
    {
        if (!isBig) return;
        
        isBig = false;
        hasFireballs = false;
        transform.localScale = normalScale;
        
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
        
        if (boxCollider != null)
        {
            boxCollider.size = normalColliderSize;
            boxCollider.offset = normalColliderOffset;
        }
        
        animationHandler?.SetBig(false);
        Debug.Log("Mario shrunk back to normal size!");
    }
    
    // ========== ПРОВЕРКА ЗЕМЛИ ==========
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            movementHandler?.SetGrounded(true);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            movementHandler?.SetGrounded(false);
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            movementHandler?.SetGrounded(true);
        }
    }
    
    // ========== ПРЫЖОК ==========
    public override void Jump()
    {
        movementHandler?.Jump();
        isGrounded = false;
    }
    
    // ========== УРОН И СМЕРТЬ ==========
    public override void TakeDamage(int damage)
    {
        if (isDead) return;
        
        if (isBig && GetHealth() <= 2)
        {
            Shrink();
            damage = 1;
        }
        
        base.TakeDamage(damage);
        
        if (GetHealth() > 0)
        {
            animationHandler?.TriggerHurt();
            rb.velocity = new Vector2(rb.velocity.x, 5f);
        }
        else
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        animationHandler?.TriggerDeath();
        
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
        
        GameManager.Instance?.Die();
        enabled = false;
    }
}
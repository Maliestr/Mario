using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int swordDamage = 1;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Vector2 attackOffset = new Vector2(0.6f, 0f);
    [SerializeField] private Vector2 attackSize = new Vector2(0.8f, 0.6f);
    
    [Header("Combo Settings")]
    [SerializeField] private float comboResetTime = 1f;
    
    private int comboStep = 0;
    private float comboTimer = 0f;
    private float cooldownTimer = 0f;
    private bool canAttack = true;
    private Transform playerTransform;
    private AnimationHandler animationHandler;
    
    public int SwordDamage => swordDamage;
    public bool CanAttack => canAttack;
    
    private void Start()
    {
        playerTransform = transform;
        animationHandler = GetComponent<AnimationHandler>();
    }
    
    private void Update()
    {
        // Перезарядка
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                canAttack = true;
        }
        
        // Сброс комбо
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
                comboStep = 0;
        }
    }
    
    public bool TryAttack()
    {
        if (!canAttack) return false;
        
        canAttack = false;
        cooldownTimer = attackCooldown;
        comboTimer = comboResetTime;
        
        animationHandler?.TriggerAttack(comboStep);
        
        comboStep = (comboStep + 1) % 3;
        return true;
    }
    
    public void ApplySwordHit()
    {
        float direction = playerTransform.localScale.x;
        Vector2 attackPos = (Vector2)playerTransform.position + 
                            new Vector2(attackOffset.x * direction, attackOffset.y);
        
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPos, attackSize, 0);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Enemy e = enemy.GetComponent<Enemy>();
                if (e != null)
                    e.TakeDamage(swordDamage);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;
        float direction = transform.localScale.x;
        Vector2 pos = (Vector2)playerTransform.position + 
                      new Vector2(attackOffset.x * direction, attackOffset.y);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos, attackSize);
    }
}
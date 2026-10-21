using UnityEngine;

public abstract class PowerUp : MonoBehaviour, ICollectible
{
    [Header("PowerUp Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float lifeTime = 10f;
    [SerializeField] protected bool moveLeft = false;
    
    protected bool isCollected = false;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        float direction = moveLeft ? -1f : 1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        
        Destroy(gameObject, lifeTime);
    }
    
    public virtual void Collect(Player player)
    {
        if (isCollected) return;
        isCollected = true;
        
        ApplyEffect(player);
        
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
        
        Destroy(gameObject, 0.2f);
    }
    
    protected abstract void ApplyEffect(Player player);
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            Collect(player);
        }
    }
}
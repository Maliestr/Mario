using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected int health = 1;
    protected int direction = -1;
    protected bool isAlive = true;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (rb != null)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    protected virtual void Update()
    {
        Move();
    }

    public abstract void Move();
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! Health: {health}");
        
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Mario player = collision.gameObject.GetComponent<Mario>();
            if (player != null)
            {
                // Только урон игроку (НЕ убиваем врага прыжком)
                player.TakeDamage(1);
                
                // Отбрасывание игрока
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 5f);
                }
            }
        }
    }
}
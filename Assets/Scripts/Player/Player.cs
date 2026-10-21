using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 7f;
    [SerializeField] protected float jumpForce = 10f;
    protected int health = 3;
    protected bool isGrounded;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public abstract void Jump();
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }
    
    protected virtual void Die()
    {
        Debug.Log("Player died");
    }
    
    // НОВЫЕ МЕТОДЫ ДЛЯ ДОСТУПА К HEALTH
    public virtual void AddHealth(int amount)
    {
        health += amount;
        Debug.Log($"Health increased to {health}");
    }
    
    public virtual int GetHealth()
    {
        return health;
    }
    
    public virtual void SetHealth(int value)
    {
        health = value;
    }
}
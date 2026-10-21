using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Fireball Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private ParticleSystem hitEffect;
    
    private Vector2 direction;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Destroy(gameObject, lifeTime);
    }
    
    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
    
    public void Initialize(Vector2 dir, float spd = -1)
    {
        direction = dir.normalized;
        if (spd > 0) speed = spd;
        
        // Поворачиваем спрайт в направлении полёта
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Fireball hit {other.name}!");
            }
            
            // Эффект попадания
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Попадание в стену/землю
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
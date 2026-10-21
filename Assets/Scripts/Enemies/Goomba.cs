using UnityEngine;

public class Goomba : Enemy
{
    [SerializeField] private Transform leftCheck;
    [SerializeField] private Transform rightCheck;
    [SerializeField] private LayerMask groundLayer;
    
    private bool isMovingRight = false;
    
    protected override void Start()
    {
        base.Start();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Автоматическое создание точек проверки
        if (leftCheck == null) leftCheck = CreateCheckPoint("LeftCheck", -0.5f);
        if (rightCheck == null) rightCheck = CreateCheckPoint("RightCheck", 0.5f);
    }
    
    private Transform CreateCheckPoint(string name, float xOffset)
    {
        GameObject check = new GameObject(name);
        check.transform.parent = transform;
        check.transform.localPosition = new Vector3(xOffset, -0.5f, 0);
        return check.transform;
    }
    
    public override void Move()
    {
        if (!isAlive) return;
        
        // Движение
        float direction = isMovingRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        
        // Проверка на разворот
        Transform activeCheck = isMovingRight ? rightCheck : leftCheck;
        RaycastHit2D hit = Physics2D.Raycast(activeCheck.position, Vector2.down, 0.2f, groundLayer);
        
        // Если земли нет впереди — развернуться
        if (hit.collider == null)
        {
            isMovingRight = !isMovingRight;
            
            // Поворот спрайта
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = isMovingRight;
            }
            
            Debug.Log($"Goomba flipped! Moving right: {isMovingRight}");
        }
        
        // Визуализация
        Debug.DrawRay(activeCheck.position, Vector2.down * 0.2f, hit.collider != null ? Color.green : Color.red);
    }
}
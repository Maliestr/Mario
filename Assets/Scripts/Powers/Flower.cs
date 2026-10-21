using UnityEngine;

public class Flower : PowerUp
{
    [Header("Flower Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireCooldown = 0.5f;
    [SerializeField] private Color flowerColor = new Color(1f, 0.5f, 0.2f);
    
    protected override void ApplyEffect(Player player)
    {
        Debug.Log("=== FLOWER COLLECTED ===");
        
        Mario mario = player as Mario;
        if (mario != null)
        {
            if (fireballPrefab == null)
            {
                Debug.LogError("Fireball Prefab is not assigned in Flower inspector!");
                return;
            }
            
            // Используем существующий метод EnableFireballs
            mario.EnableFireballs(fireballPrefab, fireCooldown);
            
            // Меняем цвет
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = flowerColor;
            }
        }
    }
}
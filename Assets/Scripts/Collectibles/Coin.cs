using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [Header("Coin Settings")]
    [SerializeField] private int pointValue = 10;
    [SerializeField] private float collectDelay = 0.1f;
    [SerializeField] private GameObject collectEffect;   // Эффект при сборе (опционально)
    
    private bool isCollected = false;
    
    public void Collect(Player player)
    {
        if (isCollected) return;
        isCollected = true;
        
        // Добавляем очки
        UIManager.Instance?.AddPoints(pointValue);
        
        // Воспроизводим звук (если есть)
        // AudioManager.Instance?.PlayCoinSound();
        
        // Эффект сбора
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        
        // Визуальный эффект: монетка исчезает
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        
        // Уничтожаем объект
        Destroy(gameObject, collectDelay);
        
        Debug.Log($"Coin collected! +{pointValue} points");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            Collect(player);
        }
    }
}
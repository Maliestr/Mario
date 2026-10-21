using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    [Header("Goal Settings")]
    public int nextLevelIndex = -1; // -1 = следующий по порядку
    public GameObject flagTop; // Верхушка флага (для анимации)
    public ParticleSystem flagEffect; // Эффект при завершении
    
    [Header("Animation")]
    public float slideSpeed = 3f;
    public float slideDistance = 2f;
    
    private bool isActivated = false;
    private Vector3 originalFlagPos;
    private GameObject player;
    
    private void Start()
    {
        if (flagTop != null)
            originalFlagPos = flagTop.transform.localPosition;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;
        
        if (other.CompareTag("Player"))
        {
            ActivateGoal();
        }
    }
    
    private void ActivateGoal()
    {
        isActivated = true;
        player = GameObject.FindGameObjectWithTag("Player");
        
        Debug.Log("Goal reached!");
        
        // Отключаем управление игроком
        if (player != null)
        {
            player.GetComponent<Mario>().enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        
        // Анимация флага (флаг опускается)
        StartCoroutine(FlagSlideAnimation());
        
        // Эффект
        if (flagEffect != null)
            flagEffect.Play();
        
        // Завершаем уровень
        Invoke("CompleteLevel", 1.5f);
    }
    
    private System.Collections.IEnumerator FlagSlideAnimation()
    {
        if (flagTop == null) yield break;
        
        Vector3 targetPos = originalFlagPos + Vector3.down * slideDistance;
        float t = 0;
        
        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;
            flagTop.transform.localPosition = Vector3.Lerp(originalFlagPos, targetPos, t);
            yield return null;
        }
    }
    
    private void CompleteLevel()
    {
        if (nextLevelIndex >= 0)
            LevelManager.Instance?.LoadLevel(nextLevelIndex);
        else
            LevelManager.Instance?.CompleteLevel();
    }
}
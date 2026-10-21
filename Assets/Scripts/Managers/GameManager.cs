using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Player Lives")]
    public int maxLives = 3;
    public int currentLives;
    public bool isGameOver = false;
    
    [Header("UI")]
    public GameObject gameOverPanel;
    
    private string currentSceneName;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        currentLives = maxLives;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateLivesUI(currentLives);
    }
    
    public void Die()
    {
        if (isGameOver) return;
        
        currentLives--;
        
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateLivesUI(currentLives);
        
        Debug.Log($"Player died! Lives left: {currentLives}");
        
        if (currentLives <= 0)
        {
            GameOver();
        }
        // Респавна НЕТ — игра просто продолжается с текущим игроком
    }
    
    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Debug.Log("GAME OVER!");
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        isGameOver = false;
        currentLives = maxLives;
        Time.timeScale = 1f;
        
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateLivesUI(currentLives);
        
        SceneManager.LoadScene(currentSceneName);
    }
}
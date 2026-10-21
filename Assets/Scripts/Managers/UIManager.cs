using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private int currentScore = 0;
    
    [Header("Score UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private string scorePrefix = "Score: ";
    
    [Header("Lives UI")]
    [SerializeField] private Text livesText;
    [SerializeField] private string livesPrefix = "Lives: ";
    [SerializeField] private GameObject[] heartIcons;
    
    [Header("Timer UI")]
    [SerializeField] private Text timerText;
    [SerializeField] private float levelTime = 300f;
    private float currentTime;
    private bool timerActive = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        currentTime = levelTime;
        timerActive = true;
        UpdateScoreUI(0);
        UpdateLivesUI(3);
    }
    
    private void Update()
    {
        if (timerActive && timerText != null && !GameManager.Instance.isGameOver)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                timerActive = false;
                GameManager.Instance?.GameOver();
            }
            
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{scorePrefix}{score}";
        }
    }
    
    public void UpdateLivesUI(int lives)
    {
        // Текстовый вариант
        if (livesText != null)
        {
            livesText.text = $"{livesPrefix}{lives}";
        }
        
        // Вариант с сердечками
        if (heartIcons != null && heartIcons.Length > 0)
        {
            for (int i = 0; i < heartIcons.Length; i++)
            {
                if (heartIcons[i] != null)
                {
                    heartIcons[i].SetActive(i < lives);
                }
            }
        }
    }
    
    public void ShowMessage(string message, float duration = 2f)
    {
        Debug.Log($"[UI] {message}");
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreUI(currentScore);
    }
}
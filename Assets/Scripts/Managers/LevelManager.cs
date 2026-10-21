using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Header("Level Settings")]
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private string[] levelSceneNames;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    [Header("Level Completion")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private float levelCompleteDelay = 1f;
    
    // Публичные свойства для чтения
    public int CurrentLevelIndex => currentLevelIndex;
    public string[] LevelSceneNames => levelSceneNames;
    
    private bool isLoading = false;
    
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
        }
    }
    
    private void Start()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }
    
    public void CompleteLevel()
    {
        Debug.Log($"Level {currentLevelIndex + 1} completed!");
        
        // Сохраняем прогресс (можно в PlayerPrefs)
        SaveProgress();
        
        // Показываем панель завершения
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            Invoke("LoadNextLevel", levelCompleteDelay);
        }
        else
        {
            LoadNextLevel();
        }
    }
    
    private void LoadNextLevel()
    {
        if (isLoading) return;
        isLoading = true;
        
        int nextLevelIndex = currentLevelIndex + 1;
        
        if (nextLevelIndex < levelSceneNames.Length)
        {
            currentLevelIndex = nextLevelIndex;
            SceneManager.LoadScene(levelSceneNames[currentLevelIndex]);
        }
        else
        {
            Debug.Log("All levels completed! Game finished!");
            // Загружаем экран победы или главное меню
            SceneManager.LoadScene(mainMenuSceneName);
        }
        
        isLoading = false;
    }
    
    public void RestartCurrentLevel()
    {
        if (isLoading) return;
        isLoading = true;
        
        SceneManager.LoadScene(levelSceneNames[currentLevelIndex]);
        isLoading = false;
    }
    
    public void LoadLevel(int levelIndex)
    {
        if (isLoading) return;
        isLoading = true;
        
        if (levelIndex >= 0 && levelIndex < levelSceneNames.Length)
        {
            currentLevelIndex = levelIndex;
            SceneManager.LoadScene(levelSceneNames[currentLevelIndex]);
        }
        
        isLoading = false;
    }
    
    public void LoadLevel(string sceneName)
    {
        if (isLoading) return;
        isLoading = true;
        
        SceneManager.LoadScene(sceneName);
        isLoading = false;
    }
    
    private void SaveProgress()
    {
        // Сохраняем максимальный пройденный уровень
        int savedLevel = PlayerPrefs.GetInt("MaxLevelReached", 0);
        if (currentLevelIndex + 1 > savedLevel)
        {
            PlayerPrefs.SetInt("MaxLevelReached", currentLevelIndex + 1);
            PlayerPrefs.Save();
        }
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
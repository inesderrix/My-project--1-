using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private Text statsText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retryButton;
    
    [Header("Scene Management")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    private bool isVictory = false;
    
    void Start()
    {
        if (GameManager.Instance != null)
        {
            isVictory = GameManager.Instance.GetEnemiesKilled() >= GameManager.Instance.GetEnemiesToWin();
        }
        
        SetupUI();
    }
    
    void SetupUI()
    {
        if (isVictory)
        {
            SetupVictoryUI();
        }
        else
        {
            SetupDefeatUI();
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }
        
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryClicked);
        }
    }
    
    void SetupVictoryUI()
    {
        if (titleText != null)
        {
            titleText.text = "VICTOIRE!";
            titleText.color = Color.yellow;
        }
        
        if (messageText != null)
        {
            messageText.text = "Félicitations! Vous avez éliminé 20 ennemis!";
            messageText.color = Color.white;
        }
        
        if (statsText != null && GameManager.Instance != null)
        {
            statsText.text = $"Ennemis tués: {GameManager.Instance.GetEnemiesKilled()}/20";
            statsText.color = Color.green;
        }
    }
    
    void SetupDefeatUI()
    {
        if (titleText != null)
        {
            titleText.text = "DÉFAITE";
            titleText.color = Color.red;
        }
        
        if (messageText != null)
        {
            messageText.text = "Vous avez été vaincu par les ennemis...";
            messageText.color = Color.white;
        }
        
        if (statsText != null && GameManager.Instance != null)
        {
            statsText.text = $"Ennemis tués: {GameManager.Instance.GetEnemiesKilled()}/20";
            statsText.color = Color.gray;
        }
        
            if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
        }
    }
    
    void OnMainMenuClicked()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    void OnRetryClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

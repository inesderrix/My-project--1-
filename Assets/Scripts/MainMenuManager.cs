using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button jouerButton;
    [SerializeField] private Button equipementButton;
    [SerializeField] private Button quitterButton;
    
    [Header("Scene Management")]
    [SerializeField] private string gameSceneName = "SampleScene";
    
    [Header("Equipment Menu")]
    [SerializeField] private GameObject equipmentPanel;
    
    void Start()
    {
        if (jouerButton != null)
        {
            jouerButton.onClick.AddListener(OnJouerClicked);
        }
        
        if (equipementButton != null)
        {
            equipementButton.onClick.AddListener(OnEquipementClicked);
        }
        
        if (quitterButton != null)
        {
            quitterButton.onClick.AddListener(OnQuitterClicked);
        }
        
        if (equipmentPanel != null)
        {
            equipmentPanel.SetActive(false);
        }
    }
    
    void OnJouerClicked()
    {
        
        SceneManager.LoadScene(gameSceneName);
    }
    
    void OnEquipementClicked()
    {
        
        if (equipmentPanel != null)
        {
            equipmentPanel.SetActive(true);
        }
    }
    
    public void OnBackFromEquipment()
    {
        
        if (equipmentPanel != null)
        {
            equipmentPanel.SetActive(false);
        }
    }
    
    void OnQuitterClicked()
    {
        
        if (UNITY_EDITOR){
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Stats")]
    [SerializeField] private int enemiesKilled = 0;
    [SerializeField] private int enemiesToWin = 30;
    
    [Header("Scene Management")]
    [SerializeField] private int defeatSceneIndex = 2;
    [SerializeField] private int victorySceneIndex = 3;

    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        // Create singleton instance
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
    
    void Start()
    {
        enemiesKilled = 0;
    }
    
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        
        // Check victory condition
        if (enemiesKilled >= enemiesToWin)
        {
            OnVictory();
        }
    }
    
    public void OnPlayerDeath()
    {
        StartCoroutine(DelayedSceneChange(defeatSceneIndex));
    }
    
    public void OnVictory()
    {
        StartCoroutine(DelayedSceneChange(victorySceneIndex));
    }
    
    IEnumerator DelayedSceneChange(int sceneIndex)
    {
        yield return new WaitForSeconds(1.5f); 
        SceneManager.LoadScene(sceneIndex);
    }
    
    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }
    
    public int GetEnemiesToWin()
    {
        return enemiesToWin;
    }
    
    public float GetProgress()
    {
        return (float)enemiesKilled / enemiesToWin;
    }
}

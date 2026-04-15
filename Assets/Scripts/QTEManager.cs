using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CameraRepairSystem : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float intervalBetweenFailures = 120f;

    [Header("Caméras Plateau")]
    [SerializeField] private Camera[] plateauCameras;

    [Header("Caméra QTE (Écran 7)")]
    [SerializeField] private Camera qteCamera;

    [Header("UI QTE")]
    [SerializeField] private Canvas qteCanvas;
    [SerializeField] private UnityEngine.UI.Image[] keyImages; 
    [SerializeField] private UnityEngine.UI.Text infoText;
    
    [Header("Key Images")]
    [SerializeField] private Sprite iKeySprite;  
    [SerializeField] private Sprite kKeySprite;  
    [SerializeField] private Sprite oKeySprite;  
    [SerializeField] private Sprite lKeySprite;  

    [Header("Audio")]
    [SerializeField] private AudioClip cameraFailSound;

    private AudioSource audioSource;
    private PlayerInput playerInput;

    private List<string> allKeys = new List<string> { "I", "K", "O", "L" };
    private List<string> repairSequence = new List<string>();
    private int currentIndex = 0;
    private bool repairInProgress = false;
    private Camera brokenCamera = null;
    private float nextFailureTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        playerInput = FindObjectOfType<PlayerInput>();



        nextFailureTime = Time.time + intervalBetweenFailures;

        if (qteCamera != null)
        {
            qteCamera.enabled = true;
        }

        if (qteCanvas != null)
        {
            qteCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!repairInProgress && Time.time >= nextFailureTime)
        {
            BreakRandomCamera();
        }

        if (repairInProgress)
        {
            CheckInput();
        }
    }

    void BreakRandomCamera()
    {
        if (plateauCameras.Length == 0) return;

        List<Camera> activeCameras = new List<Camera>();
        
        foreach (Camera cam in plateauCameras)
        {
            if (cam != null)
            {
                if (cam.enabled)
                    activeCameras.Add(cam);
            }
        }
        
        if (activeCameras.Count == 0) 
        {
            return;
        }

        int randomIndex = Random.Range(0, activeCameras.Count);
        brokenCamera = activeCameras[randomIndex];
        brokenCamera.enabled = false;

        GenererSequenceAleatoire();
        repairInProgress = true;
        currentIndex = 0;

        if (qteCanvas != null)
        {
            qteCanvas.gameObject.SetActive(true);
            UpdateQTEDisplay();
        }

        if (cameraFailSound != null)
        {
            audioSource.clip = cameraFailSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void CheckInput()
    {
        if (playerInput == null) return;

        string expectedKey = repairSequence[currentIndex];

        if (playerInput.actions[$"Button{expectedKey}"].WasPressedThisFrame())
        {
            currentIndex++;

            UpdateQTEDisplay();

            if (currentIndex >= repairSequence.Count)
            {
                RepairCamera();
            }
        }
    }

    void RepairCamera()
    {
        if (brokenCamera != null)
        {
            brokenCamera.enabled = true;
            brokenCamera = null;
        }

        repairInProgress = false;
        currentIndex = 0;

        if (qteCanvas != null)
        {
            qteCanvas.gameObject.SetActive(false);
        }

        nextFailureTime = Time.time + intervalBetweenFailures;
        
        InitializeInfoText();
        audioSource.Stop();
    }

    void GenererSequenceAleatoire()
    {
        repairSequence.Clear();
        int sequenceLength = 4;
        
        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, allKeys.Count);
            repairSequence.Add(allKeys[randomIndex]);
        }
        
        Debug.Log($"Séquence QTE: {string.Join("-", repairSequence.ToArray())}");
    }

    void UpdateQTEDisplay()
    {
        if (keyImages == null || keyImages.Length < 4)
        {
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < repairSequence.Count)
            {
                string key = repairSequence[i];
                Sprite keySprite = GetKeySprite(key);
                
                if (keySprite != null && keyImages[i] != null)
                {
                    keyImages[i].sprite = keySprite;
                    keyImages[i].enabled = true;
                    
                
                    if (i < currentIndex)
                    {
                        keyImages[i].color = Color.green;   
                    }
                    else if (i == currentIndex)
                    {
                        keyImages[i].color = Color.yellow; 
                    }
                    else
                    {
                        keyImages[i].color = Color.white; 
                    }
                    
                }
            }
            else
            {
                if (keyImages[i] != null)
                {
                    keyImages[i].enabled = false;
                }
            }
        }
        
    }
    
    Sprite GetKeySprite(string key)
    {
        switch (key)
        {
            case "I":
                return iKeySprite;  
            case "K":   
                return kKeySprite;  
            case "O":
                return oKeySprite;  
            case "L":
                return lKeySprite;  
            default:
                return null;
        }
    }
    
    void InitializeInfoText()
    {
        if (infoText != null)
        {
            infoText.text = "CAMÉRA EN PANNE - ENTREZ LE CODE";
        }
    }

    public bool IsRepairInProgress()
    {
        return repairInProgress;
    }
}

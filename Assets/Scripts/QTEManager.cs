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
    [SerializeField] private UnityEngine.UI.Text codeText;
    [SerializeField] private UnityEngine.UI.Text infoText;

    [Header("Audio")]
    [SerializeField] private AudioClip cameraFailSound;

    private AudioSource audioSource;
    private PlayerInput playerInput;

    private List<string> repairSequence = new List<string> { "I", "K", "O", "L" };
    private int currentIndex = 0;
    private bool repairInProgress = false;
    private Camera brokenCamera = null;
    private float nextFailureTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        playerInput = FindObjectOfType<PlayerInput>();



        nextFailureTime = Time.time + intervalBetweenFailures;

        if (qteCamera != null)
            qteCamera.enabled = true;

        if (qteCanvas != null)
            qteCanvas.gameObject.SetActive(false);


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
                if (cam != null && cam.enabled)
                    activeCameras.Add(cam);
            }

            if (activeCameras.Count == 0) return;

            int randomIndex = Random.Range(0, activeCameras.Count);
            brokenCamera = activeCameras[randomIndex];
            brokenCamera.enabled = false;

            repairInProgress = true;
            currentIndex = 0;

            if (qteCanvas != null)
            {
                qteCanvas.gameObject.SetActive(true);
                UpdateQTEDisplay();
            }

            if (cameraFailSound != null)
                audioSource.PlayOneShot(cameraFailSound);
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
                qteCanvas.gameObject.SetActive(false);

            nextFailureTime = Time.time + intervalBetweenFailures;
        }

        void UpdateQTEDisplay()
        {

            if (codeText == null)
            {
                return;
            }

            string displayText = "";
            for (int i = 0; i < repairSequence.Count; i++)
            {
                if (i < currentIndex)
                {
                    displayText += $"<color=green>{repairSequence[i]}</color> ";
                }
                else if (i == currentIndex)
                {
                    displayText += $"<color=yellow>{repairSequence[i]}</color> ";
                }
                else
                {
                    displayText += $"<color=white>{repairSequence[i]}</color> ";
                }
            }

            codeText.text = displayText;

            if (infoText != null)
            {
                infoText.text = "CAMÉRA EN PANNE - ENTREZ LE CODE";
            }
        }
    }

    public bool IsRepairInProgress()
    {
        return repairInProgress;
    }
}
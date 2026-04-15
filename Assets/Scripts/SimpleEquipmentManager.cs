using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SimpleEquipmentManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button backButton;
    [SerializeField] private Transform weaponButtonsPanel;
    [SerializeField] private GameObject previewPanel;
    [SerializeField] private RawImage weaponPreviewImage;
    
    [Header("Weapon Data")]
    [SerializeField] private GameObject[] availableWeapons;
    
    private int selectedWeaponIndex = 0;
    private bool isWeaponSelected = false;
    
    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
        }
        
        InitializeWeaponSlots();
        
        if (previewPanel != null)
        {
            previewPanel.SetActive(false);
        }
    }
    
    void InitializeWeaponSlots()
    {
        if (weaponButtonsPanel == null)
        {
            return;
        }
        
        if (availableWeapons == null || availableWeapons.Length == 0)
        {
            availableWeapons = new GameObject[1];
            
            GameObject testWeapon = GameObject.CreatePrimitive(PrimitiveType.Cube);
            testWeapon.name = "Katana_Test";
            testWeapon.transform.localScale = new Vector3(0.1f, 0.05f, 1f);
            
            Renderer renderer = testWeapon.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material katanaMat = new Material(Shader.Find("Standard"));
                katanaMat.color = Color.gray;
                katanaMat.SetFloat("_Metallic", 0.8f);
                katanaMat.SetFloat("_Glossiness", 0.7f);
                renderer.material = katanaMat;
                
            }
            
            availableWeapons[0] = testWeapon;
        }
        
        for (int i = 1; i <= 6; i++)
        {
            string buttonName = $"Btn_Weapon_{i}";
            Transform buttonTransform = weaponButtonsPanel.Find(buttonName);
            
            if (buttonTransform != null)
            {
                Button button = buttonTransform.GetComponent<Button>();
                if (button != null)
                {
                    int weaponIndex = i - 1;
                    button.onClick.AddListener(() => SelectWeapon(weaponIndex));
                    
                    if (weaponIndex < availableWeapons.Length && availableWeapons[weaponIndex] != null)
                    {
                        Transform weaponHolder = buttonTransform.Find("WeaponHolder");
                        if (weaponHolder == null)
                        {
                            GameObject holder = new GameObject("WeaponHolder");
                            holder.transform.SetParent(buttonTransform);
                            holder.transform.localPosition = Vector3.zero;
                            weaponHolder = holder.transform;
                        }
                        
                        GameObject weapon = Instantiate(availableWeapons[weaponIndex], weaponHolder);
                        weapon.name = $"Weapon_{weaponIndex}";
                        weapon.transform.localScale = Vector3.one * 0.5f;
                    }
                    else
                    {
                        Transform lockedHolder = buttonTransform.Find("LockedHolder");
                        if (lockedHolder == null)
                        {
                            GameObject holder = new GameObject("LockedHolder");
                            holder.transform.SetParent(buttonTransform);
                            holder.transform.localPosition = Vector3.zero;
                            lockedHolder = holder.transform;
                            
                            GameObject lockedIcon = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            lockedIcon.name = "LockedIcon";
                            lockedIcon.transform.SetParent(lockedHolder);
                            lockedIcon.transform.localScale = Vector3.one * 0.3f;
                            lockedIcon.transform.localPosition = Vector3.zero;
                            
                            Renderer renderer = lockedIcon.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material.color = Color.gray;
                            }
                        }
                    }
                }
            }
        }
    }
    
    void Update()
    {
        HandleWeaponSelection();
        Handle3DViewer();
    }
    
    void HandleWeaponSelection()
    {
        if (Keyboard.current != null && Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            selectedWeaponIndex = (selectedWeaponIndex - 1 + 6) % 6;
            UpdateWeaponDisplay();
        }
        else if (Keyboard.current != null && Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % 6;
            UpdateWeaponDisplay();
        }
    }
    
    void SelectWeapon(int index)
    {
        if (index >= availableWeapons.Length || availableWeapons[index] == null)
        {
            return;
        }
        
        selectedWeaponIndex = index;
        isWeaponSelected = true;
        
        
        if (previewPanel != null)
        {
            previewPanel.SetActive(true);
            
            UpdateWeaponDisplay();
            
            foreach (Transform child in previewPanel.transform)
            {
                if (child.name.StartsWith("Weapon_3D_"))
                {
                    Destroy(child.gameObject);
                }
            }
            
            GameObject weapon3D = Instantiate(availableWeapons[index], previewPanel.transform);
            weapon3D.name = $"Weapon_3D_{index}";
            weapon3D.transform.localPosition = Vector3.zero;
            weapon3D.transform.localRotation = Quaternion.identity;
            weapon3D.transform.localScale = Vector3.one * 2f;        
        }
        
        UpdateWeaponDisplay();
    }
    
    //ne marche pas probleme rendu a revoir
    void Handle3DViewer()
    {
        if (isWeaponSelected && previewPanel != null && previewPanel.activeSelf)
        {
            previewPanel.transform.Rotate(0, 30 * Time.deltaTime, 0);
        }
    }
    
    void UpdateWeaponDisplay()
    {
        for (int i = 1; i <= 6; i++)
        {
            string buttonName = $"Btn_Weapon_{i}";
            Transform buttonTransform = weaponButtonsPanel.Find(buttonName);
            
            if (buttonTransform != null)
            {
                Button button = buttonTransform.GetComponent<Button>();
                if (button != null)
                {
                    Image buttonImage = button.GetComponent<Image>();
                    if (buttonImage != null)
                    {
                        if (i - 1 == selectedWeaponIndex)
                        {
                            buttonImage.color = Color.yellow;
                        }
                        else
                        {
                            buttonImage.color = Color.white;
                        }
                    }
                }
            }
        }
    }
    
    void OnBackClicked()
    {
        
        MainMenuManager mainMenuManager = FindObjectOfType<MainMenuManager>();
        if (mainMenuManager != null)
        {
            mainMenuManager.OnBackFromEquipment();
        }

    }
}

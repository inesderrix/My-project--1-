using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerAlternatifTest : MonoBehaviour
{
    private const string ACTION_U = "ButtonU";
    private const string ACTION_J = "ButtonJ";
    private const string ACTION_I = "ButtonI";
    private const string ACTION_K = "ButtonK";
    private const string ACTION_O = "ButtonO";
    private const string ACTION_L = "ButtonL";

    private PlayerInput playerInput;
    private string keysPressed = "";

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }
    }

    void Update()
    {
        keysPressed = "";

        if (playerInput != null)
        {
            if (playerInput.actions[ACTION_U].IsPressed()) keysPressed += "U ";
            if (playerInput.actions[ACTION_J].IsPressed()) keysPressed += "J ";
            if (playerInput.actions[ACTION_I].IsPressed()) keysPressed += "I ";
            if (playerInput.actions[ACTION_K].IsPressed()) keysPressed += "K ";
            if (playerInput.actions[ACTION_O].IsPressed()) keysPressed += "O ";
            if (playerInput.actions[ACTION_L].IsPressed()) keysPressed += "L ";
        }

        if (string.IsNullOrEmpty(keysPressed))
        {
            keysPressed = "Aucune";
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.LowerLeft;

        string displayText = $"Contrôleur alternatif : {keysPressed}";

        GUI.Label(new Rect(10, Screen.height - 40, 500, 30), displayText, style);
    }
}
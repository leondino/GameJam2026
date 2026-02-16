using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private string gameplayActionMap = "Player";
    [SerializeField]
    private string menuActionMap = "UI";

    private bool menuOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();

        // Lock cursor to middle of the screen and hide it at start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle cursor lock with Escape so user can release during development

    }

    public void OnSwitchMenu(InputAction.CallbackContext context)
    {
        if (!context.action.WasPressedThisFrame()) return;

        // Toggle menu state
        menuOpen = !menuOpen;

        if (menuOpen)
        {
            // Switch to the menu/UI action map
            if (playerInput != null)
            {
                playerInput.SwitchCurrentActionMap(menuActionMap);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("A menu is opened");
        }
        else
        {
            // Switch back to gameplay map
            if (playerInput != null)
            {
                playerInput.SwitchCurrentActionMap(gameplayActionMap);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("A menu is closed");
        }
    }
}

using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Cursors")]
    [SerializeField]
    private Texture2D cursorDefault;
    [SerializeField]
    private Texture2D cursorHand;
    [SerializeField]
    private Texture2D cursorHandPoint;
    [SerializeField]
    private Texture2D cursorGlove;
    [SerializeField]
    private Texture2D cursorGlovePoint;
    [SerializeField]
    private Vector2 cursorHotspot = new Vector2(16f, 16f);

    private PlayerInput playerInput;
    [SerializeField]
    public GameObject player;
    private CustomerSpawner customerSpawner;
    [HideInInspector]
    public ActiveNPCManager activeNPCManager;

    [SerializeField]
    private string gameplayActionMap = "Player";
    [SerializeField]
    private string menuActionMap = "UI";

    private bool menuOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInput = player.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap(gameplayActionMap);

        customerSpawner = GetComponent<CustomerSpawner>();
        activeNPCManager = GetComponent<ActiveNPCManager>();

        // Lock cursor to middle of the screen and hide it at start
        SetCursor(CursorType.Hand);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activeNPCManager.queueNPCs.Count < activeNPCManager.maxQueueLength)
            customerSpawner.SpawnCustomer();
    }

    public void AddActiveNPC(GameObject npc)
    {
        activeNPCManager.activeNPCs.Enqueue(npc);
    }

    public void OnSwitchMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("why i run twice?");
            // Toggle menu state
            menuOpen = !menuOpen;

            if (menuOpen)
            {
                // Switch to the menu/UI action map
                if (playerInput != null)
                {
                    playerInput.SwitchCurrentActionMap(menuActionMap);

                    Debug.Log("A menu is opened");
                }

                SetCursor(CursorType.HandPoint);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
            else
            {
                // Switch back to gameplay map
                if (playerInput != null)
                {
                    playerInput.SwitchCurrentActionMap(gameplayActionMap);
                    Debug.Log("A menu is closed");
                }

                SetCursor(CursorType.Hand);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;

            }
        }
    }

    public enum CursorType { Default, Hand, HandPoint, Glove, GlovePoint }

    /// <summary>
    /// Sets the Cursor to the given cursor type and applies the corresponding texture. If the texture is missing, it will fall back to the OS default cursor.
    /// </summary>
    /// <param name="type"></param>
    private void SetCursor(CursorType type)
    {
        Texture2D tex = cursorDefault;
        switch (type)
        {
            case CursorType.Default: tex = cursorDefault; 
                break;
            case CursorType.Hand: tex = cursorHand; 
                break;
            case CursorType.HandPoint: tex = cursorHandPoint; 
                break;
            case CursorType.Glove: tex = cursorGlove; 
                break;
            case CursorType.GlovePoint: tex = cursorGlovePoint;
                break;
        }

        if (tex != null)
        {
            UnityEngine.Cursor.SetCursor(tex, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            // If texture missing, clear cursor to OS default
            UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}

using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private float gameTimePast = 0f;

    [Header("Cursors")]
    [SerializeField]
    private Texture2D cursorDefault;
    private Vector2 cursorHotspot = new Vector2(16f, 16f);

    public GameObject SearchCompleteUI;
    public InventoryBarManager inventoryBarManager;

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

    [Header("GameStart / GameOver")]
    [SerializeField]
    private TMP_Text gameStartText;
    [SerializeField]
    private float gameStartFadeIn = 0.5f;
    [SerializeField]
    private float gameStartHold = 1.5f;
    [SerializeField]
    private float gameStartFadeOut = 0.5f;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private TMP_Text scoreTimeText;
    private bool isGameOver = false;

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

        ShowGameStartSequence();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activeNPCManager.queueNPCs.Count < activeNPCManager.maxQueueLength)
            customerSpawner.SpawnCustomer();
        gameTimePast += Time.fixedDeltaTime;
    }

    public void AddActiveNPC(GameObject npc)
    {
        activeNPCManager.activeNPCs.Enqueue(npc);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
        // Format elapsed time as HH:MM:SS
        int totalSeconds = Mathf.FloorToInt(gameTimePast);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        scoreTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    public void RestartGame()
    {
        // Reload the currently active scene to restart the game
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Optionally reset any timescale or persistent state here
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Shows the game start text with a fade-in, hold, and fade-out sequence.
    /// Call this to run the intro text animation.
    /// </summary>
    public void ShowGameStartSequence()
    {
        if (gameStartText == null) return;
        // Ensure any running coroutine is stopped before starting a new one
        StopCoroutine("GameStartRoutine");
        StartCoroutine("GameStartRoutine");
    }

    /// <summary>
    /// Initiates the game start sequence by displaying the start text with a fade-in effect, holding it visible for a
    /// specified duration, and then fading it out before deactivating the text object.
    /// </summary>
    /// <remarks>The routine requires that the 'gameStartText' is not null. It manages the visibility and
    /// transparency of the text over time, using the configured fade-in, hold, and fade-out durations. This method is
    /// intended to be used with Unity's coroutine system.</remarks>
    /// <returns>An enumerator that performs the game start sequence asynchronously. The routine completes when the start text
    /// has been fully faded out and deactivated.</returns>
    private IEnumerator GameStartRoutine()
    {
        if (gameStartText == null) yield break;

        gameStartText.gameObject.SetActive(true);
        Color c = gameStartText.color;
        float t = 0f;

        // Fade in
        while (t < gameStartFadeIn)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / Mathf.Max(0.0001f, gameStartFadeIn));
            gameStartText.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        // Hold
        yield return new WaitForSeconds(gameStartHold);

        // Fade out
        t = 0f;
        while (t < gameStartFadeOut)
        {
            t += Time.deltaTime;
            float a = 1f - Mathf.Clamp01(t / Mathf.Max(0.0001f, gameStartFadeOut));
            gameStartText.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        // Ensure invisible at end
        gameStartText.color = new Color(c.r, c.g, c.b, 0f);
        gameStartText.gameObject.SetActive(false);
    }

    public void OnSwitchMenu(InputAction.CallbackContext context)
    {
        if (context.started &! isGameOver)
        {
            SwitchToMenuControls();
        }
    }

    private void SwitchToMenuControls()
    {
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

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;

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

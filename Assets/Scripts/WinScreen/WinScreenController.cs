using UnityEngine;
using UnityEngine.SceneManagement;

/// Shared "You Won" screen. A WinTrigger anywhere in the scene calls Instance.ShowWinScreen()
/// when the Player reaches it. Pauses gameplay and unlocks the cursor so the Play Again
/// button can be clicked; Play Again reloads the current scene from the start.
public class WinScreenController : MonoBehaviour
{
    public static WinScreenController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject winScreenRoot;

    [Header("Behavior")]
    [SerializeField] private bool pauseGameplayWhileShown = true;

    private void Awake()
    {
        Instance = this;

        if (winScreenRoot != null)
        {
            winScreenRoot.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void ShowWinScreen()
    {
        if (winScreenRoot != null)
        {
            winScreenRoot.SetActive(true);
        }

        if (pauseGameplayWhileShown)
        {
            Time.timeScale = 0f;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

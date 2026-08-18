using System.Collections;
using TMPro;
using UnityEngine;

/// Shared narrative popup UI. Lives once per scene (e.g. under NarrativeSystem/Canvas)
/// and is looked up by NarrativeTrigger instances through the static Instance reference,
/// so triggers never need a manually-dragged reference to it.
public class NarrativeSystemController : MonoBehaviour
{
    public static NarrativeSystemController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI narrativeText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timing")]
    [Tooltip("How long the text stays fully visible before fading out.")]
    [SerializeField] private float displayDuration = 3.5f;
    [Tooltip("Duration of the fade in / fade out.")]
    [SerializeField] private float fadeDuration = 0.4f;

    private Coroutine activeRoutine;

    private void Awake()
    {
        Instance = this;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// Shows (or replaces) the current narrative message and restarts the display timer.
    public void ShowNarrative(string text)
    {
        if (narrativeText == null || string.IsNullOrEmpty(text))
        {
            return;
        }

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
        }

        activeRoutine = StartCoroutine(ShowRoutine(text));
    }

    private IEnumerator ShowRoutine(string text)
    {
        narrativeText.text = text;

        yield return StartCoroutine(Fade(1f, fadeDuration));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(Fade(0f, fadeDuration));

        activeRoutine = null;
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}

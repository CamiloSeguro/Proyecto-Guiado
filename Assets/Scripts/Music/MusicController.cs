using System.Collections;
using UnityEngine;

/// Shared music manager. Owns the ambient background track and the boss music
/// track. BossMusicTrigger calls PlayBossMusic() when the player reaches the
/// boss area; BossDeathReaction calls FadeOutBossMusicAndResumeBackground()
/// when the boss dies.
public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource bossMusicSource;

    [Header("Fade Settings")]
    [Tooltip("How long the boss music takes to fade to silence when the boss dies.")]
    [SerializeField] private float bossMusicFadeOutDuration = 2f;

    private float bossMusicBaseVolume;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        Instance = this;

        if (bossMusicSource != null)
        {
            bossMusicBaseVolume = bossMusicSource.volume;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayBossMusic()
    {
        if (bossMusicSource == null || bossMusicSource.isPlaying)
        {
            return;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
        }

        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Pause();
        }

        bossMusicSource.volume = bossMusicBaseVolume;
        bossMusicSource.Play();
    }

    public void FadeOutBossMusicAndResumeBackground()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        if (bossMusicSource != null && bossMusicSource.isPlaying)
        {
            float startVolume = bossMusicSource.volume;
            float elapsed = 0f;

            while (elapsed < bossMusicFadeOutDuration)
            {
                elapsed += Time.deltaTime;
                bossMusicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / bossMusicFadeOutDuration);
                yield return null;
            }

            bossMusicSource.Stop();
            bossMusicSource.volume = bossMusicBaseVolume;
        }

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.UnPause();
        }

        fadeRoutine = null;
    }
}

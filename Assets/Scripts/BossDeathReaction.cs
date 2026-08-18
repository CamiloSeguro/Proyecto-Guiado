using Gamekit3D;
using UnityEngine;

// Reacts to the Boss's existing Damageable.OnDeath event: triggers the player's
// victory animation, plays whatever clip is set on the CelebrationSource /
// BossDefeatedSource audio sources (each with its own Volume, editable in the
// Inspector), and fades out the boss music (if any is playing) back to the
// ambient background track. Player and Boss are located through their tags so
// this only ever reacts to the correct, tagged objects.
public class BossDeathReaction : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bossTag = "Boss";
    [Tooltip("Name of the Animator trigger to fire on the Player when the boss dies.")]
    [SerializeField] private string victoryAnimationTrigger = "Pump";

    [Header("Sound")]
    [Tooltip("Name of the child AudioSource (under the Player) that plays the celebration clip.")]
    [SerializeField] private string celebrationSourceName = "CelebrationSource";
    [Tooltip("Name of the child AudioSource (under the Boss) that plays the defeat clip.")]
    [SerializeField] private string bossDefeatedSourceName = "BossDefeatedSource";

    private Animator playerAnimator;
    private AudioSource celebrationSource;
    private Damageable bossDamageable;
    private AudioSource bossDefeatedSource;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
            celebrationSource = FindNamedAudioSource(player.transform, celebrationSourceName);
        }

        GameObject boss = GameObject.FindGameObjectWithTag(bossTag);
        if (boss != null)
        {
            bossDamageable = boss.GetComponent<Damageable>();
            if (bossDamageable != null)
            {
                bossDamageable.OnDeath.AddListener(OnBossDeath);
            }

            bossDefeatedSource = FindNamedAudioSource(boss.transform, bossDefeatedSourceName);
        }
    }

    private void OnDestroy()
    {
        if (bossDamageable != null)
        {
            bossDamageable.OnDeath.RemoveListener(OnBossDeath);
        }
    }

    private void OnBossDeath()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(victoryAnimationTrigger);
        }

        if (celebrationSource != null)
        {
            celebrationSource.Play();
        }

        if (bossDefeatedSource != null)
        {
            bossDefeatedSource.Play();
        }

        if (MusicController.Instance != null)
        {
            MusicController.Instance.FadeOutBossMusicAndResumeBackground();
        }
    }

    private static AudioSource FindNamedAudioSource(Transform root, string childName)
    {
        AudioSource[] sources = root.GetComponentsInChildren<AudioSource>(true);
        foreach (AudioSource source in sources)
        {
            if (source.gameObject.name == childName)
            {
                return source;
            }
        }

        return null;
    }
}

using UnityEngine;

/// Invisible trigger volume: when the Player enters it, starts the boss music
/// via the shared MusicController. Position and Box Collider size stay fully
/// editable - place this wherever the boss encounter should begin.
[RequireComponent(typeof(BoxCollider))]
public class BossMusicTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [Tooltip("If enabled, this trigger only starts the boss music the first time the Player enters it.")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;

    private void Reset()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        if (triggerOnlyOnce && hasTriggered)
        {
            return;
        }

        hasTriggered = true;

        if (MusicController.Instance != null)
        {
            MusicController.Instance.PlayBossMusic();
        }
        else
        {
            Debug.LogWarning("BossMusicTrigger: no MusicController found in the scene.", this);
        }
    }
}

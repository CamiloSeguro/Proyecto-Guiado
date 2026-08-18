using UnityEngine;

/// Invisible trigger volume: when the Player enters it, shows the shared WinScreenController.
/// Position and Box Collider size stay fully editable - place this wherever the "win" spot is.
[RequireComponent(typeof(BoxCollider))]
public class WinTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [Tooltip("If enabled, this trigger only shows the win screen the first time the Player enters it.")]
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

        if (WinScreenController.Instance != null)
        {
            WinScreenController.Instance.ShowWinScreen();
        }
        else
        {
            Debug.LogWarning("WinTrigger: no WinScreenController found in the scene.", this);
        }
    }
}

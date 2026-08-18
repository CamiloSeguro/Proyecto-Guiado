using UnityEngine;

/// Invisible trigger volume that shows a short narrative message on the shared
/// NarrativeSystem when the Player enters it. Position and Box Collider size stay
/// fully editable in the Inspector/Scene view - nothing here is hard-coded.
[RequireComponent(typeof(BoxCollider))]
public class NarrativeTrigger : MonoBehaviour
{
    [Header("Narrative Text")]
    [TextArea(2, 5)]
    [SerializeField] private string narrativeText = "Enter narrative text here...";

    [Header("Behavior")]
    [Tooltip("If enabled, this trigger only shows its text the first time the Player enters it.")]
    [SerializeField] private bool triggerOnlyOnce = false;
    [SerializeField] private string playerTag = "Player";

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

        if (NarrativeSystemController.Instance != null)
        {
            NarrativeSystemController.Instance.ShowNarrative(narrativeText);
        }
        else
        {
            Debug.LogWarning("NarrativeTrigger: no NarrativeSystemController found in the scene.", this);
        }
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public class InteractionDisplay : MonoBehaviour
{
    [SerializeField] Interactor interactor;
    [SerializeField] UIDocument uiDocument;

    private VisualElement cursorDot;
    private Label interactionLabel;

    void Awake()
    {
        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            cursorDot = root.Q<VisualElement>("cursor-dot");
            interactionLabel = root.Q<Label>("interaction-label");
        }
    }

    void OnEnable()
    {
        if (interactor != null)
            interactor.OnInteractionChanged += UpdateText;

        UpdateText(null);
    }

    void OnDisable()
    {
        if (interactor != null)
            interactor.OnInteractionChanged -= UpdateText;
    }

    void UpdateText(string newText)
    {
        bool hasText = !string.IsNullOrEmpty(newText);

        if (interactionLabel != null)
        {
            interactionLabel.text = newText ?? "";
            interactionLabel.style.display = hasText ? DisplayStyle.Flex : DisplayStyle.None;
        }

        if (cursorDot != null)
        {
            cursorDot.style.display = hasText ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}

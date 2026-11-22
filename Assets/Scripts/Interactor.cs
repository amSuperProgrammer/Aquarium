using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform raycastOrigin;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] LayerMask interactableLayer = -1;
    [SerializeField] InputActionReference interactAction;

    public event Action<string> OnInteractionChanged;

    IInteractable currentInteractable;

    void OnEnable() => interactAction?.action.Enable();
    void OnDisable() => interactAction?.action.Disable();

    void Update()
    {
        Transform origin = raycastOrigin != null ? raycastOrigin : transform;
        IInteractable foundInteractable = null;

        if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, maxDistance, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                // Если мы уже что-то держим, и это тоже Pickupable - игнорируем текст
                if (Pickupable.CurrentlyHeld != null && interactable is Pickupable)
                {
                    foundInteractable = null;
                }
                else
                {
                    foundInteractable = interactable;
                }
            }
        }

        if (foundInteractable != currentInteractable)
        {
            currentInteractable = foundInteractable;
            OnInteractionChanged?.Invoke(currentInteractable?.InteractionText);
        }

        if (currentInteractable != null && interactAction.action.WasPressedThisFrame())
        {
            currentInteractable.Interact();
        }
    }
}

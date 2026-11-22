using UnityEngine;
using UnityEngine.InputSystem;

public class Pickupable : MonoBehaviour, IInteractable
{
    public static Pickupable CurrentlyHeld { get; private set; }

    public string InteractionText => "Поднять";

    [SerializeField] LayerMask placementLayer;
    [SerializeField] float placementRange = 2f;

    [Tooltip("Поворот для исправления экспорта из Blender (обычно -90 по X)")]
    [SerializeField] Vector3 rotationOffset = new Vector3(-90, 0, 0);

    private Transform holdPoint;
    private Transform playerCamera;
    private Rigidbody rb;
    private bool isHeld = false;
    private Collider col;
    private bool justPickedUp = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            playerCamera = mainCam.transform;
            holdPoint = playerCamera.Find("HoldPoint");
        }
    }

    void Update()
    {
        if (!isHeld) return;

        if (!justPickedUp && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            PlaceDown();
            return;
        }
        justPickedUp = false;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, placementRange, placementLayer))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(rotationOffset);
        }
        else
        {
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation * Quaternion.Euler(rotationOffset);
        }
    }

    public void Interact()
    {
        if (!isHeld && CurrentlyHeld == null)
        {
            Pickup();
        }
    }

    void Pickup()
    {
        isHeld = true;
        justPickedUp = true;
        CurrentlyHeld = this;

        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(holdPoint);
    }

    void PlaceDown()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, placementRange, placementLayer))
        {
            isHeld = false;
            CurrentlyHeld = null;
            transform.SetParent(null);

            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(rotationOffset);

            rb.isKinematic = false;
            col.enabled = true;
        }
        else
        {
            isHeld = false;
            CurrentlyHeld = null;
            transform.SetParent(null);

            rb.isKinematic = false;
            col.enabled = true;
        }
    }
}

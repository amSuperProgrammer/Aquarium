using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cameraPivot;
    [SerializeField] Transform cameraLens;
    [SerializeField] InputActionReference move;
    [SerializeField] float speed = 5f;
    [SerializeField] float sensitivity = 2f;

    CharacterController controller;

    Vector2 smoothDelta;
    float pitch;

    // Метод для автоматического назначения ссылок из CameraHierarchySetup
    public void AssignCameraTransforms(Transform pivot, Transform lens)
    {
        cameraPivot = pivot;
        cameraLens = lens;
    }

    void Start()
    {
        move.action.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();

        // Если ссылки не назначены вручную — пробуем найти автоматически
        if (cameraPivot == null)
        {
            var pivot = transform.Find("CameraPivot");
            if (pivot != null)
                cameraPivot = pivot;
        }
        if (cameraLens == null)
        {
            var cam = GetComponentInChildren<Camera>(true);
            if (cam != null)
                cameraLens = cam.transform;
        }
    }

    void LateUpdate()
    {
        if (cameraPivot == null || cameraLens == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity;
        smoothDelta = Vector2.Lerp(smoothDelta, delta, 1f - Mathf.Exp(-20f * Time.unscaledDeltaTime));

        // Горизонталь — вращаем пивот вокруг глобального Up
        cameraPivot.Rotate(Vector3.up * smoothDelta.x);

        // Вертикаль — вращаем линзу локально вокруг оси X
        pitch -= smoothDelta.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        cameraLens.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void Update()
    {
        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;
        // Убираем наклон по вертикали — игрок ходит только по горизонтали
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector2 input = move.action.ReadValue<Vector2>();
        Vector3 velocity = (forward * input.y + right * input.x) * speed;

        controller.Move(velocity * Time.deltaTime);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] InputActionReference move;
    [SerializeField] float speed = 5f;
    [SerializeField] float sensitivity = 5f;
    Rigidbody rb;

    float pitch;

    void Start()
    {
        move.action.Enable();
        Cursor.visible = false;

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // Поворот камеры обрабатываем в LateUpdate, чтобы он следовал за движением
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity * Time.deltaTime;
        cameraTransform.Rotate(Vector3.up * delta.x);

        Vector3 euler = cameraTransform.rotation.eulerAngles;
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, -80, 80);
        cameraTransform.localRotation = Quaternion.Euler(pitch, euler.y, 0);
    }

    void FixedUpdate()
    {
        // Сбрасываем боковую скорость — чтобы стены и другие объекты не толкали нас
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

        // Физика и движение тела только в FixedUpdate
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        // Убираем наклон по вертикали — игрок ходит только по горизонтали
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector2 input = move.action.ReadValue<Vector2>();
        Vector3 velocity = (forward * input.y + right * input.x) * speed;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}

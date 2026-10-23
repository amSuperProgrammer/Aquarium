using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] InputActionReference move;
    [SerializeField] float speed = 5;
    [SerializeField] float sensitiivty = 5;
    float pitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move.action.Enable();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitiivty * Time.deltaTime;

        cameraTransform.Rotate(Vector3.up * delta.x);

        Vector3 rotateCamera = cameraTransform.rotation.eulerAngles;
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, -90, 90);
        cameraTransform.localRotation = Quaternion.Euler(pitch, rotateCamera.y, 0);


        Vector3 cameraForward = cameraTransform.forward;
        cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z);
        Vector2 dir = move.action.ReadValue<Vector2>();
        transform.Translate(cameraTransform.right * dir.x * speed * Time.deltaTime);
        transform.Translate(cameraForward * dir.y * speed * Time.deltaTime);
    }
}

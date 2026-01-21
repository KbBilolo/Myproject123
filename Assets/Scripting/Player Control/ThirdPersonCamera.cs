using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Distance & Height")]
    public float distance = 4f;
    public float height = 2f;

    [Header("Rotation")]
    public float mouseSensitivity = 2.5f;
    public float stickSensitivity = 120f;
    public float minY = -30f;
    public float maxY = 60f;

    [Header("Input")]
    public InputActionReference lookAction; // Mouse / Right Stick
    public bool useMobileInput = false;

    [Header("Panning")]
    public float panX = 0f; // Horizontal offset for camera placement

    [Header("Collision")]
    public LayerMask collisionLayers = ~0; // Collide with everything by default
    public float cameraRadius = 0.2f; // For spherecast, helps avoid clipping

    private float yaw;
    private float pitch;
    private Vector2 mobileLook;

    void OnEnable()
    {
        if (lookAction != null)
            lookAction.action.Enable();
    }

    void OnDisable()
    {
        if (lookAction != null)
            lookAction.action.Disable();
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector2 lookInput = Vector2.zero;

        if (!useMobileInput && lookAction != null)
            lookInput = lookAction.action.ReadValue<Vector2>();

        if (useMobileInput)
            lookInput = mobileLook;

        // Horizontal & vertical rotation
        yaw += lookInput.x * stickSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Camera position with panX offset
        Vector3 offset = rotation * new Vector3(panX, 0f, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + offset;

        // Collision check
        Vector3 targetCenter = target.position + Vector3.up * height;
        Vector3 direction = (desiredPosition - targetCenter).normalized;
        float desiredDistance = (desiredPosition - targetCenter).magnitude;

        RaycastHit hit;
        if (Physics.SphereCast(targetCenter, cameraRadius, direction, out hit, desiredDistance, collisionLayers))
        {
            // Place camera at hit point, slightly away from the wall
            transform.position = hit.point - direction * cameraRadius;
        }
        else
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(targetCenter);
    }

    // Mobile swipe input
    public void SetMobileLook(Vector2 input)
    {
        mobileLook = input;
    }
}



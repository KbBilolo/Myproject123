using UnityEngine;
using UnityEngine.InputSystem;

// Controls player movement and rotation (PC + Mobile, 3rd Person)  
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Range(1f, 10f)] public float speed = 5f;
    public float rotationSmoothTime = 0.1f;

    [Header("Input")]
    public InputActionReference moveAction; // PC / Gamepad  
    public bool useMobileInput = false;

    private Rigidbody rb;
    //private Animator animator;
    [SerializeField]
    private Transform cam;

    private Vector2 moveInput;
    private Vector2 mobileInput;

    private float currentVelocityY;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        
    }
    private void Start()
    {
        cam = Camera.main.transform;
    }
    void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
    }

    void Update()
    {
        // PC / Gamepad  
        if (!useMobileInput && moveAction != null)
            moveInput = moveAction.action.ReadValue<Vector2>();

        // Mobile  
        if (useMobileInput)
            moveInput = mobileInput;
    }
    void FixedUpdate()
    {
        if (moveInput.sqrMagnitude < 0.01f)
        {
            // Commented out animation logic  
            // animator.SetBool("isMoving", false);  
            // animator.SetBool("isLturn", false);  
            // animator.SetBool("isRturn", false);  
            return;
        }

        // Camera-relative direction  
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        // Move  
        Vector3 movement = moveDir * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Smooth rotation toward movement direction  
        float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(
            rb.rotation.eulerAngles.y,
            targetAngle,
            ref currentVelocityY,
            rotationSmoothTime
        );

        rb.MoveRotation(Quaternion.Euler(0f, smoothAngle, 0f));

        // Commented out animation logic  
        // animator.SetBool("isMoving", true);  
        // animator.SetBool("isLturn", moveInput.x < -0.1f);  
        // animator.SetBool("isRturn", moveInput.x > 0.1f);  
    }

    // Called by Mobile Joystick UI  
    public void SetMobileInput(Vector2 input)
    {
        mobileInput = input;
    }
}

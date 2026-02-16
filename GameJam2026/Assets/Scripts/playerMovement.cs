using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Vector2 movementVector, rotationVector;
    [SerializeField]
    private float movementSpeed, rotateSpeed;
    [SerializeField]
    private float maxPitch = 80f;
    private float pitch;
    private float yaw;
    private Rigidbody rBody;
    private Camera playerCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        // Initialize pitch and yaw from camera rotation (convert pitch to -180..180 range)
        yaw = playerCam.transform.eulerAngles.y;
        pitch = playerCam.transform.eulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        Walking();
    }

    // Handle camera rotation in Update to ensure it happens every frame, not just on physics updates
    private void Update()
    {
        playerCam.transform.position = new Vector3(rBody.transform.position.x, playerCam.transform.position.y, rBody.position.z);
        CameraRotation();
    }

    /// <summary>
    /// Handles player movement absed on input. Movement is applied in the direction the player is facing (forward vector) and is scaled by movement speed and time.
    /// </summary>
    private void Walking() 
    {
        rBody.MovePosition(rBody.position + transform.forward * new Vector3(movementVector.x, 0, movementVector.y).magnitude * movementSpeed * Time.fixedDeltaTime);
        // Rotate the player body to match the yaw of the camera (but not pitch)
        rBody.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
    }

    /// <summary>
    /// Handles Camera rotation based on mouse input. Yaw (horizontal rotation) is applied to the player body, 
    /// while pitch (vertical rotation) is applied only to the camera. Pitch is clamped to prevent flipping. 
    /// Rotation is applied every frame in Update for smoothness, while movement is handled in FixedUpdate for consistent physics behavior.
    /// </summary>
    private void CameraRotation() 
    {
        // Accumulate yaw and pitch, clamp pitch so the camera can't flip, force roll (Z) to 0.
        pitch -= rotationVector.y * rotateSpeed * Time.fixedDeltaTime;
        yaw += rotationVector.x * rotateSpeed * Time.fixedDeltaTime;

        // Clamp vertical look
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        playerCam.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // Rotate the player body to match the yaw of the camera (but not pitch)
        rBody.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        rotationVector = context.ReadValue<Vector2>();
    }
}

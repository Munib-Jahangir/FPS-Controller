using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float sensitivity = 2.5f;
    public float smoothness = 25f;
    
    [Header("Body Reference")]
    public Transform playerBody;

    private float _xRotation = 0f;
    private float _currentX, _currentY;
    private float _targetX, _targetY;

    void Start()
    {
        // Lock cursor for FPS experience
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Auto-assign player body if missing
        if (playerBody == null && transform.parent != null)
        {
            playerBody = transform.parent;
        }
    }

    void LateUpdate()
    {
        // Get raw input
        _targetX = Input.GetAxisRaw("Mouse X") * sensitivity;
        _targetY = Input.GetAxisRaw("Mouse Y") * sensitivity;

        // Smooth the raw input
        _currentX = Mathf.Lerp(_currentX, _targetX, smoothness * Time.deltaTime);
        _currentY = Mathf.Lerp(_currentY, _targetY, smoothness * Time.deltaTime);

        // Calculate vertical rotation (Pitch)
        _xRotation -= _currentY;
        _xRotation = Mathf.Clamp(_xRotation, -85f, 85f);

        // Apply vertical rotation to camera
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        
        // Apply horizontal rotation to player body (Yaw)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * _currentX);
        }
    }
}

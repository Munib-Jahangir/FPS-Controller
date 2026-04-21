using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 8f;
    public float sprintSpeed = 13f;
    public float jumpPower = 2.5f;
    public float gravityValue = -40f; // Stronger gravity for "snappier" feel

    [Header("Quick Jump Features")]
    public float jumpBufferTime = 0.15f; // Jump if pressed slightly before landing
    public float coyoteTime = 0.15f;     // Jump if walked off ledge recently
    
    [Header("Polishing")]
    public float inputSmoothTime = 0.05f; // Faster response
    public Camera playerCamera;
    public float bobSpeed = 12f;
    public float bobAmount = 0.04f;
    public float baseFOV = 65f;
    public float sprintFOV = 75f;

    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private Vector3 _currentMoveVelocity;
    private Vector3 _moveDampVelocity;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private bool _isGrounded;
    private float _defaultYPos;
    private float _timer;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (playerCamera != null) _defaultYPos = playerCamera.transform.localPosition.y;
        else playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        _isGrounded = _characterController.isGrounded;

        // Snappier Grounding
        if (_isGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
            if (_playerVelocity.y < 0) _playerVelocity.y = -2f;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffering
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        // Execute Jump if buffered and "grounded" (within coyote time)
        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpPower * -2.0f * gravityValue);
            _jumpBufferCounter = 0f;
            _coyoteTimeCounter = 0f;
        }

        HandleMovement();
        HandleHeadBob();
        HandleFOV();

        // Apply Gravity
        _playerVelocity.y += gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        _currentMoveVelocity = Vector3.SmoothDamp(_currentMoveVelocity, inputDir, ref _moveDampVelocity, inputSmoothTime);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && inputDir.magnitude > 0.1f;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        
        Vector3 moveVector = transform.right * _currentMoveVelocity.x + transform.forward * _currentMoveVelocity.z;
        _characterController.Move(moveVector * currentSpeed * Time.deltaTime);
    }

    void HandleHeadBob()
    {
        if (playerCamera == null) return;
        if (_characterController.velocity.magnitude > 0.1f && _isGrounded)
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            _timer += Time.deltaTime * (isSprinting ? bobSpeed * 1.5f : bobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                _defaultYPos + Mathf.Sin(_timer) * bobAmount,
                playerCamera.transform.localPosition.z
            );
        }
        else
        {
            _timer = 0;
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                Mathf.Lerp(playerCamera.transform.localPosition.y, _defaultYPos, Time.deltaTime * 10f),
                playerCamera.transform.localPosition.z
            );
        }
    }

    void HandleFOV()
    {
        if (playerCamera == null) return;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && _characterController.velocity.magnitude > 0.5f;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, isSprinting ? sprintFOV : baseFOV, Time.deltaTime * 8f);
    }
}

using UnityEngine;

namespace FPSProFramework
{
    [RequireComponent(typeof(CharacterController))]
    public class ParkourFPSController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 8f;
        [SerializeField] private float sprintSpeed = 16f;
        [SerializeField] private float airSpeed = 12f;
        [SerializeField] private float wallRunSpeed = 20f;
        [SerializeField] private float slideSpeed = 25f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float doubleJumpForce = 10f;
        [SerializeField] private float gravity = -30f;
        
        [Header("Wall Run Settings")]
        [SerializeField] private float wallRunGravity = -5f;
        [SerializeField] private float wallJumpForce = 15f;
        [SerializeField] private float wallCheckDistance = 1f;
        [SerializeField] private LayerMask wallLayer;
        
        [Header("Slide Settings")]
        [SerializeField] private float slideDuration = 1.5f;
        [SerializeField] private float slideCooldown = 1f;
        
        [Header("Look Settings")]
        [SerializeField] private float mouseSensitivity = 2.5f;
        [SerializeField] private float lookXLimit = 85f;
        
        [Header("Ground Check")]
        [SerializeField] private float groundCheckRadius = 0.4f;
        [SerializeField] private LayerMask groundLayer;

        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform groundCheck;

        private CharacterController characterController;
        private Vector3 moveDirection;
        private Vector3 velocity;
        private Vector3 wallJumpDirection;
        private float rotationX;
        private bool isGrounded;
        private bool isSprinting;
        private bool isWallRunning;
        private bool isSliding;
        private bool canDoubleJump;
        private bool isWallJumping;
        private bool hasWallJump;
        private float slideTimer;
        private float slideCooldownTimer;
        private float currentSpeed;
        private float targetHeight;
        private float slideHeight = 1f;
        private float normalHeight = 2f;
        private Transform wallRunTransform;

        public float MouseSensitivity => mouseSensitivity;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            
            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();
            
            if (groundCheck == null)
            {
                GameObject checkPoint = new GameObject("GroundCheck");
                checkPoint.transform.SetParent(transform);
                checkPoint.transform.localPosition = new Vector3(0, -characterController.center.y, 0);
                groundCheck = checkPoint.transform;
            }

            currentSpeed = walkSpeed;
            wallLayer = ~0;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            CheckGroundStatus();
            CheckWallRun();
            HandleMovement();
            HandleSlide();
            HandleJump();
            HandleLook();
            ApplyGravity();
            HandleCrouch();
        }

        private void CheckGroundStatus()
        {
            Vector3 checkPosition = groundCheck != null ? groundCheck.position : transform.position;
            isGrounded = Physics.CheckSphere(checkPosition, groundCheckRadius, groundLayer);
            
            if (isGrounded)
            {
                canDoubleJump = true;
                isWallRunning = false;
                wallRunTransform = null;
            }
        }

        private void CheckWallRun()
        {
            if (!isGrounded && !isWallRunning && !isWallJumping)
            {
                Vector3[] directions = { transform.forward, transform.right, -transform.right };
                
                foreach (Vector3 dir in directions)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up * 1f, dir, out hit, wallCheckDistance, wallLayer))
                    {
                        if (Vector3.Dot(hit.normal, Vector3.up) < 0.5f)
                        {
                            isWallRunning = true;
                            wallRunTransform = hit.transform;
                            velocity.y = Mathf.Max(velocity.y, -2f);
                            return;
                        }
                    }
                }
            }
            
            if (isWallRunning)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    isWallRunning = false;
                    wallRunTransform = null;
                }
            }
        }

        private void HandleMovement()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            isSprinting = Input.GetKey(KeyCode.LeftShift) && moveZ > 0;

            if (isWallRunning)
            {
                currentSpeed = wallRunSpeed;
                Vector3 wallNormal = wallRunTransform != null ? wallRunTransform.forward : transform.forward;
                moveDirection = (transform.forward + wallNormal).normalized;
                moveDirection.x *= currentSpeed;
                moveDirection.z *= currentSpeed;
            }
            else if (isSliding)
            {
                currentSpeed = slideSpeed;
                moveDirection = transform.forward * currentSpeed;
            }
            else
            {
                currentSpeed = isGrounded ? (isSprinting ? sprintSpeed : walkSpeed) : airSpeed;
                Vector3 move = transform.right * moveX + transform.forward * moveZ;
                moveDirection.x = move.x * currentSpeed;
                moveDirection.z = move.z * currentSpeed;
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void HandleSlide()
        {
            if (slideCooldownTimer > 0)
                slideCooldownTimer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.C) && !isSliding && slideCooldownTimer <= 0 && isGrounded)
            {
                StartSlide();
            }

            if (isSliding)
            {
                slideTimer -= Time.deltaTime;
                
                if (slideTimer <= 0 || Input.GetKeyUp(KeyCode.C))
                {
                    StopSlide();
                }
            }
        }

        private void StartSlide()
        {
            isSliding = true;
            slideTimer = slideDuration;
            targetHeight = slideHeight;
            currentSpeed = slideSpeed;
            
            if (isSprinting)
            {
                velocity = transform.forward * slideSpeed;
            }
        }

        private void StopSlide()
        {
            isSliding = false;
            slideCooldownTimer = slideCooldown;
            targetHeight = normalHeight;
        }

        private void HandleCrouch()
        {
            if (Input.GetKey(KeyCode.C) && isGrounded && !isSliding)
            {
                targetHeight = slideHeight;
                currentSpeed = walkSpeed * 0.5f;
            }
            else if (!isSliding)
            {
                targetHeight = normalHeight;
            }

            float currentHeight = characterController.height;
            float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * 10f);
            
            characterController.height = newHeight;
            characterController.center = new Vector3(0, newHeight / 2f - 0.5f, 0);
            
            if (playerCamera != null)
            {
                Vector3 camPos = playerCamera.transform.localPosition;
                camPos.y = Mathf.Lerp(camPos.y, targetHeight - 0.2f, Time.deltaTime * 10f);
                playerCamera.transform.localPosition = camPos;
            }
        }

        private void HandleJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    velocity.y = jumpForce;
                }
                else if (canDoubleJump)
                {
                    velocity.y = doubleJumpForce;
                    canDoubleJump = false;
                }
                else if (isWallRunning)
                {
                    WallJump();
                }
            }
        }

        private void WallJump()
        {
            if (wallRunTransform != null)
            {
                Vector3 wallNormal = wallRunTransform.forward;
                wallJumpDirection = (wallNormal + Vector3.up).normalized;
                velocity = wallJumpDirection * wallJumpForce;
                isWallJumping = true;
                isWallRunning = false;
                wallRunTransform = null;
                
                Invoke("ResetWallJump", 0.3f);
            }
        }

        private void ResetWallJump()
        {
            isWallJumping = false;
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            if (playerCamera != null)
            {
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            }
            
            if (!isWallJumping)
            {
                transform.Rotate(Vector3.up * mouseX);
            }
        }

        private void ApplyGravity()
        {
            float gravityMultiplier = isWallRunning ? wallRunGravity : gravity;
            
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravityMultiplier * Time.deltaTime;
            
            if (!isSliding || !isGrounded)
            {
                characterController.Move(velocity * Time.deltaTime);
            }
        }

        public void SetSensitivity(float sensitivity)
        {
            mouseSensitivity = sensitivity;
        }

        public void EnableControls(bool enable)
        {
            enabled = enable;
            Cursor.lockState = enable ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !enable;
        }
    }
}
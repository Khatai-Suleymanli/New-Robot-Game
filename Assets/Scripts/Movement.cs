using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] bool cursorLock = true;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float crouchMultiplier = 0.5f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f; //?
    [SerializeField] float gravity = -30f;//?
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float jumpHeight = 6f;
    [SerializeField] float jumpCooldown = 0.2f;

    [Header("Crouch")]
    [SerializeField] float crouchHeight = 1.0f;
    [SerializeField] float crouchTransitionSpeed = 8f;

    [Header("Sprint FOV")]
    [SerializeField] Camera mainCam;
    [SerializeField] float baseFOV = 60f;
    [SerializeField] float sprintFOV = 75f;
    [SerializeField] float fovTransitionSpeed = 8f;

    float velocityY;
    bool isGrounded;
    float jumpTimer = 0f;
    float cameraCap; //?
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    float originalHeight; // crouch 

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
        HandleCrouch();
        HandleSprintFOV();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);
        jumpTimer -= Time.deltaTime;

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && currentDir.y > 0)
            speed *= sprintMultiplier;

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
            speed *= crouchMultiplier;

        velocityY += gravity * 2f * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        if (isGrounded)
        {
            if (controller.velocity.y < -1f)
                velocityY = -8f;//?

            if (Input.GetButton("Jump") && jumpTimer <= 0f)
            {
                velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpTimer = jumpCooldown;
            }
        }
    }

    void HandleCrouch()
    {
        float targetHeight = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C)) ? crouchHeight : originalHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
    }

    void HandleSprintFOV()
    {
        float targetFOV = (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0) ? sprintFOV : baseFOV;
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
    }
}
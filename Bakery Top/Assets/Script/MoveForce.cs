﻿using UnityEngine;

public class MoveForce : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float currentSpeed;
    public float speed;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Camera Settings")]
    [SerializeField] Transform cameraTransform;

    [Header("Jump")]
    [SerializeField] float jumpForce = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [Header("Animation")]
    private Animator animator;
    [Header("Kopf-Rotation")]
    public Transform headTransform;

    [Header("Inventory")]
    public InventoryManager inventoryManager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        
        cameraTransform = Camera.main.transform;

        animator = GetComponent<Animator>();
        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }

        RotatePlayerToCamera();
        UpdateAnimation();
        //RotateHeadToCamera();

    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        UpdateAnimation();
    }

    private void RotatePlayerToCamera()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;

        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            rb.MoveRotation(targetRotation);
        }
    }
    private void UpdateAnimation()
    {
        if (animator == null) return;

        // Berechne die Geschwindigkeit
        speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        // Setze die Animator-Parameter
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsMoving", speed > 0.1f);
        if (!grounded)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
    }
}

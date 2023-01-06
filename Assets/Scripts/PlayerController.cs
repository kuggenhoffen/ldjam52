using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private PlayerInput input;
    private float rotY = 0f;
    private Transform cameraTransform;
    private CursorLockMode lockMode;
    private bool cursorInit;
    private Vector3 velocity;
    private float moveSpeed = 10f;
    

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        velocity = Vector3.zero;
    }

    void OnEnable()
    {
        if (!input) {
            input = GetComponent<PlayerInput>();
        }
        lockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
        cursorInit = false;
    }

    void OnDisable()
    {
        input.actions.Disable();
        Cursor.lockState = lockMode;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputMove = input.actions["Move"].ReadValue<Vector2>();
        Vector2 inputLook = input.actions["Look"].ReadValue<Vector2>();
        bool grounded = characterController.isGrounded;
        Vector3 moveVec = transform.right * inputMove.x + transform.forward * inputMove.y;

        if (grounded && velocity.y < 0f) {
            velocity.y = 0f;
        }

        characterController.Move(moveVec * moveSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * inputLook.x * Time.deltaTime);

        if (cursorInit) {
            rotY += inputLook.y * Time.deltaTime;
            rotY = Mathf.Clamp(rotY, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(Vector3.left * rotY);
        }
        else {
            cursorInit = true;
        }

        if (!characterController.isGrounded) {
            moveVec -= Physics.gravity * Time.deltaTime;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity);
    }
}
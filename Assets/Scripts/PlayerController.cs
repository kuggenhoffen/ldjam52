using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private float rotY = 0f;
    private Transform cameraTransform;
    private Vector3 velocity;
    private float moveSpeed = 10f;
    private Interactable targetObject = null;

    private Vector2 inputMove;
    private Vector2 inputLook;
    private bool inputInteract;
    private bool inputDrop;
    public Animator toolAnimator;
    private PickupableObject heldObject;
    public Transform toolProxy;
    

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // = input.actions["Move"].ReadValue<Vector2>();
        //Vector2 inputLook = input.actions["Look"].ReadValue<Vector2>();
        bool grounded = characterController.isGrounded;
        Vector3 moveVec = transform.right * inputMove.x + transform.forward * inputMove.y;

        if (grounded && velocity.y < 0f) {
            velocity.y = 0f;
        }

        characterController.Move(moveVec * moveSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * inputLook.x * Time.deltaTime);

        //if (cursorInit) {
            rotY += inputLook.y * Time.deltaTime;
            rotY = Mathf.Clamp(rotY, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(Vector3.left * rotY);
        /*}
        else {
            cursorInit = true;
        }*/

        if (!characterController.isGrounded) {
            moveVec -= Physics.gravity * Time.deltaTime;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity);

        if (inputDrop)
        {
            DropObject();
        }

        toolAnimator.SetBool("ActivateTool", inputInteract);



        inputLook = Vector2.zero;
    }

    public void OnInputMove(InputAction.CallbackContext ctx)
    {
        inputMove = ctx.ReadValue<Vector2>();
    }

    public void OnInputLook(InputAction.CallbackContext ctx)
    {
        inputLook = ctx.ReadValue<Vector2>();
    }

    public void OnInputInteract(InputAction.CallbackContext ctx)
    {
        inputInteract = (ctx.performed && ctx.ReadValue<float>() > 0.5f);
    }

    
    public void OnInputDrop(InputAction.CallbackContext ctx)
    {
        inputDrop = (ctx.performed && ctx.ReadValue<float>() > 0.5f);
    }

    public void OnInteractAnimation()
    {
        if (targetObject) {
            if (targetObject.GetInteractType() == Interactable.InteractType.Pickup && heldObject == null) {
                targetObject.Interact(heldObject);
                PickupObject((PickupableObject)targetObject);
                targetObject = null;
            }
            else if (targetObject.GetInteractType() == Interactable.InteractType.Action) {
                targetObject.Interact(heldObject);
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        targetObject = null;
        int layerMask = LayerMask.GetMask("Ignore Raycast");
        layerMask = ~layerMask;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 2.1f, layerMask)) {
            Debug.Log("hit " + hit.transform.name);
            Interactable obj = hit.transform.GetComponentInParent<Interactable>();
            if (obj != null) {
                targetObject = obj;
            }
        }
    }

    void PickupObject(PickupableObject obj)
    {
        heldObject = obj;
        obj.Pickup(toolProxy);
    }

    void DropObject()
    {
        if (heldObject) {
            heldObject.Drop(toolProxy);
            heldObject = null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupableObject : Interactable
{

    private Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override InteractType GetInteractType()
    {
        return InteractType.Pickup;
    }

    public override void Interact()
    {
        
    }

    private void SetLayerForChildrenRecursive(GameObject obj, int layer)
    {
        foreach(Transform tf in obj.GetComponentsInChildren<Transform>()) {
            tf.gameObject.layer = layer;
        }
    }

    public void Pickup(Transform holder)
    {
        transform.parent = holder;
        rb.isKinematic = true;
        transform.position = holder.position;
        transform.rotation = holder.rotation;
        SetLayerForChildrenRecursive(transform.gameObject, LayerMask.NameToLayer("Hands"));
    }

    public void Drop(Transform holder)
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        SetLayerForChildrenRecursive(transform.gameObject, LayerMask.NameToLayer("Default"));
    }

}

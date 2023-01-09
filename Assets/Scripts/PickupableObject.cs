using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupableObject : Interactable
{

    public enum PickupableObjectType {
        Generic,
        Hammer,
        Sickle,
        Hoe,
        WateringCan,
        Seeds,
        Potato,
        Book
    };

    protected Rigidbody rb;
    public PickupableObjectType pickupObjectType;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override InteractType GetInteractType()
    {
        return InteractType.Pickup;
    }

    public override bool Interact(PickupableObject tool)
    {
        return (tool == null);
    }

    public override bool Interact(PickupableObject tool, Vector3 interactLocation, Vector3 sourceLocation)
    {
        return Interact(tool);
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
        rb.AddForce(holder.forward * 10f, ForceMode.Impulse);
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        SetLayerForChildrenRecursive(transform.gameObject, LayerMask.NameToLayer("Pickupable"));
    }

}

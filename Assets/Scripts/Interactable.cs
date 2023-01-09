using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public enum InteractType {
        Pickup,
        Action
    };


    public virtual InteractType GetInteractType()
    {
        throw new System.NotImplementedException();
    }
    

    public virtual bool Interact(PickupableObject tool)
    {
        return false;
    }

    public virtual bool Interact(PickupableObject tool, Vector3 interactLocation, Vector3 sourceLocation)
    {
        return false;
    }
}

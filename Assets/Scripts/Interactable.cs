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
        return Interact(tool, Vector3.zero, Vector3.zero);
    }

    public virtual bool Interact(PickupableObject tool, Vector3 interactLocation, Vector3 sourceLocation)
    {
        return false;
    }
}

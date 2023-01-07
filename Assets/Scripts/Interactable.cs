using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public enum InteractType {
        Pickup,
        Action
    };


    public abstract InteractType GetInteractType();
    
    public abstract void Interact();
}

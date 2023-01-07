using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoHopper : Interactable
{

    public GameController gameController;

    List<Collider> colliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override InteractType GetInteractType()
    {
        return InteractType.Action;
    }

    public override bool Interact(PickupableObject tool)
    {
        if (tool != null) {
            return false;
        }

        foreach (Collider col in colliders) {
            Destroy(col.gameObject);
            gameController.SellPotato();
        }
        colliders.Clear();

        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PickupableObject>(out PickupableObject pickupable)) {
            if (pickupable.pickupObjectType == PickupableObject.PickupableObjectType.Potato) {
                colliders.Add(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }
}

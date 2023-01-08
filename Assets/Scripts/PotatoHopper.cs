using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoHopper : MonoBehaviour
{
    List<Collider> colliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPotatoCount()
    {
        return colliders.Count;
    }

    public void ClearPotatoes()
    {
        foreach (Collider col in colliders) {
            Destroy(col.gameObject);
        }
        colliders.Clear();
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

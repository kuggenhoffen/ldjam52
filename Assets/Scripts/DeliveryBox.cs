using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBox : Interactable
{

    public List<GameObject> items = new List<GameObject>();

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

        Vector3 pos = transform.position;

        Destroy(gameObject);

        foreach (GameObject obj in items) {
            Instantiate(obj, pos, Quaternion.identity);
        }

        return true;
    }
}

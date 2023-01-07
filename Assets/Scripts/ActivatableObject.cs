using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableObject : PickupableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Activate()
    {
        throw new System.NotImplementedException();
    }
}

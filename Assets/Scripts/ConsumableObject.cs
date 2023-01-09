using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableObject : PickupableObject
{

    [SerializeField]
    int uses;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InteractResult(Interactable other, bool success)
    {
        if (success) {
            uses--;
        }
    }

    public bool isConsumed() {
        return uses <= 0;
    }
}

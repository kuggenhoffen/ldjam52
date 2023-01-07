using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{

    public GameController gameController;

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

        gameController.SleepInBed();

        return true;
    }

}

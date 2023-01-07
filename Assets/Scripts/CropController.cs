using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : Interactable
{

    public enum PlotState {
        EMPTY,
        SEED,
        GROWING,
        READY,
        ROCK,
        WEED
    };

    public GameObject rockObject;
    public GameObject weedObject;
    public PlotState state;

    private const int rockInteractLimit = 4;
    private const int weedInteractLimit = 4;
    private const float interactTimeoutLimit = 1f; 
    private int interactCount;
    private float interactTimeout = 0f;

    private Dictionary<PlotState, PickupableObject.PickupableObjectType> toolsForStates = new Dictionary<PlotState, PickupableObject.PickupableObjectType>() { 
        { PlotState.ROCK, PickupableObject.PickupableObjectType.Hammer },
        { PlotState.WEED, PickupableObject.PickupableObjectType.Sickle },
    };



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        interactTimeout -= Time.deltaTime;
        if (interactTimeout <= 0f) {
            Reset();
        }
    }

    public override InteractType GetInteractType()
    {
        return InteractType.Action;
    }

    bool IsCorrectTool(PickupableObject tool)
    {
        PickupableObject.PickupableObjectType pt = toolsForStates.GetValueOrDefault(state, PickupableObject.PickupableObjectType.Generic);
        return (pt != PickupableObject.PickupableObjectType.Generic) && (tool.pickupObjectType == pt);
    }

    public override bool Interact(PickupableObject tool)
    {
        bool res = false;
        switch (state) {
            case PlotState.ROCK:
            case PlotState.WEED:
                if (IsCorrectTool(tool)) {
                    interactCount -= 1;
                    if (interactCount <= 0) {
                        state = PlotState.EMPTY;
                        res = true;
                        Reset();
                    }
                    else {
                        interactTimeout = interactTimeoutLimit;
                    }
                }
                break;
            default:
                break;
        }
        return res;
    }

    private void Reset()
    {
        switch (state) {
            case PlotState.ROCK:
                weedObject.SetActive(false);
                rockObject.SetActive(true);
                interactCount = rockInteractLimit;
                break;
            case PlotState.WEED:
                weedObject.SetActive(true);
                rockObject.SetActive(false);
                interactCount = weedInteractLimit;
            break;
            default:
                weedObject.SetActive(false);
                rockObject.SetActive(false);
            break;
            
        }
    }
}

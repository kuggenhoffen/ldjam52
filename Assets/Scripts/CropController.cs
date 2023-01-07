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

    public override void Interact()
    {
        interactCount -= 1;
        if (interactCount <= 0) {
            HandleInteract();
        }
        else {
            interactTimeout = interactTimeoutLimit;
        }
    }

    private void HandleInteract()
    {
        switch (state) {
            case PlotState.ROCK:
                state = PlotState.EMPTY;
                break;
            case PlotState.WEED:
                state = PlotState.EMPTY;
            break;
            default:
            break;
        }
        Reset();
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

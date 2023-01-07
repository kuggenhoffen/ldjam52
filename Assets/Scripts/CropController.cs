using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : Interactable
{

    public enum PlotState {
        Empty,
        Shaped,
        Sown,
        Growing,
        Ready,
        Rock,
        Weed
    };

    public GameObject rockObject;
    public GameObject weedObject;
    public GameObject cropObject;
    public GameObject growingObject;
    public GameObject shapedObject;
    public GameObject sownObject;
    public GameObject potatoPrefab;

    public PlotState state;
    public bool watered;

    private const int rockInteractLimit = 4;
    private const int weedInteractLimit = 2;
    private const int emptyInteractLimit = 3;
    private const int shapedInteractLimit = 1;
    private const int wateringInteractLimit = 1;
    private const int readyInteractLimit = 1;
    private const float interactTimeoutLimit = 1f; 
    private const int growLimitFinished = 3;
    private const int growLimitIntermediate = 1;
    private int growCount = 0;
    private int interactCount;
    private float interactTimeout = 0f;

    private Dictionary<PlotState, PickupableObject.PickupableObjectType> toolsForStates = new Dictionary<PlotState, PickupableObject.PickupableObjectType>() { 
        { PlotState.Rock, PickupableObject.PickupableObjectType.Hammer },
        { PlotState.Weed, PickupableObject.PickupableObjectType.Sickle },
        { PlotState.Empty, PickupableObject.PickupableObjectType.Hoe },
        { PlotState.Shaped, PickupableObject.PickupableObjectType.Seeds },
        { PlotState.Sown, PickupableObject.PickupableObjectType.WateringCan },
        { PlotState.Growing, PickupableObject.PickupableObjectType.WateringCan },
        { PlotState.Ready, PickupableObject.PickupableObjectType.Hoe }
    };

    private const float weedChance = 0.5f;

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
        return (tool != null) && (pt != PickupableObject.PickupableObjectType.Generic) && (tool.pickupObjectType == pt);
    }

    public override bool Interact(PickupableObject tool)
    {
        bool res = false;
        switch (state) {
            case PlotState.Rock:
            case PlotState.Weed:
            case PlotState.Empty:
            case PlotState.Sown:
            case PlotState.Shaped:
            case PlotState.Growing:
            case PlotState.Ready:
                if (IsCorrectTool(tool)) {
                    interactCount -= 1;
                    if (interactCount <= 0) {
                        HandleInteract();
                        res = true;
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

    private void HandleInteract()
    {
        bool spawnPotato = false;
        switch (state) {
            case PlotState.Empty:
                state = PlotState.Shaped;
                break;
            case PlotState.Rock:
            case PlotState.Weed:
                state = PlotState.Empty;
                break;
            case PlotState.Shaped:
                state = PlotState.Sown;
                growCount = 0;
                break;
            case PlotState.Sown:
                watered = true;
                break;
            case PlotState.Growing:
                watered = true;
                break;
            case PlotState.Ready:
                state = PlotState.Empty;
                spawnPotato = true;
                break;
            default:
                break;
        }
        
        Reset();
        if (spawnPotato) {
            Instantiate(potatoPrefab, transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        }
    }

    private void Reset()
    {
        weedObject.SetActive(false);
        rockObject.SetActive(false);
        cropObject.SetActive(false);
        growingObject.SetActive(false);
        shapedObject.SetActive(false);
        sownObject.SetActive(false);
        switch (state) {
            case PlotState.Empty:
                interactCount = emptyInteractLimit;
                break;
            case PlotState.Rock:
                rockObject.SetActive(true);
                interactCount = rockInteractLimit;
                break;
            case PlotState.Weed:
                weedObject.SetActive(true);
                interactCount = weedInteractLimit;
                break;
            case PlotState.Shaped:
                shapedObject.SetActive(true);
                interactCount = shapedInteractLimit;
                break;
            case PlotState.Sown:
                sownObject.SetActive(true);
                interactCount = wateringInteractLimit;
                break;
            case PlotState.Growing:
                growingObject.SetActive(true);
                interactCount = wateringInteractLimit;
                break;
            case PlotState.Ready:
                cropObject.SetActive(true);
                interactCount = readyInteractLimit;
                break;
            default:
                break;
            
        }
    }

    public void RoundEnd()
    {
        Debug.Log("Doing round end stuff, state is " + state.ToString());
        switch (state) {
            case PlotState.Empty:
            case PlotState.Shaped:
                // Grow weed for empty or shaped plots
                if (Random.Range(0f, 1f) >= weedChance) {
                    state = PlotState.Weed;
                }
                break;
            case PlotState.Sown:
            case PlotState.Growing:
                // Non-watered sown plots get cleared
                if (!watered) {
                    state = PlotState.Empty;
                }
                else {
                    
                    if (growCount >= growLimitFinished) {
                        state = PlotState.Ready;
                    }
                    else if (growCount >= growLimitIntermediate) {
                        state = PlotState.Growing;
                    }
                    growCount++;
                }
                break;
            default:
                break;
        }

        watered = false;

        Reset();
    }
}

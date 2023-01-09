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

    public GameObject plotObject;
    public GameObject rockObject;
    public GameObject weedObject;
    public GameObject[] cropObjects;
    public GameObject shapedObject;
    public GameObject sownObject;
    public GameObject potatoPrefab;
    public GameObject soilHitParticles;
    public GameObject plantHitParticles;
    public GameObject rockHitParticles;
    public Material dryMaterial;
    public Material wetMaterial;

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

    private void SpawnParticles(Vector3 position, Vector3 direction)
    {
        GameObject particles = null;
        switch (state) {
            case PlotState.Rock:
                particles = rockHitParticles;
                break;
            case PlotState.Weed:
            case PlotState.Ready:
                particles = plantHitParticles;
                break;
            case PlotState.Empty:
                particles = soilHitParticles;
                break;
            case PlotState.Sown:
            case PlotState.Shaped:
            case PlotState.Growing:
            default:
                break;
        }
        if (particles != null) {
            Instantiate(particles, position, Quaternion.identity);
        }
    }

    public override bool Interact(PickupableObject tool, Vector3 interactLocation, Vector3 sourceLocation)
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
                    SpawnParticles(interactLocation, sourceLocation - interactLocation);
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
                SetMaterial(watered);
                break;
            case PlotState.Growing:
                watered = true;
                SetMaterial(watered);
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

    private void SetMaterial(bool watered)
    {
        List<MeshRenderer> plotRenderers = new List<MeshRenderer>();
        plotRenderers.AddRange(plotObject.GetComponentsInChildren<MeshRenderer>());
        plotRenderers.AddRange(sownObject.GetComponentsInChildren<MeshRenderer>());
        plotRenderers.AddRange(shapedObject.GetComponentsInChildren<MeshRenderer>());
        foreach (MeshRenderer mr in plotRenderers) {
            mr.material = watered ? wetMaterial : dryMaterial;
        }
    }

    private void Reset()
    {

        bool plotEnable = false;
        bool rockEnable = false;
        bool weedEnable = false;
        bool shapeEnable = false;
        bool cropEnable = false;
        bool sownEnable = false;

        switch (state) {
            case PlotState.Empty:
                plotEnable = true;
                interactCount = emptyInteractLimit;
                break;
            case PlotState.Rock:
                plotEnable = true;
                rockEnable = true;
                interactCount = rockInteractLimit;
                break;
            case PlotState.Weed:
                plotEnable = true;
                weedEnable = true;
                interactCount = weedInteractLimit;
                break;
            case PlotState.Shaped:
                shapeEnable = true;
                interactCount = shapedInteractLimit;
                break;
            case PlotState.Sown:
                sownEnable = true;
                interactCount = wateringInteractLimit;
                break;
            case PlotState.Growing:
                cropEnable = true;
                sownEnable = true;
                interactCount = wateringInteractLimit;
                break;
            case PlotState.Ready:
                cropEnable = true;
                sownEnable = true;
                interactCount = readyInteractLimit;
                break;
            default:
                break;
            
        }

        SetObjectActive(plotObject, plotEnable);
        SetObjectActive(weedObject, weedEnable);
        SetObjectActive(rockObject, rockEnable);
        SetObjectActive(shapedObject, shapeEnable);
        SetObjectActive(sownObject, sownEnable);
        SetCropObject(cropEnable);
    }

    void SetCropObject(bool enable)
    {
        int i = 0;
        foreach (GameObject obj in cropObjects) {
            if (i == growCount && enable) {
                SetObjectActive(obj, true);
            }
            else {
                SetObjectActive(obj, false);
            }
            i++;
        }
    }

    void SetObjectActive(GameObject obj, bool enable)
    {
        if (obj.activeInHierarchy != enable) {
            obj.SetActive(enable);
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
                // Non-watered sown plots get cleared
                if (false && !watered) {
                    state = PlotState.Empty;
                }
                else {
                    state = PlotState.Growing;
                }
                break;
            case PlotState.Growing:
                // Non-watered sown plots get cleared
                if (false && !watered) {
                    state = PlotState.Empty;
                }
                else {
                    if (growCount >= growLimitFinished) {
                        state = PlotState.Ready;
                    }
                    growCount++;
                }
                break;
            default:
                break;
        }

        watered = false;
        SetMaterial(watered);

        Reset();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
[RequireComponent(typeof(PlayerInput))]
public class GameController : MonoBehaviour
{

    private PlayerInput input;
    private CursorLockMode lockMode;
    private CropController[] plots;
    private const float weedDistribution = 0.3f;
    private const float rockDistribution = 0.3f;

    private int money = 100;
    private int daysLeft = 2;

    public Transform padUI;
    public Transform cursorUI; 
    public Transform pageWelcome;
    public Transform pagePotato;
    public Transform pageShop;
    public Transform btnPotato;
    private bool padVisible = true;

    public Blackout blackout;

    List<GameObject> orderedItems = new List<GameObject>();

    public Transform deliveryProxy;
    public GameObject deliveryBoxPrefab;
    public PotatoHopper hopper;

    // Start is called before the first frame update
    void Start()
    {        
        plots = FindObjectsOfType<CropController>();
        InitializeCrops();
        pagePotato.gameObject.SetActive(false);
        pageWelcome.gameObject.SetActive(true);
        pageShop.gameObject.SetActive(false);
        btnPotato.gameObject.SetActive(false);
        HidePadUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnEnable()
    {
        if (!input) {
            input = GetComponent<PlayerInput>();
        }
    }

    void OnDisable()
    {
        input.actions.Disable();
    }

    public void OnDebugInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<float>() > 0.5f) {
            EndRound();
        }
    }

    public void OnPadOpenInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<float>() > 0.5f) {
            if (padVisible) {
                HidePadUI();
            }
            else {
                ShowPadUI();
            }
        }
    }

    public void ShowPadUI()
    {
        padUI.gameObject.SetActive(true);
        cursorUI.gameObject.SetActive(false);
        Cursor.lockState = lockMode;
        input.SwitchCurrentActionMap("UI");
        padVisible = true;
    }

    public void HidePadUI()
    {
        padUI.gameObject.SetActive(false);
        cursorUI.gameObject.SetActive(true);
        lockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
        input.SwitchCurrentActionMap("Player");
        padVisible = false;
    }

    void InitializeCrops()
    {
        int n = plots.Length;
        while (n > 0) {
            n--;
            int i = Random.Range(0, n + 1);
            CropController temp = plots[i];
            plots[i] = plots[n];
            plots[n] = temp;
        }

        n = 0;
        for (int count = Mathf.CeilToInt(weedDistribution * plots.Length); n < plots.Length && count > 0; n++, count--) {
            plots[n].state = CropController.PlotState.Weed;
        }
        
        for (int count = Mathf.CeilToInt(rockDistribution * plots.Length); n < plots.Length && count > 0; n++, count--) {
            plots[n].state = CropController.PlotState.Rock;
        }
    }

    void EndRound()
    {
        input.enabled = false;
        if (daysLeft > 0) {
            blackout.SetText(string.Format("{0} {1} until winter.", daysLeft, daysLeft > 1 ? "days" : "day"));
        }
        else {
            blackout.SetText(string.Format("Winter has arrived."));
        }
        daysLeft -= 1;
        blackout.DoBlackout(ProcessRoundEnd, BlackoutFinishedCallback);
    }

    void ProcessRoundEnd()
    {
        foreach (CropController crop in plots) {
            crop.RoundEnd();
        }
        if (orderedItems.Count > 0) {
            GameObject deliveryBox = Instantiate(deliveryBoxPrefab, deliveryProxy.position, Quaternion.identity);
            deliveryBox.GetComponent<DeliveryBox>().items.AddRange(orderedItems);
        }
        orderedItems.Clear();
        
        int potatoCount = hopper.GetPotatoCount();
        money += potatoCount * 20;
        hopper.ClearPotatoes();
    }

    void BlackoutFinishedCallback()
    {
        input.enabled = true;
    }

    public void SleepInBed()
    {
        EndRound();
    }

    public void OnClickBtnClose()
    {
        HidePadUI();
    }

    public void OnClickBtnQuit()
    {
        Application.Quit();
    }

    public void OnClickBtnPotatoFarming()
    {
        pagePotato.gameObject.SetActive(true);
        pageWelcome.gameObject.SetActive(false);
        pageShop.gameObject.SetActive(false);
    }

    public void OnClickBtnWelcome()
    {
        pagePotato.gameObject.SetActive(false);
        pageWelcome.gameObject.SetActive(true);
        pageShop.gameObject.SetActive(false);
    }

    public void OnClickBtnShop()
    {
        pagePotato.gameObject.SetActive(false);
        pageWelcome.gameObject.SetActive(false);
        pageShop.gameObject.SetActive(true);
    }

    public void OnOrderItem(ShopItem shopItem)
    {
        if (money - shopItem.price >= 0) {
            // Ehh, so nasty but took too much time trying to figure out how to define item specific callback in the item descriptor in prefab.
            // Probably it should not be defined in prefab as a UnityEvent but do it in some other way.
            if (shopItem.itemName == "Potato farming instructions") {
                btnPotato.gameObject.SetActive(true);
            }
            else {
                if (shopItem.prefab != null) {
                    orderedItems.Add(shopItem.prefab);
                }
            }
            Debug.Log("Bought item " + shopItem.itemName);
            money -= shopItem.price;
        }
        else {
            // Show tooltip?
        }
    }


}

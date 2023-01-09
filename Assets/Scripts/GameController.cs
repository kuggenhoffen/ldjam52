using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[System.Serializable]
[RequireComponent(typeof(PlayerInput))]
public class GameController : MonoBehaviour
{

    private PlayerInput input;
    private CropController[] plots;
    public PlayerController playerController;
    private const float weedDistribution = 0.3f;
    private const float rockDistribution = 0.3f;
    private const float roundTime = 2 * 60f;
    private float roundTimer = 0f;
    
    private const int seasonLength = 2;
    private int daysLeft = seasonLength;
    private int potatoesHarvested = 0;

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
    public Transform playerSpawn;
    public GameObject player;
    public RectTransform timerNeedle;
    public TMPro.TMP_Text moneyText;
    bool initialBlackout = true;

    PersistentData persistentData;

    // Start is called before the first frame update
    void Start()
    {        
        if (!input) {
            input = GetComponent<PlayerInput>();
        }
        persistentData = GameObject.FindGameObjectWithTag("PersistentData").GetComponent<PersistentData>();
        plots = FindObjectsOfType<CropController>();
        InitializeCrops();
        pagePotato.gameObject.SetActive(false);
        pageWelcome.gameObject.SetActive(true);
        pageShop.gameObject.SetActive(false);
        btnPotato.gameObject.SetActive(false);
        HidePadUI();
        timerNeedle.eulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.isActiveAndEnabled) {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 0f) {
                EndRound(initialBlackout);
                initialBlackout = false;
            }
        }
        timerNeedle.eulerAngles = Vector3.forward * Mathf.Lerp(0f, -180f, (roundTime - roundTimer) / roundTime);
        moneyText.SetText(string.Format("{0} $", persistentData.money));
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

    void SwitchInput(string inputMap)
    {

        bool inputEnabled = input.isActiveAndEnabled;
        input.ActivateInput();
        input.SwitchCurrentActionMap(inputMap);
        if (!inputEnabled) {
            input.DeactivateInput();
        }
    }

    public void ShowPadUI()
    {
        btnPotato.gameObject.SetActive(persistentData.potatoInstructionsBought);
        padUI.gameObject.SetActive(true);
        cursorUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        SwitchInput("UI");
        padVisible = true;
    }

    public void HidePadUI()
    {
        padUI.gameObject.SetActive(false);
        cursorUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        SwitchInput("Player");
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
            temp.gameController = this;
        }

        n = 0;
        for (int count = Mathf.CeilToInt(weedDistribution * plots.Length); n < plots.Length && count > 0; n++, count--) {
            plots[n].state = CropController.PlotState.Weed;
        }
        
        for (int count = Mathf.CeilToInt(rockDistribution * plots.Length); n < plots.Length && count > 0; n++, count--) {
            plots[n].state = CropController.PlotState.Rock;
        }
    }

    void EndRound(bool initial = false)
    {
        input.DeactivateInput();
        if (daysLeft > 0) {
            blackout.SetText(string.Format("Season {0}\n\n{1} {2} until winter.", persistentData.numSeasons, daysLeft, daysLeft > 1 ? "days" : "day"));
        }
        else {
            blackout.SetText(string.Format("Season {2}\n\nWinter has arrived.\nYou harvested {0} potatoes this season and {1} potatoes in total.\n\nPress any key to continue to next season.", potatoesHarvested, persistentData.potatoesHarvested, persistentData.numSeasons));
            SwitchInput("WaitForAnyKey");
            persistentData.numSeasons += 1;
            input.ActivateInput();
        }
        if (initial) {
            blackout.DoFadeOut(ProcessRoundEnd, BlackoutFinishedCallback);
        }
        else {
            blackout.DoFadeIn(ProcessRoundEnd, BlackoutFinishedCallback, daysLeft <= 0);
        }
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

        playerController.DropObject();
        
        int potatoCount = hopper.GetPotatoCount();
        persistentData.money += potatoCount * 20;
        hopper.ClearPotatoes();

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerSpawn.position;
        player.transform.forward = playerSpawn.forward;
        player.GetComponent<CharacterController>().enabled = true;

        roundTimer = roundTime;

        if (daysLeft == seasonLength) {
            ShowPadUI();
        }
        else {
            HidePadUI();
        }

        daysLeft -= 1;
    }

    void BlackoutFinishedCallback()
    {
        Debug.Log("Blackout finished");
        input.ActivateInput();
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
        SceneManager.LoadScene("TitleScene");
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
        if (persistentData.money - shopItem.price >= 0) {
            // Ehh, so nasty but took too much time trying to figure out how to define item specific callback in the item descriptor in prefab.
            // Probably it should not be defined in prefab as a UnityEvent but do it in some other way.
            if (shopItem.itemName == "Potato farming instructions") {
                persistentData.potatoInstructionsBought = true;
            }
            else {
                if (shopItem.prefab != null) {
                    orderedItems.Add(shopItem.prefab);
                }
            }
            Debug.Log("Bought item " + shopItem.itemName);
            persistentData.money -= shopItem.price;
        }
        else {
            // Show tooltip?
        }
    }

    public void HarvestPotato()
    {
        potatoesHarvested += 1;
        persistentData.potatoesHarvested += 1;
    }

    public void ContinueToNextSeason(InputAction.CallbackContext ctx)
    {
        Debug.Log("Continue to next");
        SceneManager.LoadScene("PlayScene");
    }

}

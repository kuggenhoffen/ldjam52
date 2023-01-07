using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameController : MonoBehaviour
{

    private PlayerInput input;
    private CursorLockMode lockMode;
    private CropController[] plots;
    private const float weedDistribution = 0.3f;
    private const float rockDistribution = 0.3f;

    private int money = 100;

    // Start is called before the first frame update
    void Start()
    {        
        plots = FindObjectsOfType<CropController>();
        InitializeCrops();
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
        lockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        input.actions.Disable();
        Cursor.lockState = lockMode;
    }

    public void OnDebugInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<float>() > 0.5f) {
            EndRound();
        }
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
        foreach (CropController crop in plots) {
            crop.RoundEnd();
        }
    }

    public void SellPotato()
    {
        money += 20;
        Debug.Log("Money is " + money);
    }

}

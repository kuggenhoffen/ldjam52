using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintEvent()
    {
        if (playerController) {
            playerController.OnInteractAnimation();
        }
    }
}

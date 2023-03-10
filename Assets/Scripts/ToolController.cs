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

    public void StartAnimation()
    {
        if (playerController) {
            playerController.OnStartAnimation();
        }
    }

    public void AnimationEvent()
    {
        if (playerController) {
            playerController.OnInteractAnimation();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CropController))]
public class CropRandomizer : MonoBehaviour
{

    CropController crop;

    // Start is called before the first frame update
    void Start()
    {
        crop = GetComponent<CropController>();
        
        crop.state = CropController.PlotState.Ready;
        crop.growCount = Random.Range(2, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

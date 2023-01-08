using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandom : MonoBehaviour
{

    [SerializeField]
    bool RotateAroundXAxis;

    [SerializeField]
    bool RotateAroundYAxis;
    [SerializeField]
    bool RotateAroundZAxis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Debug.Log("Enabled");
        Vector3 rotationVector = Vector3.zero;
        rotationVector += RotateAroundXAxis ? Vector3.right * Random.Range(0f, 360f) : Vector3.zero;
        rotationVector += RotateAroundYAxis ? Vector3.forward * Random.Range(0f, 360f) : Vector3.zero;
        rotationVector += RotateAroundZAxis ? Vector3.up * Random.Range(0f, 360f) : Vector3.zero;
        transform.Rotate(rotationVector);
    }
}

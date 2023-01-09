using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{

    [SerializeField]
    Vector3 rotationRange;

    [SerializeField]
    Vector3 scaleMin = Vector3.one;

    [SerializeField]
    Vector3 scaleMax = Vector3.one;


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
        Vector3 rotationVector = Vector3.zero;
        rotationVector += rotationRange.x * Vector3.right * Random.Range(0f, 360f);
        rotationVector += rotationRange.y * Vector3.forward * Random.Range(0f, 360f);
        rotationVector += rotationRange.z * Vector3.up * Random.Range(0f, 360f);
        transform.Rotate(rotationVector);

        Vector3 scaleVector = new Vector3(
            Random.Range(scaleMin.x, scaleMax.x),
            Random.Range(scaleMin.y, scaleMax.y),
            Random.Range(scaleMin.z, scaleMax.z)
        );
        transform.localScale = scaleVector;
    }
}

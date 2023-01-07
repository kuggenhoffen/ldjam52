using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpawningPickupableObject : PickupableObject
{

    bool spawnFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnFinished) {
            transform.position += Vector3.up * Time.deltaTime * 4f;
        }
    }

    void OnTriggerExit(Collider other) {
        if (!spawnFinished) {
            spawnFinished = true;
            rb.isKinematic = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

 [System.Serializable]
public class ShopItemOrderCallback : UnityEvent <ShopItem> {}

public class ShopItem : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public int price;
    public bool immediate;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        prefab = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

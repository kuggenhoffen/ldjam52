using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class ShopManager : MonoBehaviour
{

    GameController gameController;
    public List<ShopItem> shopItems;
    public GameObject shopPagePanel;
    public GameObject shopItemPanelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        foreach (ShopItem item in shopItems) {
            GameObject itemPanel = Instantiate(shopItemPanelPrefab, Vector3.zero, Quaternion.identity, shopPagePanel.transform);
            itemPanel.GetComponent<ShopItemPanel>().SetItem(item, true, OnItemOrder);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnItemOrder(ShopItem item)
    {
        gameController.OnOrderItem(item);
    }


}

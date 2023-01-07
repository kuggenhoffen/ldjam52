using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{

    public delegate void ItemBuyCallback(ShopItem item);

    public TMPro.TMP_Text TextName;
    public TMPro.TMP_Text TextDesc;
    public TMPro.TMP_Text TextPrice;
    public TMPro.TMP_Text TextBtn;
    public Button BtnOrder;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetItem(ShopItem item, bool enabled, ItemBuyCallback cb)
    {        
        TextName.SetText(item.itemName);
        TextDesc.SetText(item.itemDescription);
        TextPrice.SetText(string.Format("Price\n{0} c", item.price));
        BtnOrder.onClick.AddListener(() => cb.Invoke(item));
        BtnOrder.interactable = enabled;
        TextBtn.SetText(item.immediate ? "Buy" : "Order");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemData : MonoBehaviour
{
    [SerializeField] GameObject shopManagerObj;
    [SerializeField] GameObject shopItemSelector;
    ShopManager shopManager;
    public Item itemData;

    #region Shop Right Panel

    [SerializeField] GameObject _itemTitle;
    [SerializeField] GameObject _itemDescription;
    [SerializeField] GameObject _itemCost;
    #endregion

    void Start()
    {

        shopManagerObj = GameObject.Find("Shop Canvas");
        shopManager = shopManagerObj.GetComponent<ShopManager>();

        _itemTitle = GameObject.Find("Item Title");
        _itemDescription = GameObject.Find("Item Description");
        _itemCost = GameObject.Find("Item Cost");
    }

    public void OnButtonClick()
    {
        if (GameObject.Find("Item Selector(Clone)") == false){
            var newItem = Instantiate(shopItemSelector, transform);            
            newItem.transform.SetParent(shopManagerObj.transform, false);

            shopItemSelector = newItem;
            shopItemSelector.transform.position = transform.position;
        }
        else {
            shopItemSelector = GameObject.Find("Item Selector(Clone)");
            shopItemSelector.transform.position = transform.position;
        }
        shopManager.itemSelected = itemData;

        _itemTitle.GetComponent<TextMeshProUGUI>().text = itemData.itemName;
        _itemDescription.GetComponent<TextMeshProUGUI>().text = itemData.itemDescription;
        _itemCost.GetComponent<TextMeshProUGUI>().text = "Cost: " + itemData.itemCost.ToString();
    }
}

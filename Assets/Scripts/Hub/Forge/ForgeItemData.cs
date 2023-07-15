using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgeItemData : MonoBehaviour
{
    [SerializeField] GameObject shopManagerObj;
    [SerializeField] GameObject shopItemSelector;
    ForgeManager shopManager;
    public Weapon weaponData;
    public Armor armorData;

    #region Shop Right Panel

    [SerializeField] GameObject _itemTitle;
    [SerializeField] GameObject _itemDescription;
    #endregion

    void Start()
    {

        shopManagerObj = GameObject.Find("Forge Canvas");
        shopManager = shopManagerObj.GetComponent<ForgeManager>();

        _itemTitle = GameObject.Find("Item Title");
        _itemDescription = GameObject.Find("Item Description");
        // _itemCost = GameObject.Find("Item Cost");
        // _itemCount = GameObject.Find("Item Count");
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

        switch(shopManager.tabSelected){
            case "Weapon":
                shopManager.weaponSelected = weaponData;

                _itemTitle.GetComponent<TextMeshProUGUI>().text = weaponData.itemName;
                _itemDescription.GetComponent<TextMeshProUGUI>().text = weaponData.itemDescription;
                break;
            case "Armor":
                shopManager.armorSelected = armorData;

                _itemTitle.GetComponent<TextMeshProUGUI>().text = armorData.itemName;
                _itemDescription.GetComponent<TextMeshProUGUI>().text = armorData.itemDescription;
                break;
        }
    }
}

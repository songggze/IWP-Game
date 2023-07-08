using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{

    #region References
    [SerializeField] GameObject hubManagerObj;
    HubManager hubManager;

    [SerializeField] GameObject shopSelectParent;
    #endregion


    #region Shop

    [SerializeField] GameObject itemPanel_prefab;

    #endregion

    #region Variables
    
    List<Item> itemList;
    public Item itemSelected;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
       hubManager = hubManagerObj.GetComponent<HubManager>(); 

        // Get item data which contains scriptable objects
        itemList = Resources.LoadAll<Item>("ItemData").ToList();

        // Create copy to mody the item data
        // List<Item> tempList = new List<Item>(itemList);

        // Remove item from list if itemcount is 0
        int index = 0;
        foreach (var item in itemList){

            GameObject panel = Instantiate(itemPanel_prefab);
            panel.transform.SetParent(shopSelectParent.transform, false);

            panel.GetComponent<ShopItemData>().itemData = item;
            panel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemIcon;

            index++;
        }

        // selectedItem = 0;
        // itemTypeCount = itemList.Count() - 1;

        // // Set initial display for the item holders
        // UpdateHolder();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem()
    {
        // Money reduction goes here...

        if (itemSelected.itemCount < itemSelected.maxCount){

            itemSelected.itemCount++;
            Debug.Log($"Buy: {itemSelected.itemName} , {itemSelected.itemCount}");
        }
        else{

            Debug.Log("Max capacity, cannot purchase");
        }

    }


    public void OnButtonExit()
    {
        hubManager.isSelectedBuilding = false;
        gameObject.SetActive(false);           
    }
}

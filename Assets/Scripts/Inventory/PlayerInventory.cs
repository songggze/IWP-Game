using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class PlayerInventory : MonoBehaviour
{

    // The player inventory
    List<Item> itemList;
    
    // DEBUG
    bool init = false;

    int selectedItem;
    int selectorMaxCount;

    // Item Selector Stuff
    [SerializeField] GameObject itemHolders;

    // Start is called before the first frame update
    void Start()
    {
        // Get item data which contains scriptable objects
        itemList = Resources.LoadAll<Item>("ItemData").ToList();

        // Create copy to mody the item data
        List<Item> tempList = new List<Item>(itemList);

        // Remove item from list if itemcount is 0
        foreach (var item in tempList){
            if (item.itemCount <= 0){
                itemList.Remove(item);
            }
        }

        selectedItem = 0;
        selectorMaxCount = itemList.Count() - 1;

        // Set initial display for the item holders
        UpdateHolder();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!init){
            PrintList();
            init = true;
        }

        // Select Item
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") < 0){
            selectedItem--;
            if (selectedItem < 0){
                selectedItem = selectorMaxCount;
            }
            UpdateHolder();
            Debug.Log($"Selected Item: {itemList[selectedItem].itemName}");
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") > 0){

            selectedItem++;
            if (selectedItem > selectorMaxCount){

                selectedItem = 0;
            }
            UpdateHolder();
            Debug.Log($"Selected Item: {itemList[selectedItem].itemName}");
        }
    }

    //void AddItem(string _itemName, int _itemId, int _count)
    //{

    //}
    
    void PrintList()
    {
        foreach (var item in itemList){

            Debug.Log($"Item: {item.itemName}, {item.itemCount}");
        }
    }

    void UpdateHolder()
    {
        // Update the item icons for all panels based on current selected item
        for (int index = 0; index < itemHolders.transform.childCount; index++){

            var panelIcon = itemHolders.transform.GetChild(index).gameObject.transform.Find("Icon").gameObject;
            panelIcon.GetComponent<Image>().sprite = itemList[SelectedIndex(index - 1)].itemIcon;
        }

        // Update item name and count for selected item
        GameObject.Find("Item Selected Text").GetComponent<TextMeshProUGUI>().text = itemList[selectedItem].itemName;
        GameObject.Find("Item Selected Count").GetComponent<TextMeshProUGUI>().text = itemList[selectedItem].itemCount.ToString();
    }

    int SelectedIndex(int offset)
    {
        int result = selectedItem + offset;

        // "Wrapping" if overshoot    
        if (result < 0){
            result = selectorMaxCount;
        }
        if (result > selectorMaxCount){

            result = 0;
        }

        return result;
    }
}

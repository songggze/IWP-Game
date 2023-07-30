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
    int itemTypeCount;

    // Item Selector Stuff
    [SerializeField] GameObject itemHolders;
    [SerializeField] GameObject canvas;

    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    [SerializeField] GameObject _healingPrefab;

    // Shock Trap
    [SerializeField] GameObject _shockTrapPrefab;

    // Start is called before the first frame update
    void Start()
    { 
        playerStats = player.GetComponent<PlayerStats>();

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
        itemTypeCount = itemList.Count() - 1;

        // Set initial display for the item holders
        UpdateHolder();
    }

    // Update is called once per frame
    void Update()
    {
        
        // To show list at start of game
        if (!init){
            PrintList();
            init = true;
        }

        // Select Item
        if (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") < 0){
            selectedItem++;
            if (selectedItem > itemTypeCount){

                selectedItem = 0;
            }
            UpdateHolder();
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0){

            selectedItem--;
            if (selectedItem < 0){
                selectedItem = itemTypeCount;
            }
            UpdateHolder();
        }

        if (playerStats.isHealing){

            if (playerStats.healTimer <= 0 && !playerStats.healEffect)
            {
                Instantiate(_healingPrefab, player.transform);
                playerStats.healEffect = true;
                ConsumeItem();
            }
        }

        // Use item
        if (Input.GetKeyDown(KeyCode.F) && !playerStats.isHealing){
            UseItem();
        }
        if (Input.GetKeyDown(KeyCode.F) && !playerStats.isHealing){
            UseItem();
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
        if (itemTypeCount < 0){
            return;
        }

        // Debug.Log($"Selected Item: {itemList[selectedItem].itemName}");
        // Update the item icons for all panels based on current selected item
        for (int index = 0; index < itemHolders.transform.childCount; index++){

            var panelIcon = itemHolders.transform.GetChild(index).gameObject.transform.Find("Icon").gameObject;
            panelIcon.GetComponent<Image>().sprite = itemList[SelectedIndex(index - 1)].itemIcon;
        }

        // Update item name and count for selected item
        GameObject.Find("Item Selected Text").GetComponent<TextMeshProUGUI>().text = itemList[selectedItem].itemName;
        GameObject.Find("Item Selected Count").GetComponent<TextMeshProUGUI>().text = itemList[selectedItem].itemCount.ToString();
    }

    void DisplayEmptyHolders()
    {
        // Set everything to empty
        for (int index = 0; index < itemHolders.transform.childCount; index++){

            var panelIcon = itemHolders.transform.GetChild(index).gameObject.transform.Find("Icon").gameObject;
            panelIcon.GetComponent<Image>().sprite = null;
            panelIcon.GetComponent<CanvasRenderer>().SetAlpha(0);
        }

        GameObject.Find("Item Selected Text").GetComponent<TextMeshProUGUI>().text = null;
        GameObject.Find("Item Selected Count").GetComponent<TextMeshProUGUI>().text = null;
    }

    int SelectedIndex(int offset)
    {
        int result = selectedItem + offset;

        // "Wrapping" if overshoot    
        if (result < 0){
            result = itemTypeCount;
        }
        if (result > itemTypeCount){

            result = 0;
        }

        return result;
    }

    void UseItem()
    {
        // If inventory is empty
        if (itemTypeCount < 0){
            return;
        }
         
        playerStats.isHealing = true;
    }

    void ConsumeItem()
    {
        switch(itemList[selectedItem].itemType)
        {
            // Restores health
            case "Heal":

                playerStats.health += itemList[selectedItem].itemPower;
                if (playerStats.health >= playerStats.maxHealth){
                    playerStats.health = playerStats.maxHealth;
                }

                break;

            // Immobolizes monster
            case "Trap":
                GameObject trap = Instantiate(_shockTrapPrefab);
                trap.transform.position = playerStats.transform.position;
                trap.transform.position += new Vector3(0, 1, 0);

                break;

            // Sharpens weapon
            case "Sharpen":
                break;
        }

        // Decrease the item count by one
        itemList[selectedItem].itemCount -= 1;

        // Remove from itemList if item count is 0
        if (itemList[selectedItem].itemCount <= 0){
            itemList.RemoveAt(selectedItem);
            itemTypeCount -= 1;

            if (selectedItem > itemTypeCount){
                selectedItem = 0;
            }
        }

        if (itemTypeCount >= 0){
            UpdateHolder();
        }
        else{
            DisplayEmptyHolders();
        }

    }
}

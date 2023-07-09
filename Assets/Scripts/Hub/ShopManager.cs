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
    [SerializeField] GameObject player;
    PlayerData playerData;
    #endregion


    #region Shop

    [SerializeField] GameObject itemPanel_prefab;
    [SerializeField] GameObject result_prefab;
    [SerializeField] TextMeshProUGUI playerGoldCount;
    [SerializeField] TextMeshProUGUI itemCount;
    [SerializeField] Button buyButton;

    #endregion

    #region Variables
    
    List<Item> itemList;
    public Item itemSelected;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        hubManager = hubManagerObj.GetComponent<HubManager>();
        playerData = player.GetComponent<PlayerData>();
        buyButton.interactable = false;

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
        playerGoldCount.text = playerData.gold.ToString() + " G";
        if (itemSelected != null){
            buyButton.interactable = true;
            itemCount.text = itemSelected.itemCount.ToString();
        }
    }

    public void BuyItem()
    {
        string textDisplay;
        bool purchaseSuccess;

        // Purchase item requirements
        if (playerData.gold >= itemSelected.itemCost &&
            itemSelected.itemCount < itemSelected.maxCount){

            itemSelected.itemCount++;
            playerData.gold -= itemSelected.itemCost;

            textDisplay = "Purchased item!";
            Debug.Log($"Buy: {itemSelected.itemName} , {itemSelected.itemCount}");
            purchaseSuccess = true;
        }
        else{

            purchaseSuccess = false;
            // Fail type
            if (playerData.gold < itemSelected.itemCost){
                textDisplay = "Not enough G";
                // Debug.Log("Not enough G!");
            }
            else {
                textDisplay = "Item has reached max capacity!";
                // Debug.Log("Item has reached max capacity!");
            }
        }

        // If existing result text, destroy it
        if (GameObject.Find("Purchase Result(Clone)") != null){
            Destroy(GameObject.Find("Purchase Result(Clone)"));
        }

        // Display the result output by instantiating the text object
        GameObject newText = Instantiate(result_prefab);
        newText.transform.SetParent(GameObject.Find("Shop Canvas").transform, false);
        if (purchaseSuccess){
            newText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else{
            newText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        newText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = textDisplay;
    }


    public void OnButtonExit()
    {
        if (GameObject.Find("Purchase Result(Clone)") != null){
            Destroy(GameObject.Find("Purchase Result(Clone)"));
        }

        hubManager.isSelectedBuilding = false;
        gameObject.SetActive(false);           
    }
}

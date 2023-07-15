using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgeManager : MonoBehaviour
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
    // [SerializeField] GameObject result_prefab;
    // [SerializeField] TextMeshProUGUI playerGoldCount;
    // [SerializeField] TextMeshProUGUI itemCount;
    [SerializeField] Button buyButton;

    #endregion
    
    #region Category Buttons

    [SerializeField] Button weaponButton;
    [SerializeField] Button armorButton;

    #endregion

    #region Variables
    
    public string tabSelected = "Weapon";
    bool tabChanged = false;
    List<Weapon> weaponList;
    public Weapon weaponSelected;
    List<Armor> armorList;
    public Armor armorSelected;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        hubManager = hubManagerObj.GetComponent<HubManager>();
        playerData = player.GetComponent<PlayerData>();
        buyButton.interactable = false;

        // Get item data which contains scriptable objects
        weaponList = Resources.LoadAll<Weapon>("Weapon").ToList();

        // Remove item from list if itemcount is 0
        foreach (var item in weaponList){

            GameObject panel = Instantiate(itemPanel_prefab);
            panel.transform.SetParent(shopSelectParent.transform, false);

            panel.GetComponent<ForgeItemData>().weaponData = item;
            panel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemIcon;

        }

        // // Get item data which contains scriptable objects
        // armorList = Resources.LoadAll<Armor>("Armor").ToList();

        // // Remove item from list if itemcount is 0
        // foreach (var item in armorList){

        //     GameObject panel = Instantiate(itemPanel_prefab);
        //     panel.transform.SetParent(shopSelectParent.transform, false);

        //     panel.GetComponent<ForgeItemData>().armorData = item;
        //     panel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemIcon;

        // }

        // selectedItem = 0;
        // itemTypeCount = itemList.Count() - 1;

        // // Set initial display for the item holders
        // UpdateHolder();
        
        // Weapon tab is selected by default
        weaponButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Item Selector(Clone)") != null){
            buyButton.interactable = true;
        }
        else{
            buyButton.interactable = false;
        }

        if (tabChanged){
            UpdateDisplay();
        }
    }

    #region Tab Selection

    public void WeaponTab()
    {        
        armorButton.interactable = true;
        weaponButton.interactable = false;
        tabChanged = true;
        tabSelected = "Weapon";
    }

    public void ArmorTab()
    {
        weaponButton.interactable = true;
        armorButton.interactable = false;
        tabChanged = true;
        tabSelected = "Armor";
    }

    #endregion

    private void UpdateDisplay()
    {
        foreach (Transform child in shopSelectParent.transform)
        {
            Destroy(child.gameObject);
        }

        switch(tabSelected)
        {
            case "Weapon":

                // Remove item from list if itemcount is 0
                foreach (var item in weaponList)
                {

                    GameObject panel = Instantiate(itemPanel_prefab);
                    panel.transform.SetParent(shopSelectParent.transform, false);

                    panel.GetComponent<ForgeItemData>().weaponData = item;
                    panel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemIcon;

                }

                break;
            case "Armor":

                // Get item data which contains scriptable objects
                armorList = Resources.LoadAll<Armor>("Armor").ToList();

                // Remove item from list if itemcount is 0
                foreach (var item in armorList)
                {

                    GameObject panel = Instantiate(itemPanel_prefab);
                    panel.transform.SetParent(shopSelectParent.transform, false);

                    panel.GetComponent<ForgeItemData>().armorData = item;
                    panel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemIcon;

                }


                break;
        }

        // Resets cursor information on previous tab
        GameObject shopItemSelector = GameObject.Find("Item Selector(Clone)");
        Destroy(shopItemSelector);
        tabChanged = false;
    }

    public void BuyItem()
    {
        // string textDisplay;
        // bool purchaseSuccess;

        // Purchase item requirements
        // if (playerData.gold >= itemSelected.itemCost &&
        //     itemSelected.itemCount < itemSelected.maxCount){

        //     itemSelected.itemCount++;
        //     playerData.gold -= itemSelected.itemCost;

        //     textDisplay = "Purchased item!";
        //     Debug.Log($"Buy: {itemSelected.itemName} , {itemSelected.itemCount}");
        //     purchaseSuccess = true;
        // }
        // else{

        //     purchaseSuccess = false;
        //     // Fail type
        //     if (playerData.gold < itemSelected.itemCost){
        //         textDisplay = "Not enough G";
        //         // Debug.Log("Not enough G!");
        //     }
        //     else {
        //         textDisplay = "Item has reached max capacity!";
        //         // Debug.Log("Item has reached max capacity!");
        //     }
        // }

        // If existing result text, destroy it
        // if (GameObject.Find("Purchase Result(Clone)") != null){
        //     Destroy(GameObject.Find("Purchase Result(Clone)"));
        // }

        // Display the result output by instantiating the text object
        // GameObject newText = Instantiate(result_prefab);
        // newText.transform.SetParent(GameObject.Find("Shop Canvas").transform, false);
        // if (purchaseSuccess){
        //     newText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;
        // }
        // else{
        //     newText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
        // }
        // newText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = textDisplay;
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

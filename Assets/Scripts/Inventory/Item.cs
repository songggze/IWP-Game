using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item")]
public class Item : ScriptableObject 
{
    public int itemId;
    public string itemName;
    public int itemCount;
    public int maxCount;

    public Sprite itemIcon;

    // Item stats
    public string itemType;
    public int itemPower;

    // Constructor
    //Item(int _id, string _name, int _count){
        //itemId = _id;
        //itemName = _name;
        //itemCount = _count;
    //}
}

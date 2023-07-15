using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inventory System/Weapon")]
public class Weapon : ScriptableObject
{
    public int itemId;
    public string itemName;
    public string itemDescription;
    public bool isOwned = false;

    public Sprite itemIcon;

    // Weapon stats
    public int power = 0;
    
    // Materials required to craft
    public List<CraftingMaterial> craftingMaterials;
}

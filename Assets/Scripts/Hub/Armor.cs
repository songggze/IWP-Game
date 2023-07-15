using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inventory System/Armor")]
public class Armor : ScriptableObject
{
    public int itemId;
    public string itemName;
    public string itemDescription;
    public bool isOwned = false;

    public Sprite itemIcon;

    // Weapon stats
    public int power = 0;

    // TODO:
    // Add special effects???
    // public string effectType;
    // then switch statemenmt somewhere....
    
    // Materials required to craft
    public List<CraftingMaterial> craftingMaterials;
}

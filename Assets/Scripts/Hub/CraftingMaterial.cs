using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Crafting Material")]
public class CraftingMaterial : ScriptableObject
{

    public int itemId;
    public string itemName;
    public int itemCount;
    public int maxCount;
}

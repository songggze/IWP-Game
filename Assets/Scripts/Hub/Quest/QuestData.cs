using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Quest Data")]
public class QuestData : ScriptableObject 
{
    public int itemId;
    public string itemName;
    public string itemDescription;

    public bool questUnlocked = false;

    public Sprite itemIcon;
}

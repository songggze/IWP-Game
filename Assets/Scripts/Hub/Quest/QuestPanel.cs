using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] GameObject questManagerObj;
    [SerializeField] GameObject questManagerSelector;
    QuestManager questManager;
    public QuestData itemData;

    #region Quest Right Panel

    [SerializeField] GameObject _itemTitle;
    [SerializeField] GameObject _itemDescription;
    #endregion

    void Start()
    {
        if (!itemData.questUnlocked){
            Debug.Log("disable");
            gameObject.SetActive(false);
        }

        questManagerObj = GameObject.Find("Quest Canvas");
        questManager = questManagerObj.GetComponent<QuestManager>();
    }

    public void OnButtonClick()
    {
        _itemTitle.GetComponent<TextMeshProUGUI>().text = itemData.itemName;
        _itemDescription.GetComponent<TextMeshProUGUI>().text = itemData.itemDescription;
        questManager.questSelected = true;
    }
}

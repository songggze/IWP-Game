using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{

    #region References
    [SerializeField] GameObject hubManagerObj;
    HubManager hubManager;

    [SerializeField] GameObject shopSelectParent;
    [SerializeField] GameObject player;
    PlayerData playerData;
    #endregion


    #region Quest

    [SerializeField] Button startButton;

    #endregion

    #region Variables
    
    public bool questSelected = false;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        hubManager = hubManagerObj.GetComponent<HubManager>();
        playerData = player.GetComponent<PlayerData>();
        startButton.interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (questSelected){
            startButton.interactable = true;
        }
    }


    public void OnButtonExit()
    {
        hubManager.isSelectedBuilding = false;
        gameObject.SetActive(false);           
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HubManager : MonoBehaviour
{
    Ray ray;
    public bool isSelectedBuilding = false;
    private bool updateDisplay = false;

    // Canvases
    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject forgeCanvas;

    [SerializeField] Button _quitButton;
    [SerializeField] TextMeshProUGUI _labelText;

    // Display quest clear status
    [SerializeField] TextMeshProUGUI _clearTime;

    // Start is called before the first frame update
    void Start()
    {
        // Display quest clear status, if clear show fastest time record
        if (PlayerPrefs.HasKey("GroundedMonster Time")){
            float tempTime = PlayerPrefs.GetFloat("GroundedMonster Time");
            float tempMinute = Mathf.Floor(tempTime / 60);
            float tempSecond = tempTime % 60;
            if (tempSecond < 10){
                _clearTime.text = $"Clear time   {tempMinute} : 0{tempSecond}";
            }
            else {
                _clearTime.text = $"Clear time   {tempMinute} : {tempSecond}";
            }
        }
        else{
            _clearTime.text = "NOT CLEARED!";
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isSelectedBuilding){

            // Check Selected
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Label text display when mouse over clickable building
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                MeshFilter meshFilter = clickedObject.GetComponent<MeshFilter>();

                if (meshFilter != null)
                {
                    HighlightBuilding(clickedObject.transform.root.name);
                }
            }

            // Click Building
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    MeshFilter meshFilter = clickedObject.GetComponent<MeshFilter>();

                    if (meshFilter != null)
                    {
                        _labelText.text = "";
                        SelectBuilding(clickedObject.transform.root.name);
                    }
                }
            }

        }


        if (!isSelectedBuilding && !updateDisplay){

            // Display quest clear status, if clear show fastest time record
            if (PlayerPrefs.HasKey("GroundedMonster Time"))
            {
                float tempTime = PlayerPrefs.GetFloat("GroundedMonster Time");
                float tempMinute = Mathf.Floor(tempTime / 60);
                float tempSecond = tempTime % 60;
                if (tempSecond < 10)
                {
                    _clearTime.text = $"Clear time   {tempMinute} : 0{tempSecond}";
                }
                else
                {
                    _clearTime.text = $"Clear time   {tempMinute} : {tempSecond}";
                }
            }
            else
            {
                _clearTime.text = "";
            }

            updateDisplay = true;
        }
    }
    void HighlightBuilding(string objectName)
    {
        switch(objectName)
        {
            case "Quests":
            case "Shop":
            case "Forge":

                _labelText.text = objectName;
                break;

            default:
                _labelText.text = "";
                break;
        }
    }

    void SelectBuilding(string objectName)
    {
        switch(objectName)
        {
            case "Quests":
                SceneManager.LoadScene("Forest");
                break;
            case "Shop":
                isSelectedBuilding = true;
                shopCanvas.SetActive(true);
                break;
            case "Forge":
                isSelectedBuilding = true;
                forgeCanvas.SetActive(true);
                break;
        }

        _clearTime.text = "";
        updateDisplay = false;
    }

    public void OnButtonQuit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

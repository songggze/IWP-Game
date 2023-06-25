using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Check Selected
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))        {
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                MeshFilter meshFilter = clickedObject.GetComponent<MeshFilter>();

                if (meshFilter != null)
                {
                    Debug.Log("Clicked Mesh: " + clickedObject.transform.root.name);
                    SelectBuilding(clickedObject.transform.root.name);
                }
            }
        }
    }

    void SelectBuilding(string objectName)
    {
        switch(objectName)
        {
            case "Quest Bulletin":
                SceneManager.LoadScene("Forest");
                break;
        }
    }
}

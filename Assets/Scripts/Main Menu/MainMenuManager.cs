using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void StartButton()
    {
        SceneManager.LoadScene("Hub");
    }

    public void OptionButton()
    {
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}

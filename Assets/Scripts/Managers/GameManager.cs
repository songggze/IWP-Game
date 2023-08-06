using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Stats
    [SerializeField] GameObject player, monster;
    private PlayerStats playerStats;
    private GroundedMonster monsterStats;

    // Canvas
    [SerializeField] GameObject winCanvas, loseCanvas;
    [SerializeField] GameObject timerText;

    // Flags
    bool winGame;
    bool endGame;

    // Quest Timer
    float counterTimer;
    public float timeSecond, timeMinute;
    float transitionTimer;

    // Start is called before the first frame update
    void Start()
    {
        
        playerStats = player.GetComponent<PlayerStats>();
        monsterStats = monster.GetComponent<GroundedMonster>();

        winGame = false;
        endGame = false;
         
        counterTimer = 0;
        timeSecond = 0;
        timeMinute = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.visible){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        // Game ending criteria
        if (!endGame){

            if (playerStats.health <= 0){

                winGame = false;
                endGame = true;
                transitionTimer = 3;
                loseCanvas.SetActive(true);
            }
            if (monsterStats.health <= 0){

                winGame = true;
                endGame = true;
                transitionTimer = 3;
                winCanvas.SetActive(true);

                // Saving time,
                // will only update when key is null or set new record time
                if (!PlayerPrefs.HasKey("GroundedMonster Time") || 
                    PlayerPrefs.GetFloat("GroundedMonster Time", 9999) >= (timeMinute * 60) + timeSecond){

                    PlayerPrefs.SetFloat("GroundedMonster Time", (timeMinute * 60) + timeSecond);
                    Debug.Log("Quest time saved");
                }
                else{
                    Debug.Log("Didn't save :(");
                }
            }

        }

        // Quest timer tracking
        if (!endGame){

            counterTimer -= Time.deltaTime;
            if (counterTimer <= 0)
            {
                counterTimer += 1;
                timeSecond++;
                if (timeSecond == 60)
                {
                    timeMinute++;
                    timeSecond = 0;
                }
            }
        }

        // A delay timer before transitioning to win/lose screen
        if (endGame && transitionTimer > 0){
            transitionTimer -= Time.deltaTime;
        }

        // Display end result
        if (endGame && transitionTimer <= 0){

            if (winGame){

                winCanvas.GetComponent<CanvasGroup>().alpha += 0.5f * Time.deltaTime;

                // Display time taken to clear quest
                if (timeSecond < 10){
                    timerText.GetComponent<TextMeshProUGUI>().text = $"Clear time   {timeMinute} : 0{timeSecond}";
                }
                else {
                    timerText.GetComponent<TextMeshProUGUI>().text = $"Clear time   {timeMinute} : {timeSecond}";
                }

            }
            else{

                loseCanvas.GetComponent<CanvasGroup>().alpha += 1 * Time.deltaTime;

            }

            // Restart game
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Hub");
            }
        }
    }

}

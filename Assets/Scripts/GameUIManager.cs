using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float startTime;
    [SerializeField] private GameObject pausePanel;

    private float currentTime;

    // public Text blueTeamName;
    // public Text redTeamName;
    // public Text score;

    // public GameObject scoreManager;
    // public GameObject namesManager;

    private void Start()
    {
        currentTime = startTime;

        // scoreManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void FixedUpdate()
    {
        currentTime -= Time.fixedDeltaTime;
        if (currentTime < 0f)
        {
            currentTime = 0f;
            pausePanel.SetActive(true);
            gameObject.GetComponent<VideoRecorder>.StopCapture();
            Time.timeScale = 0f;
        }

        int minutes = Mathf.FloorToInt(currentTime / 60f);

        int seconds = Mathf.FloorToInt(currentTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = timeString;

        // score.text = scoreManager.GetComponent<GoalSystem>().RedScore.ToString() + ":" + scoreManager.GetComponent<GoalSystem>().BlueScore.ToString();


        // if (PlayerPrefs.HasKey("BlueName"))
        // {
        //     blueTeamName.text = PlayerPrefs.GetString("BlueName");
        // }
        // if (PlayerPrefs.HasKey("RedName"))
        // {
        //     redTeamName.text = PlayerPrefs.GetString("RedName");
        // }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text errorTextObject;
    
    [SerializeField] private float startTime;
    
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject tc;
    [SerializeField] private GameObject vd;

    private float currentTime;

    public bool canDecrease = false;

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
        if (canDecrease)
        {
            currentTime -= Time.fixedDeltaTime;
            if (currentTime < 0f)
            {
                currentTime = 0f;
                GameOver();
                // gameObject.GetComponent<VideoRecorder>.StopCapture();
            }

            int minutes = Mathf.FloorToInt(currentTime / 60f);

            int seconds = Mathf.FloorToInt(currentTime % 60f);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timeString;
        }

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
    public void GameOver(string errorText = "")
    {
        tc = GameObject.Find("Tournament");
        tc.GetComponent<TournamentController>().onGameOver();
        pausePanel.SetActive(true);
        errorTextObject.text = errorText;
        StartCoroutine(DoStopGame());
    }
    IEnumerator DoStopGame()
    {
        yield return new WaitForSeconds(1f);
        vd.GetComponent<VideoRecorder>().StopVideoCapture();
        Time.timeScale = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject startServerButton;
    
    [SerializeField] private Image leftPlayerReady, leftPlayerNotReady;
    [SerializeField] private Image rightPlayerReady, rightPlayerNotReady;
    
    [SerializeField] private TMP_Text leftPlayerNick, leftPlayerPort;
    [SerializeField] private TMP_Text rightPlayerNick, rightPlayerPort;

    [SerializeField] private GameObject tc;

    private void Start()
    {
        // PauseGame();
        if (tc.GetComponent<TournamentController>().isServerSide())
        {
            startServerButton.SetActive(false);
            StartGame();
            // Начать запись видео
        }
        leftPlayerNick.text = tc.GetComponent<TournamentController>().getPlayerName(0);
        rightPlayerNick.text = tc.GetComponent<TournamentController>().getPlayerName(1);

        // leftPlayerPort.text = tc.GetComponent<TournamentController>().getPlayerPort(0).ToString();
        // rightPlayerPort.text = tc.GetComponent<TournamentController>().getPlayerPort(1).ToString();
    }

    private void FixedUpdate()
    {
        if (tc.GetComponent<TournamentController>().isPlayerReady(0))
        {
            leftPlayerNotReady.gameObject.SetActive(false);
        }
        if (tc.GetComponent<TournamentController>().isPlayerReady(1))
        {
            rightPlayerNotReady.gameObject.SetActive(false);
        }
    }
    public void StartGame()
    {
        if (tc.GetComponent<TournamentController>().isPlayerReady(0) && tc.GetComponent<TournamentController>().isPlayerReady(1))
        {
            ResumeGame();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        startMenuPanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] bluePlayers;
    [SerializeField] private GameObject[] redPlayers;
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject startServerButton;
    
    [SerializeField] private Image leftPlayerReady, leftPlayerNotReady;
    [SerializeField] private Image rightPlayerReady, rightPlayerNotReady;
    
    [SerializeField] private TMP_Text leftPlayerNick, leftPlayerPort;
    [SerializeField] private TMP_Text rightPlayerNick, rightPlayerPort;
    
    [SerializeField] private GameObject tc;
    [SerializeField] private GameObject vd;

    private bool isPortSetted = false;

    private void Start()
    {
        bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
        redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
        PauseGame();
        Debug.Log("tc" + tc.GetComponent<TournamentController>().isServerSide());
        StartCoroutine(DoCheckIsServerSide());

        
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
        if (isPortSetted)
        {
            return;
        }
        else
        {
            try
            {
                leftPlayerNick.text = tc.GetComponent<TournamentController>().getPlayerName(0);
                rightPlayerNick.text = tc.GetComponent<TournamentController>().getPlayerName(1);
                leftPlayerPort.text = tc.GetComponent<TournamentController>().getPlayerPort(0).ToString();
                rightPlayerPort.text = tc.GetComponent<TournamentController>().getPlayerPort(1).ToString();
                isPortSetted = true;
            }
            catch
            {
                return;
            }
        }
    }
    private IEnumerator DoCheckIsServerSide()
    {
        yield return new WaitForSeconds(0.2f);
        if (tc.GetComponent<TournamentController>().isServerSide())
        {
            startServerButton.SetActive(false);
            StartGame();
            // Начать запись видео
        }
        else
        {
            startServerButton.SetActive(true);
        }
    }
    public void StartGame()
    {
        if (tc.GetComponent<TournamentController>().isPlayerReady(0) && tc.GetComponent<TournamentController>().isPlayerReady(1))
        {
            ResumeGame();
            vd.GetComponent<VideoRecorder>().StartRecording();
        }
    }
    public void PauseGame()
    {
        startMenuPanel.SetActive(true);
        foreach(GameObject bluePlayer in bluePlayers)
        {
            bluePlayer.GetComponent<PlayerMovement>().canMove = false;
        }
        foreach(GameObject redPlayer in redPlayers)
        {
            redPlayer.GetComponent<PlayerMovement>().canMove = false;
        }
        gameObject.GetComponent<GameUIManager>().canDecrease = false;
    }
    public void ResumeGame()
    {
        foreach(GameObject bluePlayer in bluePlayers)
        {
            bluePlayer.GetComponent<PlayerMovement>().canMove = true;
        }
        foreach(GameObject redPlayer in redPlayers)
        {
            redPlayer.GetComponent<PlayerMovement>().canMove = true;
        }
        gameObject.GetComponent<GameUIManager>().canDecrease = true;
        startMenuPanel.SetActive(false);
    }
}

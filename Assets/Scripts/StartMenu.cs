using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startServerButton;
    
    [SerializeField] private Image leftPlayerReady, leftPlayerNotReady;
    [SerializeField] private Image rightPlayerReady, rightPlayerNotReady;
    
    [SerializeField] private TMP_Text leftPlayerNick, leftPlayerPort;
    [SerializeField] private TMP_Text rightPlayerNick, rightPlayerPort;

    [SerializeField] private GameObject tc;

    private void Start()
    {
        if (tc.GetComponent<TournamentController>().isServerSide())
        {
            startServerButton.SetActive(false);
        }
        
        // MenuPlayer leftPlayer = MenuPlayer();
        // MenuPlayer rightPlayer = MenuPlayer();
    }

    private void Update()
    {
          
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    public class MenuPlayer
    {
        public Image readyImage, notReadyImage;
        public TMP_Text nickname, port;
    }
}

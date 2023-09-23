using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDownScore : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("BlueTrigger"))
        {
            if(other.CompareTag("RedPlayer") && other.GetComponent<CatchBall>().isCatched)
            {
                Debug.Log("+5 красным");
                gameManager.GetComponent<GoalSystem>().RedScore += 5;
                gameManager.GetComponent<GoalSystem>().UpdateUI();
                gameManager.GetComponent<TeamSpawner>().RespawnTeams();
            }
        }
        else if(gameObject.CompareTag("RedTrigger"))
        {
            if(other.CompareTag("BluePlayer") && other.GetComponent<CatchBall>().isCatched)
            {
                Debug.Log("+5 синим");
                gameManager.GetComponent<GoalSystem>().BlueScore += 5;
                gameManager.GetComponent<GoalSystem>().UpdateUI();
                gameManager.GetComponent<TeamSpawner>().RespawnTeams();
            }
        }
    }
}

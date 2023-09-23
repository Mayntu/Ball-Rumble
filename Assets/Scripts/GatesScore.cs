using UnityEngine;

public class GatesScore : MonoBehaviour
{
	[SerializeField] private GameObject gameManager;

	private void Start()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("BlueGatesTrigger"))
		{
			gameManager.GetComponent<GoalSystem>().RedScore += 2;
			gameManager.GetComponent<GoalSystem>().UpdateUI();
			gameManager.GetComponent<TeamSpawner>().RespawnTeams();
		}
		else if(other.CompareTag("RedGatesTrigger"))
		{
			gameManager.GetComponent<GoalSystem>().BlueScore += 2;
			gameManager.GetComponent<GoalSystem>().UpdateUI();
			gameManager.GetComponent<TeamSpawner>().RespawnTeams();
		}
	}
}

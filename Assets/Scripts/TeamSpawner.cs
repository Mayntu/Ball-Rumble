using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSpawner : MonoBehaviour
{
    [SerializeField] private GameObject redPlayerPrefab, bluePlayerPrefab, lightBluePlayerPrefab, yellowPlayerPrefab, 
        blackPlayerPrefab, whitePlayerPrefab;
        
    private GameObject[] redTeam = null;
    private GameObject[] blueTeam = null;

    [SerializeField] private Transform[] redTeamSpawnPoints;
    [SerializeField] private Transform[] blueTeamSpawnPoints;

    private void Awake()
    {
        redTeam = new GameObject[] { redPlayerPrefab, lightBluePlayerPrefab, blackPlayerPrefab };
        blueTeam = new GameObject[] { bluePlayerPrefab, yellowPlayerPrefab, whitePlayerPrefab };
    }
    private void Start()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                SpawnRedPlayers();
                break;
            case 1:
                SpawnLightBluePlayers();
                break;
            case 2:
                SpawnBlackPlayers();
                break;
            default:
                break;
        }

        switch (Random.Range(0, 3))
        {
            case 0:
                SpawnBluePlayers();
                break;
            case 1:
                SpawnYellowPlayers();
                break;
            case 2:
                SpawnWhitePlayers();
                break;
            default:
                break;
        }
        
    }

    private void SpawnRedPlayers() { SpawnRedTeam(redPlayerPrefab); }

    private void SpawnBluePlayers() { SpawnBlueTeam(bluePlayerPrefab); }

    private void SpawnLightBluePlayers() { SpawnRedTeam(lightBluePlayerPrefab); }

    private void SpawnYellowPlayers() { SpawnBlueTeam(yellowPlayerPrefab); }

    private void SpawnBlackPlayers() { SpawnRedTeam(blackPlayerPrefab); }

    private void SpawnWhitePlayers() { SpawnBlueTeam(whitePlayerPrefab); }
    
    private void SpawnRedTeam(GameObject playerPrefab)
    {
        foreach(Transform spawnPoint in redTeamSpawnPoints)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
    private void SpawnBlueTeam(GameObject playerPrefab)
    {
        foreach(Transform spawnPoint in blueTeamSpawnPoints)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
    private void SpawnTeam(string teamName, GameObject playerPrefab)
    {
        // switch(teamName)
        // {
        //     case "red":
        //         ...
        //         break;
        //     case "blue":
        //         ...
        //         break;
        //     default:
        //         Debug.Log("Неправильное название команды!")
        // }
        // (teamName == "red") ? {} : {}
    }
}

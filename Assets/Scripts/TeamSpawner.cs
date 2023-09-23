using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SerializableAttribute = System.SerializableAttribute;
using System.IO;

public class TeamSpawner : MonoBehaviour
{
    [SerializeField] private GameObject uniquePlayerPrefab;

    [SerializeField] private Transform[] redTeamSpawnPoints;
    [SerializeField] private Transform[] blueTeamSpawnPoints;
    	
    [SerializeField] private Transform ballSpawnPoint;

    // [SerializeField] private Image[] redTeamPanels;
    // [SerializeField] private Image[] blueTeamPanels;

    [SerializeField] List<TeamNamePanel> redTeamPanels = new List<TeamNamePanel>();
    [SerializeField] List<TeamNamePanel> blueTeamPanels = new List<TeamNamePanel>();

    [SerializeField] private GameObject redTrigger;
    [SerializeField] private GameObject blueTrigger;

    private GameObject[] redTeam;
    private GameObject[] blueTeam;

    private GameObject ball;

    private List<int> nums = new List<int> {1, 2, 3, 4, 5, 6, 7, 8};

    private FileLogger logger;
    
    private string infoToLog = "";

    private void Start()
    {
        logger = GetComponent<FileLogger>();
        
        // от 0 до 8
        // в зависимости от цифры будет выбераться принадлежность к команде и её цвет
        // менять тэг, менять флажок цвета, менять сторону
        // создать массив по типу nums[0, 1, 2, 3, 4, 5, 6, 7, 8]
        // когда выпадает цифра удалять её из массива, чтобы генерировать другую комманду


        Debug.Log(nums.Count);

        int randomInt = nums[Random.Range(0, nums.Count)];
        
        Debug.Log(randomInt);
        
        if(randomInt == 1)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 2)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 3)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 4)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 5)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 6)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 7)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 8)
        {
            SpawnRedTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }

        
        Debug.Log(nums.Count);

        randomInt = nums[Random.Range(0, nums.Count)];
                    
        Debug.Log(randomInt);
        
        if(randomInt == 1)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 2)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 3)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 4)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 5)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 6)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 7)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
        else if(randomInt == 8)
        {
            SpawnBlueTeam(uniquePlayerPrefab, randomInt);
            nums.RemoveAt(nums.IndexOf(randomInt));
        }
    }

    private void SpawnRedTeam(GameObject playerPrefab, int randomInt)
    {
        infoToLog += "\n\t";
        
        infoToLog += "Red Team Information:\n\t\t";
        
        playerPrefab.tag = "RedPlayer";

        infoToLog += "Tag: " + playerPrefab.tag + " | ";

        playerPrefab.GetComponentInChildren<PlayerType>().ResetColor();
        if(randomInt == 1) { playerPrefab.GetComponentInChildren<PlayerType>().isRed = true; SetImageEnabled(redTeamPanels, "Red", true); }
        else if(randomInt == 2) { playerPrefab.GetComponentInChildren<PlayerType>().isBlue = true; SetImageEnabled(redTeamPanels, "Blue", true); }
        else if(randomInt == 3) { playerPrefab.GetComponentInChildren<PlayerType>().isLightBlue = true; SetImageEnabled(redTeamPanels, "LightBlue", true); }
        else if(randomInt == 4) { playerPrefab.GetComponentInChildren<PlayerType>().isYellow = true; SetImageEnabled(redTeamPanels, "Yellow", true);}
        else if(randomInt == 5) { playerPrefab.GetComponentInChildren<PlayerType>().isPurple = true; SetImageEnabled(redTeamPanels, "Purple", true); }
        else if(randomInt == 6) { playerPrefab.GetComponentInChildren<PlayerType>().isBlack = true; SetImageEnabled(redTeamPanels, "Black", true); }
        else if(randomInt == 7) { playerPrefab.GetComponentInChildren<PlayerType>().isWhite = true; SetImageEnabled(redTeamPanels, "White", true); }
        else if(randomInt == 8) { playerPrefab.GetComponentInChildren<PlayerType>().isPink = true; SetImageEnabled(redTeamPanels, "Pink", true); }

        foreach(Transform spawnPoint in redTeamSpawnPoints)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }

        infoToLog += "Team number: " + randomInt.ToString();
    }
    private void SpawnBlueTeam(GameObject playerPrefab, int randomInt)
    {
        infoToLog += "\n\t";

        infoToLog += "Blue Team Information:\n\t\t";
        
        playerPrefab.tag = "BluePlayer";

        infoToLog += "Tag: " + playerPrefab.tag + " | ";

        playerPrefab.GetComponentInChildren<PlayerType>().ResetColor();
        if(randomInt == 1) { playerPrefab.GetComponentInChildren<PlayerType>().isRed = true; SetImageEnabled(blueTeamPanels, "Red", true); }
        else if(randomInt == 2) { playerPrefab.GetComponentInChildren<PlayerType>().isBlue = true; SetImageEnabled(blueTeamPanels, "Blue", true); }
        else if(randomInt == 3) { playerPrefab.GetComponentInChildren<PlayerType>().isLightBlue = true; SetImageEnabled(blueTeamPanels, "LightBlue", true); }
        else if(randomInt == 4) { playerPrefab.GetComponentInChildren<PlayerType>().isYellow = true; SetImageEnabled(blueTeamPanels, "Yellow", true); }
        else if(randomInt == 5) { playerPrefab.GetComponentInChildren<PlayerType>().isPurple = true; SetImageEnabled(blueTeamPanels, "Purple", true); }
        else if(randomInt == 6) { playerPrefab.GetComponentInChildren<PlayerType>().isBlack = true; SetImageEnabled(blueTeamPanels, "Black", true); }
        else if(randomInt == 7) { playerPrefab.GetComponentInChildren<PlayerType>().isWhite = true; SetImageEnabled(blueTeamPanels, "White", true); }
        else if(randomInt == 8) { playerPrefab.GetComponentInChildren<PlayerType>().isPink = true; SetImageEnabled(blueTeamPanels, "Pink", true); }

        foreach(Transform spawnPoint in blueTeamSpawnPoints)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }

        infoToLog += "Team number: " + randomInt.ToString() + "\n";
        

        logger.Log(infoToLog);
    }
    public void RespawnTeams()
    {
        StartCoroutine(DoGoal());
    }
    IEnumerator DoGoal()
    {
        redTrigger.GetComponent<TouchDownScore>().canAdd = false;
        blueTrigger.GetComponent<TouchDownScore>().canAdd = false;
        ball = GameObject.FindGameObjectWithTag("Ball");
        redTeam = GameObject.FindGameObjectsWithTag("RedPlayer");
        blueTeam = GameObject.FindGameObjectsWithTag("BluePlayer");
        yield return new WaitForSeconds(0.2f);
        ball.GetComponent<Rigidbody>().isKinematic = true;
        for(int i = 0; i < redTeam.Length; i++)
        {
            redTeam[i].GetComponent<PlayerMovement>().canMove = false;
            blueTeam[i].GetComponent<PlayerMovement>().canMove = false;
        }
        yield return new WaitForSeconds(1.5f);
        for(int i = 0; i < redTeamSpawnPoints.Length; i++)
        {
            redTeam[i].GetComponent<CatchBall>().DropBall();
            redTeam[i].transform.position = redTeamSpawnPoints[i].position;
            redTeam[i].GetComponent<PlayerMovement>().canMove = true;
        }
        for(int i = 0; i < blueTeamSpawnPoints.Length; i++)
        {
            blueTeam[i].GetComponent<CatchBall>().DropBall();
            blueTeam[i].transform.position = blueTeamSpawnPoints[i].position;
            blueTeam[i].GetComponent<PlayerMovement>().canMove = true;
        }
        ball.transform.position = ballSpawnPoint.position;
        redTrigger.GetComponent<TouchDownScore>().canAdd = true;
        blueTrigger.GetComponent<TouchDownScore>().canAdd = true;
    }
    private void SetImageEnabled(List<TeamNamePanel> list, string teamName, bool value)
    {
        var teamNamePanel = list.Find(kv => kv.teamName == teamName);
        
        if (teamNamePanel != null)
        {
            teamNamePanel.panel.GetComponent<Image>().enabled = value;
        }
    }
    [System.Serializable]
    public class TeamNamePanel
    {
        public string teamName;
        public Image panel;
    }
}

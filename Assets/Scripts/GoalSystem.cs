using UnityEngine;
using TMPro;

public class GoalSystem : MonoBehaviour
{
    public int RedScore
    {
        get { return redScore; }
        set { redScore = value; }
    }
    public int BlueScore
    {
        get { return blueScore; }
        set { blueScore = value; }
    }
    
    [SerializeField] private int blueScore;
    [SerializeField] private int redScore;

    [SerializeField] private TMP_Text blueTeamScore;
    [SerializeField] private TMP_Text redTeamScore;

    private void Start()
    {
        UpdateUI();
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        // UpdateUI();
    }
    public void UpdateUI()
    {
        blueTeamScore.SetText(blueScore.ToString());
        redTeamScore.SetText(redScore.ToString());
    }
    public int GetScore(string team)
    {
        if (team == "blue")
        {
            return blueScore;
        }
        else if (team == "red")
        {
            return redScore;
        }
        else
        {
            return -1;
        }
    }
}

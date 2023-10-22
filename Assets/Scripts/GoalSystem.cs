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
    public int GetScore(int teamId)
    {
        if (teamId == 0)
        {
            return blueScore;
        }
        else if (teamId == 1)
        {
            return redScore;
        }
        else
        {
            return -1;
        }
    }
}

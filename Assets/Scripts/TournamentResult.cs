using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[Serializable]
public class TournamentResult 
{
    public TeamResult[] teams;
    public int winnerId = -1;

    public TournamentResult(TournamentPlayer[] players) {
        teams = new TeamResult[players.Length];
        for(int i = 0; i < teams.Length; i++) {
            teams[i] = new TeamResult(players[i]);
        }

    }

    public void setScore(int id,  int score) {
        teams[id].score = score;
        winnerId = getWinnerId();
    }

    public void setError(int id, string description) {
        teams[id].error = description;
        if (description != "") winnerId = getWinnerId();
    }

    public string toJson() {
        return JsonUtility.ToJson(this);
    }

    private int getWinnerId() {
        TeamResult[] goodTeams = teams.Where(team => team.error == "").ToArray();
        int maxScore = goodTeams.Max(team => team.score);
        TeamResult[] topTeams = teams.Where(team => team.score == maxScore).ToArray();
        if (topTeams.Length == 1) return topTeams[0].id;
        return -1;
    }
}

[Serializable]
public class TeamResult
{
    public int id;
    public string name;
    public int score;
    public string error;

    public TeamResult(TournamentPlayer player) {
        id = player.playerId();
        name = player.playerName();
        score = 0;
        error = "";
    }
}

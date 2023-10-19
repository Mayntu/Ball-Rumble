using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CliManager {
    private List<string> teams = new List<string>();
    public CliManager() { 
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length-1; i++) {
            if (args[i] == "--player") {
                teams.Add(args[++i]);
                Debug.Log($"GameArguments:: got team cfg: {args[i]}");
            }
        }
    }


    public int teamsConfigured() { return teams.Count; }


    public string getTeamConfigById(int id) {
        if (id < teams.Count) {
            string pattern = @"""id""\s:\s\""" + $"{id}" + @"""";
            Regex re = new Regex(pattern);
            foreach (string cgf in teams) {
                if (re.IsMatch(cgf)) return teams[id];
            }
        } 
        return "";
    }
}

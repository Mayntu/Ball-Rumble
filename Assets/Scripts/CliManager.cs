using System.Collections.Generic;
using UnityEngine;

public class CliManager {
    private List<string> teams = new List<string>();
    public CliManager() { 
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length-1; i++) {
            if (args[i].ToLower() == "--player") {
                teams.Add(args[++i]);
                Debug.Log($"GameArguments:: got team cfg: {args[i]}");
            }
        }
    }

    public int teamsConfigured() { return teams.Count; }
}

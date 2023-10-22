using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

public class CliManager {
    private List<string> teams = new List<string>();
    public CliManager() {
        //test();
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
            string pattern = @"""id""\s?:\s?" + $"{id}";
            Regex re = new Regex(pattern);
            foreach (string cgf in teams) {
                if (re.IsMatch(cgf)) return teams[id];
            }
        } 
        return "";
    }

    public void echo(string text) {
        Debug.Log("cout << " + text);
        Console.WriteLine(text);
    }

    private void test() {
        teams.Add("{\"id\": 0, \"name\": \"Foo\"}");
        teams.Add("{\"id\": 1, \"name\": \"Bar\"}");

        string t0 = getTeamConfigById(0);
        string t1 = getTeamConfigById(1);
        Debug.Log($"Gonfigured team0: {t0}");
        Debug.Log($"Gonfigured team1: {t1}");
        teams.Clear();
    }

}

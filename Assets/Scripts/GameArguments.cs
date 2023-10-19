using UnityEngine;

public class GameArguments {
    public string[] args;
    public GameArguments() { 
        args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++) {
            Debug.Log($"GameArguments[{i}] = {args[i]}");
        }
    }

}

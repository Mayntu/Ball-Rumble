
using System;
using UnityEngine;

[Serializable]
public class TeamInfo {
    public int id;
    public string tag;
    public string name;
    public string host;
    public int port;

    public TeamInfo(int id, string tag, string name, string host, int port) {
        this.id = id;
        this.tag = tag;
        this.name = name;
        this.host = host;
        this.port = port;
    }

    public bool update(string jsonconfig) {
        try {
            JsonUtility.FromJsonOverwrite(jsonconfig, this);
        }
        catch { 
            return false;
        }
        Debug.Log($"Team {id} updated: tag {tag}, name {name}, host {host}, port {port}");
        return true;
    }
}

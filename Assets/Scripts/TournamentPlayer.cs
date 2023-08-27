using UnityEngine;

public class TournamentPlayer {
    private TeamInfo player; 
    private TeamConnector client;

    public TournamentPlayer(TeamInfo player) {
        this.player = player;
        this.client = new TeamConnector(player.host, player.port);
    }
    public void requestActions(UnitInfoCollection units) {
        string data = JsonUtility.ToJson(units);
        //Debug.Log("requestActions(" + data + ")");
        client.sendRequest(TeamConnector.Requests.ACTIONS, data, onActionsReceived);
    }

    public string playerName() {
        return player.name;
    }

    public string info() {
        string template = "Client {0} '{1}', on {2}:{3}";
        return string.Format(template, player.id, player.name, player.host, player.port);
    }

    private void onActionsReceived(string data) {
        Debug.Log("Received: " + data);
    }
}

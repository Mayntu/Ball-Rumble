using Unity.VisualScripting;
using UnityEngine;

public class TournamentPlayer {
    private TeamInfo player; 
    private TeamConnector client;
    public bool ready {  get; private set; }
    public string errorDescription { get; private set; } = "not ready";

    public TournamentPlayer(TeamInfo player) {
        this.player = player;
        this.client = new TeamConnector(player.host, player.port);
        this.client.setErrorHandler(errorReceiver);
    }

    public string playerName() {
        return player.name;
    }

    public string playerTag() {
        return player.tag;
    }

    public int playerId() {
        return player.id;
    }

    public int playerPort() {
        return player.port;
    }


    public string info() {
        return $"Client {player.id} '{player.name}', on {player.host}:{player.port}";
    }


    public void requestActions(UnitInfoCollection units) {
        if (!ready) {
            Debug.LogError($"TournamentPlayer.requestActions(): Player {player.id} not ready!");
            return;
        }
        string data = JsonUtility.ToJson(units);
        client.sendRequest(TeamConnector.Requests.ACTIONS, data, actionsReceiver);
    }

    public void requestReady() {
        string data = "{" + "\"data\": {\"teamName\": \"%player.tag%\"}" + "}";
        data = data.Replace("%player.tag%", player.tag);
        client.sendRequest(TeamConnector.Requests.READY, data, readyReceiver);
    }

    public void requestGameOver() {
        if (!ready) {
            Debug.LogError($"TournamentPlayer.requestGameOver(): Player {player.id} not ready!");
            return;
        }
        string data = "{" + "\"data\": {}" + "}";
        client.sendRequest(TeamConnector.Requests.GAMEOVER, data, null);
    }



    private void readyReceiver(string data) {
        if (data.Contains("ok")) { 
            ready = true;
            errorDescription = "";
            TournamentController.Instance.playerReadyHandler(player.id);
        }
    }

    private void actionsReceiver(string data) {
        //Debug.Log("Received: " + data);
        errorDescription = "";
        UnitActionCollection res = JsonUtility.FromJson<UnitActionCollection>(data);
        TournamentController.Instance.playerActionsHandler(player.id, res);
    }

    private void gameoverReceiver(string data) {
        TournamentController.Instance.playerGameOverHandler(player.id);
    }

    private void errorReceiver(string description) {
        errorDescription = description;
        TournamentController.Instance.playerFailedHandler(player.id);
    }
}

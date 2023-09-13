using UnityEngine;
using System.Linq;

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
        client.sendRequest(TeamConnector.Requests.ACTIONS, data, actionsReceiver);
    }

    public string playerName() {
        return player.name;
    }

    public int playerId() {
        return player.id;
    }

    public string info() {
        string template = "Client {0} '{1}', on {2}:{3}";
        return string.Format(template, player.id, player.name, player.host, player.port);
    }

    private bool isInMyTeam(int id) {
        //Debug.Log("Searching " + id.ToString());
        GameObject obj = GameObject.Find(id.ToString());
        if (obj != null) {
            //Debug.Log("Object found");
            return obj.CompareTag(player.name);
        }
        //Debug.Log("Object NULL");
        return false;
    }


    private void actionsReceiver(string data) {
        //Debug.Log("Received: " + data);
        UnitActionCollection res = JsonUtility.FromJson<UnitActionCollection>(data);
        TournamentController.Instance.Enqueue(() => {
            UnitActionCollection myActions = new();
            myActions.data = res.data.Where((unit) => isInMyTeam(unit.id)).ToArray();
            //Debug.Log("MyActions (" + myActions.data.Length.ToString() + "): " + JsonUtility.ToJson(myActions));
            TournamentController.Instance.refreshUnitActions(player.id, myActions);
        });

    }
}

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class TournamentController : MonoBehaviour {
    // класс будет синглтоном
    private static TournamentController instance;
    private static readonly Queue<Action> executionQueue = new Queue<Action>();

    [SerializeField] private uint updatesPerRequest = 3;
    [SerializeField] private uint totalTeams = 2;
    [SerializeField] private uint unitsInTeam = 6; // в релизной версии должно быть по 6 юнитов в команде
    /* Важно! В любом случае, в каждой команде должно быть одинаковое количество юнитов!
       Иначе игра упадёт с NullReferenceException */

    private uint updatesCount = 0;
    private CliManager cli;
    private bool updatedByCli = false;
    private TournamentPlayer[] teams = null;
    private UnitInfoCollection objectsInfo = new();
    private struct TagName { public string tag; public string name; }
    private TagName[] teamNames = { 
        new TagName {tag="RedPlayer",  name="Red"},
        new TagName {tag="BluePlayer", name="Blue"}
    };
    

    public static TournamentController Instance {
        get {
            if (instance == null) {
                // если экземпляр не был создан - значит, что-то не так со сценой, падаем!
                throw new Exception("NO instance of TournamentController!");
            }
            return instance;
        }
    }


    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        cli = new CliManager();
        teams = new TournamentPlayer[totalTeams];
    }


    void Start() {
        createTestPlayers();
        createObjectsInfo();
    }
    

    void FixedUpdate() {
        updatesCount++;
        if (updatesCount % updatesPerRequest != 0) return;
        updatesCount = 0;
        refreshObjectsInfo();

        // Этот блок временный
        bool allPlayersReady = true;
        for(int i=0; i<totalTeams; i++) allPlayersReady &= isPlayerReady(i);
        if (!allPlayersReady) {
            foreach (TournamentPlayer player in teams) player.requestReady();
            return;
        }

        foreach (TournamentPlayer player in teams) {
            if (player.playerId() == 0) {
                player.requestActions(objectsInfo);
            }
            else {
                player.requestActions(objectsInfo.mirrored());
            }
        }
    }


    void OnDestroy() {
        foreach (TournamentPlayer player in teams) {
            player.requestGameOver();
        }
    }


    void Update() {
        lock (executionQueue) {
            while (executionQueue.Count > 0) {
                executionQueue.Dequeue().Invoke();
            }
        }
    }

    public bool isServerSide() {
        bool r = updatedByCli && cli.teamsConfigured() >= totalTeams;
        if (r) Debug.Log("RUNNING ON SERVER SIDE");
        return r;
    }


    public void setPlayerName(int i,  string name) {
        if (!string.IsNullOrEmpty(name) && 0 <= i && i < teamNames.Length) {
            teamNames[i].name = name;
        }
    }


    public string getPlayerName(int i) {
        return teamNames[i].name;
    }

    public int getPlayerPort(int i) {
        return teams[i].playerPort();
    }


    public bool isPlayerReady(int i) {
        return teams[i].ready;
    }


    public void playerActionsHandler(int team_id, UnitActionCollection actions) {
        enqueue(() => {
            UnitActionCollection myActions = new();
            myActions.data = actions.data.Where((unit) => isATeamMember(team_id, unit.id)).ToArray();
            //Debug.Log($"myActions.data.Length = {myActions.data.Length}");
            refreshUnitActions(team_id, myActions);
        });
    }


    public void playerReadyHandler(int team_id) {
        // enqueue(() => { 
        // });
    }


    public void playerGameOverHandler(int team_id) {
        // enqueue(() => {
        // });
    }



    private void enqueue(Action action) {
        lock (executionQueue) {
            executionQueue.Enqueue(action);
        }
    }

    private bool isATeamMember(int team_id, int unit_id) {
        GameObject obj = GameObject.Find(unit_id.ToString());
        if (obj != null) {
            return obj.CompareTag(teams[team_id].playerTag());
        }
        return false;
    }

    private void refreshUnitActions(int team_id, UnitActionCollection actions) {
        foreach (UnitActionRecord action in actions.data) {
            if (team_id == 1) action.mirror();
            GameObject unit = GameObject.Find(action.id.ToString());
            unit.GetComponent<UnitAction>().set(action.type, action.force, action.direction, action.angle);
        }
    }


    private void refreshObjectsInfo() {
        foreach (UnitInfo unit in objectsInfo.data) {
            unit.refresh();
        }
    }


    private void createObjectsInfo() {
        // размер массива в objectsInfo: количество команд * юнитов в команде + мяч + 4 штанги
        objectsInfo.data = new UnitInfo[totalTeams * unitsInTeam + 1 + 4];
        Debug.Log($"Created UnitInfo for {objectsInfo.data.Length} game objects");
        // добавляем мяч
        objectsInfo.data[0] = new UnitInfo(GameObject.FindWithTag("Ball"));
        uint i = 1;
        // добавляем штанги
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GoalPost");
        foreach (GameObject obj in objects) {
            objectsInfo.data[i] = new UnitInfo(obj);
            i++;
        }
        // добавляем юниты
        for (uint t=0; t<totalTeams; t++) {
            objects = GameObject.FindGameObjectsWithTag(teams[t].playerTag());
            foreach(GameObject obj in objects) {
                objectsInfo.data[i] = new UnitInfo(obj);
                i++;
            }
        }
    }


    private void createTestPlayers() {
        for (int i = 0; i < totalTeams; i++) {
            string playerTag = teamNames[i].tag;
            string playerName = teamNames[i].name;
            string playerHost = "127.0.0.1";
            int playerPort = 8201 + i;
            TeamInfo client = new(i, playerTag, playerName, playerHost, playerPort);
            updatedByCli |= client.update(cli.getTeamConfigById(i));
            teams[i] = new TournamentPlayer(client);
            unitsInTeam = (uint) GameObject.FindGameObjectsWithTag(playerTag).Length;
        }
    }

}

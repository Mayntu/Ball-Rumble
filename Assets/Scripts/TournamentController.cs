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
    private System.Threading.Timer quitTimer = null;
    private System.Threading.Timer readyTimer = null;
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
        if (isServerSide()) {
            readyTimer = new(readyTimerHandler, null, TimeSpan.FromSeconds(2), TimeSpan.Zero);
        }
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


    void Update() {
        lock (executionQueue) {
            while (executionQueue.Count > 0) {
                executionQueue.Dequeue().Invoke();
            }
        }
    }


    public void onGameOver() {
        TournamentResult result = new(teams);
        GoalSystem goals = GameObject.Find("GameManager").GetComponent<GoalSystem>();
        foreach (TournamentPlayer player in teams) {
            int id = player.playerId();
            result.setError(id, player.errorDescription);
            result.setScore(id, goals.GetScore(id));
            player.requestGameOver();
        }
        VideoRecorder vr = GameObject.Find("GameManager").GetComponent<VideoRecorder>();
        // TODO: Uncomment when 'fileName' is available in VideoRecorder
        // if (vr != null) result.video = vr.fileName;
        cli.echo($"RESULTS: {result.toJson()}");
        if (isServerSide()) {
            quitTimer = new(quitTimerHandler, null, TimeSpan.FromSeconds(2), TimeSpan.Zero);
        }
    }


    public bool isServerSide() {
        bool r = updatedByCli && cli.teamsConfigured() >= totalTeams;
        if (r) cli.echo("RUNNING ON SERVER SIDE");
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


    public void playerFailedHandler(int team_id) {
        enqueue(() => {
            GameUIManager gameui = GameObject.Find("GameManager").GetComponent<GameUIManager>();
            if (gameui != null) {
                string er = $"Player '{teamNames[team_id].name}': {teams[team_id].errorDescription}";
                gameui.GameOver(er);
            }
        });
    }


    private void quitTimerHandler(object? _) {
        enqueue(() => { Application.Quit(); });
    }


    private void readyTimerHandler(object? _) {
        enqueue(() => {
            foreach (var team in teams) {
                if (!team.ready) {
                    int team_id = team.playerId();
                    GameUIManager gameui = GameObject.Find("GameManager").GetComponent<GameUIManager>();
                    Debug.Log($"checkAllReady(): team {team_id} not ready, gameui = {gameui}");
                    if (gameui != null) {
                        string er = $"Player '{teamNames[team_id].name}': {teams[team_id].errorDescription}";
                        gameui.GameOver(er);
                    }
                }
            }
        });
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

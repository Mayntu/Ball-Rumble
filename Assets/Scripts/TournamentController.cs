using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
    private TournamentPlayer[] teams = null;
    private UnitInfoCollection objectsInfo = new();



    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        teams = new TournamentPlayer[totalTeams];
    }


    void Start() {
        createTestPlayers();
        createObjectsInfo();
    }


    public static TournamentController Instance {
        get {
            // если экземпляр не был создан - значит, что-то не так со сценой, падаем!
            if (instance == null) {
                throw new Exception("NO instance of TournamentController!");
            }
            return instance;
        }
    }

    

    void FixedUpdate() {
        updatesCount++;
        if (updatesCount % updatesPerRequest != 0) return;
        updatesCount = 0;
        refreshObjectsInfo();
        foreach(TournamentPlayer player in teams) {
            if (player.playerId() == 0) {
                player.requestActions(objectsInfo);
            } else {
                player.requestActions(objectsInfo.mirrored());
            }
        }
    }

    private void Update() {
        lock (executionQueue) {
            while (executionQueue.Count > 0) {
                executionQueue.Dequeue().Invoke();
            }
        }
    }


    public void Enqueue(Action action) {
        lock (executionQueue) {
            executionQueue.Enqueue(action);
        }
    }


    private string[] actionNames = new[] { "none", "run", "throw", "kick", "jump" };

    public void refreshUnitActions(int team_id, UnitActionCollection actions) {
        foreach(UnitActionRecord action in actions.data) {
            if (team_id == 1) {
                action.mirror();
            }
            GameObject unit = GameObject.Find(action.id.ToString());
            int act_type = Array.IndexOf(actionNames, action.type);
            if (act_type != -1) {
                unit.GetComponent<UnitAction>().set((UnitAction.Types)act_type, action.force, action.direction, action.angle);
            }
        }
    }


    private void refreshObjectsInfo() {
        foreach(UnitInfo unit in objectsInfo.data) {
            unit.refresh();
        }
    }



    private void createObjectsInfo() {
        // размер массива в objectsInfo: количество команд * юнитов в команде + мяч + 4 штанги
        objectsInfo.data = new UnitInfo[totalTeams * unitsInTeam + 1 + 4];
        Debug.Log(objectsInfo.data.Length);
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
            objects = GameObject.FindGameObjectsWithTag(teams[t].playerName());
            foreach(GameObject obj in objects) {
                objectsInfo.data[i] = new UnitInfo(obj);
                i++;
            }
        }
    }


    private void createTestPlayers() {
        for (int i = 0; i < totalTeams; i++) {
            string playerName = (i == 0) ? "RedPlayer" : "BluePlayer";
            string playerHost = "127.0.0.1";
            int playerPort = 8201 + i;
            TeamInfo client = new(i, playerName, playerHost, playerPort);
            teams[i] = new TournamentPlayer(client);
            unitsInTeam = (uint) GameObject.FindGameObjectsWithTag(playerName).Length;
        }
    }




}

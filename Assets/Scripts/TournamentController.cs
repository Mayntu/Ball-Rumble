using UnityEngine;

public class TournamentController : MonoBehaviour {
    [SerializeField] private uint totalTeams = 2;
    [SerializeField] private uint unitsInTeam = 6; // в релизной версии должно быть по 6 юнитов в команде
    /* Важно! В любом случае, в каждой команде должно быть одинаковое количество юнитов!
       Иначе игра упадёт с NullReferenceException */
    [SerializeField] private uint updatesPerRequest = 3;
    private uint updatesCount = 0;
    private TournamentPlayer[] teams = null;
    private UnitInfoCollection objectsInfo = new();

    void Awake() {
        teams = new TournamentPlayer[totalTeams];
        createTestPlayers();
    }

    void Start() {
        createObjectsInfo();
    }

    void FixedUpdate() {
        updatesCount++;
        if (updatesCount % updatesPerRequest != 0) return;
        updatesCount = 0;
        refreshObjectsInfo();
        foreach(TournamentPlayer player in teams) {
            player.requestActions(objectsInfo);
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

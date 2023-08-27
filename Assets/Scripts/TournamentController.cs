using UnityEngine;

public class TournamentController : MonoBehaviour {
    [SerializeField] private uint totalTeams = 2;
    [SerializeField] private uint unitsInTeam = 1; // 6 units should be in release
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
        // ����� ������� objectsInfo: ���������� ������ � ������ ������� + ��� + 4 ������ �����
        objectsInfo.data = new UnitInfo[totalTeams * unitsInTeam + 1 + 4];
        Debug.Log(objectsInfo.data.Length);
        // ��������� ���
        objectsInfo.data[0] = new UnitInfo(GameObject.FindWithTag("Ball"));
        uint i = 1;
        // ��������� ������
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GoalPost");
        foreach (GameObject obj in objects) {
            objectsInfo.data[i] = new UnitInfo(obj);
            i++;
        }
        // ��������� ����� ������
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
            string playerHost = "localhost";
            int playerPort = 8200 + i;
            TeamInfo client = new(i, playerName, playerHost, playerPort);
            teams[i] = new TournamentPlayer(client);
        }
    }
}

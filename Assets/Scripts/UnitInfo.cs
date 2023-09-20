using System;
using UnityEngine;

[Serializable]
public class UnitInfo {
    public int id;
    public string tag;
    public Vector3 position;
    public UnitSize size;
    public bool has_ball;

    public UnitInfo(GameObject unit) {
        id = unit.GetInstanceID();
        unit.name = id.ToString();
        tag = unit.tag;
        position = unit.transform.position;
        CatchBall ball = unit.GetComponent<CatchBall>();
        has_ball = (ball != null) && ball.isCatched;
        //size.height = unit.GetComponent<CapsuleCollider>().height;
        //size.radius = unit.GetComponent<CapsuleCollider>().radius;
    }

    public UnitInfo(UnitInfo other) {
        id = other.id;
        tag = other.tag;
        position = new(other.position.x, other.position.y, other.position.z);
        size = other.size;
        has_ball = other.has_ball;
    }

    public void mirror() {
        position.x *= -1;
        position.z *= -1;
    }

    public void refresh() {
        GameObject obj = GameObject.Find(id.ToString());
        position = obj.transform.position;
        CatchBall ball = obj.GetComponent<CatchBall>();
        has_ball = (ball != null) && ball.isCatched;
        //size.height = obj.GetComponent<CapsuleCollider>().height;
        //size.radius = obj.GetComponent<CapsuleCollider>().radius;
    }

}


[Serializable]
public class UnitInfoCollection {
    public UnitInfo[] data;
    public UnitInfoCollection mirrored() {
        UnitInfoCollection c = new();
        c.data = new UnitInfo[data.Length];
        for(int i = 0; i < data.Length; i++) {
            c.data[i] = new UnitInfo(data[i]);
            c.data[i].mirror();
        }
        return c;
    }
}

[Serializable]
public class UnitSize {
    public float height;
    public float radius;
}
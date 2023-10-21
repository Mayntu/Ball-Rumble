using System;
using UnityEngine;

[Serializable]
public class UnitInfo {
    public int id;
    public string tag;
    public Vector3 position;
    public UnitSize size;
    public float stamina;
    public bool hasBall;

    public UnitInfo(GameObject unit) {
        id = unit.GetInstanceID();
        unit.name = id.ToString();
        tag = unit.tag;
        position = unit.transform.position;
        CatchBall ball = unit.GetComponent<CatchBall>();
        hasBall = (ball != null) && ball.isCatched;
        size = new UnitSize();
        CapsuleCollider collider = unit.GetComponent<CapsuleCollider>();
        size.height = (collider != null) ? collider.height : 0;
        size.radius = (collider != null) ? collider.radius : 0;
        PlayerMovement movement = unit.GetComponent<PlayerMovement>();
        stamina = (movement != null) ? movement.stamina : 0;
    }

    public UnitInfo(UnitInfo other) {
        id = other.id;
        tag = other.tag;
        position = new(other.position.x, other.position.y, other.position.z);
        size = new UnitSize(other.size.height, other.size.radius);
        hasBall = other.hasBall;
    }

    public void mirror() {
        position.x *= -1;
        position.z *= -1;
    }

    public void refresh() {
        GameObject obj = GameObject.Find(id.ToString());
        position = obj.transform.position;
        CatchBall ball = obj.GetComponent<CatchBall>();
        hasBall = (ball != null) && ball.isCatched;
        CapsuleCollider collider = obj.GetComponent<CapsuleCollider>();
        size.height = (collider != null) ? collider.height : 0;
        size.radius = (collider != null) ? collider.radius : 0;
        PlayerMovement movement = obj.GetComponent<PlayerMovement>();
        stamina = (movement != null) ? movement.stamina : 0;
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

    public UnitSize(float height = 0, float radius = 0) { this.height = height; this.radius = radius; }
}
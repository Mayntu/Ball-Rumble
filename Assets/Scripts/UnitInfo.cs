using System;
using UnityEngine;

[Serializable]
public class UnitInfo {
    public int id;
    public string tag;
    public Vector3 position;
    public UnitSize size;
    public float stamina;
    public string feature;


    public UnitInfo(GameObject unit) {
        id = unit.GetInstanceID();
        unit.name = id.ToString();
        tag = unit.tag;
        size = new UnitSize();
        feature = "";
        refresh(unit);
    }


    public UnitInfo(UnitInfo other) {
        id = other.id;
        tag = other.tag;
        position = new(other.position.x, other.position.y, other.position.z);
        size = new UnitSize(other.size.height, other.size.radius);
        feature = other.feature;
    }


    public void mirror() {
        position.x *= -1;
        position.z *= -1;
    }


    public void refresh(GameObject obj = null) {
        if (obj == null) {
            obj = GameObject.Find(id.ToString());
            if (obj == null) return;
        }

        position = obj.transform.position;
        CatchBall ball = obj.GetComponent<CatchBall>();
        if (ball != null) {
            feature = ball.isCatched ? "hasBall" : feature;
        }
        CapsuleCollider collider = obj.GetComponent<CapsuleCollider>();
        if (collider != null) {
            size.height = collider.height;
            size.radius = collider.radius;
        }
        PlayerMovement movement = obj.GetComponent<PlayerMovement>();
        if (movement != null) {
            stamina = movement.stamina;
            feature = movement.isFallen ? "fallen" : feature;
        }
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
using System;
using UnityEngine;

public class UnitAction : MonoBehaviour {
    public enum Types {
        NONE,
        RUN,
        THROW,
        KICK,
        JUMP
    }

    public Types type = Types.NONE;
    public uint force = 0;
    public double direction = 0;
    public double verticalAngle = 0;


    public void set(Types actionType, uint actionForce, double relativeDirection = 0, double verticalAngle = 0) {
        type = actionType;
        force = (actionForce <= 100) ? actionForce : 100;
        direction = relativeDirection;
        this.verticalAngle = verticalAngle;
    }

}


[Serializable]
public class UnitActionRecord {
    public int id;
    public string type;
    public uint force;
    public double direction;
    public double angle;

    public void mirror() {
        direction = direction - 180;
    }
}

[Serializable]
public class UnitActionCollection {
    public UnitActionRecord[] data;
}
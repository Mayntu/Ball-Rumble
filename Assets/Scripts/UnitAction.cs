using System;
using UnityEngine;

public class UnitAction : MonoBehaviour {
    public enum Types { NONE, RUN, THROW, KICK, JUMP };
    public static string[] names = new[] { "none", "run", "throw", "kick", "jump" };

    public static Types TypeFromString(string actionTypeName) {
        int action_index = Array.IndexOf(names, actionTypeName);
        if (action_index != -1) return (Types)action_index;
        else return Types.NONE;
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

    public void set(string actionTypeName, uint actionForce, double relativeDirection = 0, double verticalAngle = 0) {
        set(TypeFromString(actionTypeName), actionForce, relativeDirection, verticalAngle);
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
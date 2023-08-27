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


    void set(Types actionType, uint actionForce, double relativeDirection = 0, double verticalAngle = 0) {
        type = actionType;
        force = (actionForce <= 100) ? actionForce : 100;
        direction = relativeDirection;
        this.verticalAngle = verticalAngle;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitInfo {
    public string tag;
    public Vector3 position;
    //public Vector3 size;    // TODO: �������� ������� ��������

    public UnitInfo(GameObject unit) {
        tag = unit.tag;
        position = unit.transform.position;
    }
}

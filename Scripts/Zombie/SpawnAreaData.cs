using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnAreaData
{
    [HideInInspector] public Vector3 center;
    public Transform centerTrans;
    public Vector2 size;
}

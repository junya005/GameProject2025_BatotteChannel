using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GenerateSettingScriptableObject")]
public class GenerateSettingScriptableObject : ScriptableObject
{
    public int player;
    public float timing;
    public Vector3 generatePos;
}

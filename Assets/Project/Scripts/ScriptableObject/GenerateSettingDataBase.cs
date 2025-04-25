using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GenerateSettingDataBase")]
public class GenerateSettingDataBase : ScriptableObject
{
    public List<GenerateSettingScriptableObject> generateSettingList;
}

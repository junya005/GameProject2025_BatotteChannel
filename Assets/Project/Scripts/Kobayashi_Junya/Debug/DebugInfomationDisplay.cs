using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// デバック用の情報を表示するクラス
/// </summary>
public class DebugInfomationDisplay : Singleton<DebugInfomationDisplay>
{
    [SerializeField]
    private TextMeshProUGUI _debugInfoText;

    // Update is called once per frame
    void Update()
    {
        // fpsとメモリを表示
        float fps = 1.0f / Time.deltaTime;
        _debugInfoText.text = $"FPS : {fps}\nMemory : {Mathf.Round(GC.GetTotalMemory(false) / 1000000.0f)}Mib";
    }
}

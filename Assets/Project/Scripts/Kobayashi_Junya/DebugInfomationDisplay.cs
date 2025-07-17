using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebugInfomationDisplay : Singleton<DebugInfomationDisplay>
{
    [SerializeField]
    private TextMeshProUGUI _debugInfoText;

    public bool isAutoPlay;

    // Update is called once per frame
    void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        _debugInfoText.text = $"FPS : {fps}\nMemory : {GC.GetTotalMemory(false)}";
    }

    public void SetIsAutoPlay(bool value)
    {
        isAutoPlay = value;
    }
}

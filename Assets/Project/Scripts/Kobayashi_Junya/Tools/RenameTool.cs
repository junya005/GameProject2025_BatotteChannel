using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenameTool : EditorWindow
{
    private string renameText;
    private string prefixText;

    [MenuItem("Tool/BatotteChannel/RenameTool")]
    private static void ShowWindow()
    {
        RenameTool window = GetWindow<RenameTool>();
        window.titleContent = new GUIContent("Rename Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("改名ツール");

    }
}

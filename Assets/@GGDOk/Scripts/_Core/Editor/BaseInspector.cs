
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;
// using Debug = UnityEngine.Debug;

[CanEditMultipleObjects]
public class BaseInspector : Editor
{
    // private Event storedEvent;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUI.enabled = false;
        GUI.SetNextControlName("Editor");
        var a = EditorGUILayout.ObjectField("Editor", this, GetType(), false);
        string scriptPath = GetScriptPath(GetType());
        // CheckObjectFieldDoubleClick();
        if (!string.IsNullOrEmpty(scriptPath))
        {
            if (CheckObjectFieldDoubleClick())
            {
                OpenScriptInExternalEditor(scriptPath);
            }
        }
        GUI.enabled = true;
    }
    
    private bool CheckObjectFieldDoubleClick()
    {
        // 마우스 이벤트 감지
        Event current = Event.current;
        Rect mouseRect = new Rect(current.mousePosition.x, current.mousePosition.y, 1, 1);
        if (current.clickCount == 2)
        {
            if (GUI.GetNameOfFocusedControl() == "Editor" && mouseRect.Contains(current.mousePosition))
            {
                current.Use();
                return true;
            }
        }
        return false;
    }
    private static string GetScriptPath(System.Type scriptType)
    {
        string[] scriptGuids = AssetDatabase.FindAssets($"{scriptType.Name} t:Script");
        foreach (string guid in scriptGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return path;
        }
        return null;
    }

    private static void OpenScriptInExternalEditor(string scriptPath)
    {
        string externalEditorPath = EditorPrefs.GetString("kScriptsDefaultApp");
        if (string.IsNullOrEmpty(externalEditorPath))
        {
            externalEditorPath = "code";
        }
        string fullPath = Path.GetFullPath(scriptPath);
        Process.Start(externalEditorPath, fullPath);
    }
}

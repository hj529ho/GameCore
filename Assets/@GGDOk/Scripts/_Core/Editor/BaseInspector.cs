using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;
// using Debug = UnityEngine.Debug;

[CanEditMultipleObjects]
public class BaseInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // ─── 2) 에디터 스크립트 자기 자신 ───────────────────
        using (new EditorGUI.DisabledScope(true))
        {
            var self = MonoScript.FromScriptableObject(this);
            EditorGUILayout.ObjectField("Editor Script", self,
                typeof(MonoScript), false);
        }
    }
}
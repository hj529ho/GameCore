using UnityEditor;
using UnityEngine;
using System.IO;

public class BuildSettingsWindow : EditorWindow
{
    private BuildSettingsSO buildSettings;
    private const string assetPath = "Assets/BuildSettings.asset";
    private int selectedTab = 0;
    private string[] tabs = { "Android", "iOS" };

    [MenuItem("Build/Build Settings")]
    public static void ShowWindow()
    {
        GetWindow<BuildSettingsWindow>("Build Settings");
    }

    private void OnEnable()
    {
        LoadOrCreateSettings();
    }

    private void LoadOrCreateSettings()
    {
        buildSettings = AssetDatabase.LoadAssetAtPath<BuildSettingsSO>(assetPath);
        if (buildSettings == null)
        {
            buildSettings = ScriptableObject.CreateInstance<BuildSettingsSO>();
            AssetDatabase.CreateAsset(buildSettings, assetPath);
            AssetDatabase.SaveAssets();
        }
    }

    private void OnGUI()
    {
        if (buildSettings == null)
        {
            LoadOrCreateSettings();
        }

        selectedTab = GUILayout.Toolbar(selectedTab, tabs);

        switch (selectedTab)
        {
            case 0:
                DrawAndroidSettings();
                break;
            case 1:
                DrawiOSSettings();
                break;
        }

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(buildSettings);
            AssetDatabase.SaveAssets();
        }
    }

    private void DrawAndroidSettings()
    {
        buildSettings.keystoreName = EditorGUILayout.TextField("Keystore Name", buildSettings.keystoreName);
        buildSettings.keystorePass = EditorGUILayout.TextField("Keystore Pass", buildSettings.keystorePass);
        buildSettings.keyaliasName = EditorGUILayout.TextField("Key Alias Name", buildSettings.keyaliasName);
        buildSettings.keyaliasPass = EditorGUILayout.TextField("Key Alias Pass", buildSettings.keyaliasPass);
        buildSettings.outputPath = EditorGUILayout.TextField("Output Path", buildSettings.outputPath);
    }

    private void DrawiOSSettings()
    {
        buildSettings.outputPath = EditorGUILayout.TextField("iOS Output Path", buildSettings.outputPath);
    }
}
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Builder
{
    public static void BuildAndroid()
    {
        BuildSettingsSO buildSettings = AssetDatabase.LoadAssetAtPath<BuildSettingsSO>("Assets/BuildSettings.asset");

        if (buildSettings == null)
        {
            Debug.LogError("BuildSettings.asset not found!");
            return;
        }

        string[] args = System.Environment.GetCommandLineArgs();
        int buildNum = 0;
        foreach (string a in args)
        {
            if (a.StartsWith("build_num"))
            {
                var arr = a.Split(":");
                if (arr.Length == 2)
                {
                    int.TryParse(arr[1], out buildNum);
                }
            }
        }

        Debug.Log("Build Started");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = FindEnabledEditorScenes();
        buildPlayerOptions.locationPathName = buildSettings.outputPath + buildNum + ".aab";
        buildPlayerOptions.target = BuildTarget.Android;
        EditorUserBuildSettings.buildAppBundle = true;

        PlayerSettings.Android.bundleVersionCode = buildNum;
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = buildSettings.keystoreName;
        PlayerSettings.Android.keystorePass = buildSettings.keystorePass;
        PlayerSettings.Android.keyaliasName = buildSettings.keyaliasName;
        PlayerSettings.Android.keyaliasPass = buildSettings.keyaliasPass;

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded) Debug.Log("Build Success");
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Failed) Debug.Log("Build Failed");
    }

    public static void BuildiOS()
    {
        BuildSettingsSO buildSettings = AssetDatabase.LoadAssetAtPath<BuildSettingsSO>("Assets/BuildSettings.asset");

        if (buildSettings == null)
        {
            Debug.LogError("BuildSettings.asset not found!");
            return;
        }

        string[] args = System.Environment.GetCommandLineArgs();
        int buildNum = 0;
        foreach (string a in args)
        {
            if (a.StartsWith("build_num"))
            {
                var arr = a.Split(":");
                if (arr.Length == 2)
                {
                    int.TryParse(arr[1], out buildNum);
                }
            }
        }

        Debug.Log("Build Started");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = FindEnabledEditorScenes();
        buildPlayerOptions.locationPathName = buildSettings.outputPath + buildNum;
        buildPlayerOptions.target = BuildTarget.iOS;

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded) Debug.Log("Build Success");
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Failed) Debug.Log("Build Failed");
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }

        return EditorScenes.ToArray();
    }
}
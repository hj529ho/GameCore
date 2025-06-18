using UnityEngine;

[CreateAssetMenu(fileName = "BuildSettings", menuName = "Build/BuildSettings")]
public class BuildSettingsSO : ScriptableObject
{
    #if UNITY_EDITOR
    public string keystoreName;
    public string keystorePass;
    public string keyaliasName;
    public string keyaliasPass;
    public string outputPath;
    #endif
}
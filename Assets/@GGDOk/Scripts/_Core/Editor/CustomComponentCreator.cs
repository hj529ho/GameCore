using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// [InitializeOnLoad]
public static class CustomComponentCreator
{
      
    [MenuItem("GameObject/UI/HAMA/Canvas - SafeArea", false, 10)]
    static void CreateSafeAreaCanvas(MenuCommand menuCommand)
    {
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/Canvas.prefab";
        CreateComponent(path, "SafeCanvas", menuCommand);
    }
    [MenuItem("GameObject/UI/HAMA/Canvas - Scaled(SafeArea)", false, 10)]
    static void CreateScaledCanvas(MenuCommand menuCommand)
    {
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/ScaledCanvas.prefab";
        CreateComponent(path, "ScaledCanvas", menuCommand);
    }
    [MenuItem("GameObject/UI/HAMA/ToggleTap", false, 10)]
    static void CreateToggleTap(MenuCommand menuCommand)
    {
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/ToggleTab.prefab";
        CreateComponent(path, "ToggleTab", menuCommand);
    }
    [MenuItem("GameObject/UI/HAMA/Text - TMP Locale", false, 10)]
    static void LocaleText(MenuCommand menuCommand)
    {
        Debug.Log("아직 구현되지 않았습니다.");
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/LocaleText.prefab";
        CreateComponent(path, "LocaleText", menuCommand);
    }
    
    [MenuItem("GameObject/UI/HAMA/Buttons/Button - OK", false, 10)]
    static void CreateOkButton(MenuCommand menuCommand)
    {
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/OkButton.prefab";
        CreateComponent(path, "OkButton", menuCommand);
    }
    #region 프리팹 미제작

    [MenuItem("GameObject/UI/HAMA/Buttons/Button - Cancel", false, 10)]
    static void CreateCancelButton(MenuCommand menuCommand)
    {
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/CancelButton.prefab";
        CreateComponent(path, "CancelButton", menuCommand);
    }
    
    // [MenuItem("GameObject/UI/HAMA/Slider", false, 10)]
    static void CreateSlider(MenuCommand command) 
    {
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/Slider.prefab";
        CreateComponent(path, "Slider", command);
    }
    
    // [MenuItem("GameObject/UI/HAMA/ModalPopup", false, 10)]
    static void CreateModal(MenuCommand menuCommand)
    {
        var path = "Assets/@HaMa/@Resources/EditorPrefabs/Modal.prefab";
        CreateComponent(path, "Modal", menuCommand);
    }
    #endregion
    

    private static void CreateComponent(string path, string undoName, MenuCommand menuCommand)
    {
        GameObject customCanvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (customCanvasPrefab == null)
        {
            Debug.LogError($"Custom Canvas prefab not found at {path}");
            return;
        }

        // Get the current stage
        var currentStage = StageUtility.GetCurrentStageHandle();
        GameObject parentObject = menuCommand.context as GameObject;

        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(customCanvasPrefab);
        Undo.RegisterCreatedObjectUndo(go, undoName);

        // If we're in prefab mode, parent to the prefab root if no specific parent is selected
        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (parentObject == null)
            {
                parentObject = prefabStage.prefabContentsRoot;
            }
        }

        GameObjectUtility.SetParentAndAlign(go, parentObject);
        Selection.activeObject = go;
        PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.UserAction);
    }
}
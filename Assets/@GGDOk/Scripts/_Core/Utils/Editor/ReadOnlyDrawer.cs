// https://discussions.unity.com/t/how-to-make-a-readonly-property-in-inspector/75448/5
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // KCY : List create, delete not locked by GUI.enabled
        var shouldEnabled = false;
        if (attribute is ReadOnlyAttribute attributeReadOnly)
        {
            if (attributeReadOnly.WritableOnPlay)
            {
                shouldEnabled = EditorApplication.isPlaying;
            }
            if (attributeReadOnly.WritableOnEditor)
            {
                shouldEnabled = !EditorApplication.isPlaying;
            }
            if (attributeReadOnly.WritableOnInstance) {
                Object target = property.serializedObject.targetObject;
                bool isPrefabAsset = PrefabUtility.GetPrefabAssetType(target) == PrefabAssetType.NotAPrefab;
                if (isPrefabAsset)
                {
                    shouldEnabled = true;
                }
            }
        }
        GUI.enabled = shouldEnabled;
        try
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        finally
        {
            GUI.enabled = true;
        }
    }
}
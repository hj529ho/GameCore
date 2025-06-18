using System;
using Core.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(SerializeDictionary<,>))]
public class SerializeDictionaryDrawer : PropertyDrawer
{
    private bool foldout = true;
    private object newKey;
    private object newValue;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        foldout = EditorGUI.Foldout(position, foldout, label);
        if (foldout)
        {
            EditorGUI.indentLevel++;
            SerializedProperty keysProperty = property.FindPropertyRelative("_keys");
            SerializedProperty valuesProperty = property.FindPropertyRelative("_values");

            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                Rect keyRect = new Rect(position.x, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, position.width / 3, EditorGUIUtility.singleLineHeight);
                Rect valueRect = new Rect(position.x + position.width / 3, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, position.width * 2 / 3 - 30, EditorGUIUtility.singleLineHeight);
                Rect removeButtonRect = new Rect(position.x + position.width / 3 + position.width * 2 / 3 - 30, position.y + (i + 1) * EditorGUIUtility.singleLineHeight, 30, EditorGUIUtility.singleLineHeight);

                EditorGUI.BeginDisabledGroup(true);
                DrawPropertyField(keyRect, keysProperty.GetArrayElementAtIndex(i));
                EditorGUI.EndDisabledGroup();
                DrawPropertyField(valueRect, valuesProperty.GetArrayElementAtIndex(i));

                if (GUI.Button(removeButtonRect, "-"))
                {
                    keysProperty.DeleteArrayElementAtIndex(i);
                    valuesProperty.DeleteArrayElementAtIndex(i);
                }
            }

            Rect newKeyRect = new Rect(position.x, position.y + (keysProperty.arraySize + 1) * EditorGUIUtility.singleLineHeight, position.width / 3, EditorGUIUtility.singleLineHeight);
            Rect newValueRect = new Rect(position.x + position.width / 3, position.y + (keysProperty.arraySize + 1) * EditorGUIUtility.singleLineHeight, position.width * 2 / 3, EditorGUIUtility.singleLineHeight);
            
            newKey = DrawNewField(newKeyRect, fieldInfo.FieldType.GetGenericArguments()[0], newKey);
            newValue = DrawNewField(newValueRect, fieldInfo.FieldType.GetGenericArguments()[1], newValue);
            
            if (GUI.Button(new Rect(position.x, position.y + (keysProperty.arraySize + 2) * EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight), "Add Element"))
            {
                if (newKey != null && newValue != null)
                {
                    keysProperty.arraySize++;
                    valuesProperty.arraySize++;
                    SetPropertyValue(keysProperty.GetArrayElementAtIndex(keysProperty.arraySize - 1), newKey);
                    SetPropertyValue(valuesProperty.GetArrayElementAtIndex(valuesProperty.arraySize - 1), newValue);
                    newKey = null;
                    newValue = null;
                }
            }

            EditorGUI.indentLevel--;
        }
    }

    private void DrawPropertyField(Rect rect, SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                property.intValue = EditorGUI.IntField(rect, property.intValue);
                break;
            case SerializedPropertyType.Float:
                property.floatValue = EditorGUI.FloatField(rect, property.floatValue);
                break;
            case SerializedPropertyType.String:
                property.stringValue = EditorGUI.TextField(rect, property.stringValue);
                break;
            case SerializedPropertyType.Boolean:
                property.boolValue = EditorGUI.Toggle(rect, property.boolValue);
                break;
            case SerializedPropertyType.Color:
                property.colorValue = EditorGUI.ColorField(rect, property.colorValue);
                break;
            case SerializedPropertyType.ObjectReference:
                property.objectReferenceValue = EditorGUI.ObjectField(rect, property.objectReferenceValue, typeof(Object), true);
                break;
            case SerializedPropertyType.LayerMask:
                property.intValue = EditorGUI.LayerField(rect, property.intValue);
                break;
            case SerializedPropertyType.Enum:
                property.enumValueIndex = EditorGUI.Popup(rect, property.enumValueIndex, property.enumDisplayNames);
                break;
            case SerializedPropertyType.Vector2:
                property.vector2Value = EditorGUI.Vector2Field(rect, GUIContent.none, property.vector2Value);
                break;
            case SerializedPropertyType.Vector3:
                property.vector3Value = EditorGUI.Vector3Field(rect, GUIContent.none, property.vector3Value);
                break;
            case SerializedPropertyType.Vector4:
                property.vector4Value = EditorGUI.Vector4Field(rect, GUIContent.none, property.vector4Value);
                break;
            case SerializedPropertyType.Rect:
                property.rectValue = EditorGUI.RectField(rect, property.rectValue);
                break;
            case SerializedPropertyType.AnimationCurve:
                property.animationCurveValue = EditorGUI.CurveField(rect, property.animationCurveValue);
                break;
            case SerializedPropertyType.Bounds:
                property.boundsValue = EditorGUI.BoundsField(rect, property.boundsValue);
                break;
            case SerializedPropertyType.Quaternion:
                property.quaternionValue = Quaternion.Euler(EditorGUI.Vector3Field(rect, GUIContent.none, property.quaternionValue.eulerAngles));
                break;
            // 다른 자료형에 대한 처리 추가
            default:
                EditorGUI.PropertyField(rect, property, GUIContent.none);
                break;
        }
    }

    private object DrawNewField(Rect rect, System.Type type, object value)
    {
        if (type == typeof(int))
        {
            return EditorGUI.IntField(rect, value != null ? (int)value : 0);
        }
        else if (type == typeof(float))
        {
            return EditorGUI.FloatField(rect, value != null ? (float)value : 0f);
        }
        else if (type == typeof(string))
        {
            return EditorGUI.TextField(rect, value != null ? (string)value : "");
        }
        else if (type == typeof(bool))
        {
            return EditorGUI.Toggle(rect, value != null ? (bool)value : false);
        }
        else if (type == typeof(Color))
        {
            return EditorGUI.ColorField(rect, value != null ? (Color)value : Color.white);
        }
        else if (type == typeof(Object))
        {
            return EditorGUI.ObjectField(rect, value != null ? (Object)value : null, typeof(Object), true);
        }
        else if (type.IsEnum)
        {
            return EditorGUI.EnumPopup(rect, value != null ? (Enum)value : (Enum)Enum.GetValues(type).GetValue(0));
        }
        else if (type == typeof(Vector2))
        {
            return EditorGUI.Vector2Field(rect, GUIContent.none, value != null ? (Vector2)value : Vector2.zero);
        }
        else if (type == typeof(Vector3))
        {
            return EditorGUI.Vector3Field(rect, GUIContent.none, value != null ? (Vector3)value : Vector3.zero);
        }
        else if (type == typeof(Vector4))
        {
            return EditorGUI.Vector4Field(rect, GUIContent.none, value != null ? (Vector4)value : Vector4.zero);
        }
        else if (type == typeof(Rect))
        {
            return EditorGUI.RectField(rect, value != null ? (Rect)value : new Rect());
        }
        else if (type == typeof(AnimationCurve))
        {
            return EditorGUI.CurveField(rect, value != null ? (AnimationCurve)value : new AnimationCurve());
        }
        else if (type == typeof(Bounds))
        {
            return EditorGUI.BoundsField(rect, value != null ? (Bounds)value : new Bounds());
        }
        else if (type == typeof(Quaternion))
        {
            return EditorGUI.Vector4Field(rect, GUIContent.none, value != null ? ((Quaternion)value).eulerAngles : Quaternion.identity.eulerAngles);
        }
        else
        {
            return EditorGUI.ObjectField(rect, value != null ? (Object)value : null, type, true);
        }
        // 다른 자료형에 대한 처리 추가
        return null;
    }

    private void SetPropertyValue(SerializedProperty property, object value)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                property.intValue = (int)value;
                break;
            case SerializedPropertyType.Float:
                property.floatValue = (float)value;
                break;
            case SerializedPropertyType.String:
                property.stringValue = (string)value;
                break;
            case SerializedPropertyType.Boolean:
                property.boolValue = (bool)value;
                break;
            case SerializedPropertyType.Color:
                property.colorValue = (Color)value;
                break;
            case SerializedPropertyType.ObjectReference:
                property.objectReferenceValue = (Object)value;
                break;
            case SerializedPropertyType.LayerMask:
                property.intValue = (int)value;
                break;
            case SerializedPropertyType.Enum:
                property.enumValueIndex = (int)value;
                break;
            case SerializedPropertyType.Vector2:
                property.vector2Value = (Vector2)value;
                break;
            case SerializedPropertyType.Vector3:
                property.vector3Value = (Vector3)value;
                break;
            case SerializedPropertyType.Vector4:
                property.vector4Value = (Vector4)value;
                break;
            case SerializedPropertyType.Rect:
                property.rectValue = (Rect)value;
                break;
            case SerializedPropertyType.AnimationCurve:
                property.animationCurveValue = (AnimationCurve)value;
                break;
            case SerializedPropertyType.Bounds:
                property.boundsValue = (Bounds)value;
                break;
            case SerializedPropertyType.Quaternion:
                property.quaternionValue = Quaternion.Euler((Vector4)value);
                break;
            // 다른 자료형에 대한 처리 추가
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (foldout)
        {
            SerializedProperty keysProperty = property.FindPropertyRelative("_keys");
            return (keysProperty.arraySize + 3) * EditorGUIUtility.singleLineHeight;
        }
        else
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
    
    
    public static bool ContainsKey(SerializedProperty property, object key)
    {
        for (int i = 0; i < property.arraySize; i++)
        {
            var aa = property.GetArrayElementAtIndex(i);
            if (SerializedPropertyEquals(aa, key))
            {
                return true;
            }
        }
        return false;
    }

    private static bool SerializedPropertyEquals(SerializedProperty property, object value)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue.Equals(value);
            case SerializedPropertyType.Float:
                return property.floatValue.Equals(value);
            case SerializedPropertyType.String:
                return property.stringValue.Equals(value);
            case SerializedPropertyType.Boolean:
                return property.boolValue.Equals(value);
            case SerializedPropertyType.Color:
                return property.colorValue.Equals(value);
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue.Equals(value);
            case SerializedPropertyType.LayerMask:
                return property.intValue.Equals(value);
            case SerializedPropertyType.Enum:
                return property.enumValueIndex.Equals(value);
            case SerializedPropertyType.Vector2:
                return property.vector2Value.Equals(value);
            case SerializedPropertyType.Vector3:
                return property.vector3Value.Equals(value);
            case SerializedPropertyType.Vector4:
                return property.vector4Value.Equals(value);
            case SerializedPropertyType.Rect:
                return property.rectValue.Equals(value);
            case SerializedPropertyType.AnimationCurve:
                return property.animationCurveValue.Equals(value);
            case SerializedPropertyType.Bounds:
                return property.boundsValue.Equals(value);
            case SerializedPropertyType.Quaternion:
                return property.quaternionValue.Equals(value);
            // 다른 자료형에 대한 처리 추가
            default:
                return false;
        }
    }
}
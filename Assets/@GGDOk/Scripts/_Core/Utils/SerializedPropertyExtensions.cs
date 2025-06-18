#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class SerializedPropertyExtensions
{
    public static bool ContainsKey(this SerializedProperty property, object key)
    {
        SerializedProperty keysProperty = property.FindPropertyRelative("_keys");

        for (int i = 0; i < keysProperty.arraySize; i++)
        {
            SerializedProperty keyProperty = keysProperty.GetArrayElementAtIndex(i);
            if (SerializedPropertyEquals(keyProperty, key))
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
#endif
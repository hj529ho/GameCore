using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

[CustomEditor(typeof(Core.UI.ToggleTap))]
public class ToggleTapEditor : Editor
{
    private SerializedProperty eventsListProperty;
    private void OnEnable()
    {
        eventsListProperty = serializedObject.FindProperty("eventsList");
    }
    public override void OnInspectorGUI()
    {
        // 대상 객체 가져오기
        Core.UI.ToggleTap toggleTap = (Core.UI.ToggleTap)target;
        // 기본 인스펙터 그리기
        DrawDefaultInspector();
        if (toggleTap.tabContent == null)
        {
            EditorGUILayout.HelpBox("Tab Content가 할당되지 않았습니다.", MessageType.Warning);
            return;
        }

        // toggleGroup이 null인지 확인
        if (toggleTap.toggleGroup == null)
        {
            EditorGUILayout.HelpBox("Toggle Group이 할당되지 않았습니다.", MessageType.Warning);
            return;
        }

        toggleTap.tapCount = EditorGUILayout.IntField("탭 개수",toggleTap.tapCount);

        // Set 버튼
        if (GUILayout.Button("만들기"))
        {
            SetTapButton(toggleTap);
        }
        for (int i = 0; i < eventsListProperty.arraySize; i++)
        {
            SerializedProperty eventDataProp = eventsListProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(eventDataProp);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void SetTapButton(Core.UI.ToggleTap targetTap)
    {
        List<GameObject> destroyList = new List<GameObject>();
            
        for(int i = 1; i < targetTap.toggleGroup.transform.childCount; i++)
        {
            destroyList.Add(targetTap.toggleGroup.transform.GetChild(i).gameObject);
            targetTap.eventsList.Clear();
        }
        foreach (var go in destroyList)
        {
            Undo.DestroyObjectImmediate(go);
        }
        for (int i = 0; i <  targetTap.tapCount; i++)
        {
            GameObject tap = Instantiate(targetTap.tabContent.gameObject);
            // 인스턴스화가 성공했는지 확인
            if (tap != null)
            {
                // 생성된 게임 오브젝트를 Undo 시스템에 등록
                Undo.RegisterCreatedObjectUndo(tap, "Create " + tap.name);
                // toggleGroup이 null이 아닌 경우 부모 설정 및 정렬
                if (targetTap.toggleGroup != null)
                {
                    GameObjectUtility.SetParentAndAlign(tap, targetTap.toggleGroup.gameObject);
                }
                var unityEvent = new UnityEngine.Events.UnityEvent<bool>();
                targetTap.eventsList.Add(unityEvent);
                var toggle = tap.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener(unityEvent.Invoke);
                toggle.group = targetTap.toggleGroup;
                tap.name = $"ToggleTap{i}";
                tap.SetActive(true);
            }
            else
            {
                Debug.LogError("프리팹 인스턴스화에 실패했습니다.");
            }    
        }
    }
}
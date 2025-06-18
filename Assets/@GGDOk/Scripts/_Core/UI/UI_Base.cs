using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    [SelectionBase]
    public class UI_Base : MonoBehaviour
    {
        Dictionary<Type, UnityEngine.Object[]> _object = new Dictionary<Type, UnityEngine.Object[]>();
        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            Debug.Log(names[0]);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _object.Add(typeof(T), objects);

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == "COUNT")
                    continue;
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Utils.Statics.FindChild(gameObject, names[i], true);
                else
                    objects[i] = Utils.Statics.FindChild<T>(gameObject, names[i], true);
                if (objects[i] == null)
                    Debug.Log($"Failed to bind({names[i]})");
            }
        }
        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            UnityEngine.Object[] objects = null;
            _object.TryGetValue(typeof(T), out objects);
            if (_object.TryGetValue(typeof(T), out objects) == false)
                return null;
            if (objects.Length <= idx)
            {
                Debug.LogError("Index is out of range");
                return null;
            }
            return objects[idx] as T;
        }

        protected bool TryGet<T>(int idx, out T result) where T : UnityEngine.Object
        {
            result = Get<T>(idx);
            if (result == null)
                return false;
            return true;
        }
        protected bool TryGet<T>(Enum @enum, out T result) where T : UnityEngine.Object
        {
            result = Get<T>(@enum);
            if (result == null)
                return false;
            return true;
        }

        protected T Get<T>(Enum enumValue) where T : UnityEngine.Object
        {
            UnityEngine.Object[] objects = null;
            _object.TryGetValue(typeof(T), out objects);
            if (_object.TryGetValue(typeof(T), out objects) == false)
                return null;
            int index = Convert.ToInt32(enumValue);
            if (objects.Length <= index)
            {
                Debug.LogError("Index is out of range");
                return null;
            }
            return objects[index] as T;
        }

        #region Utils
        protected TMP_Text GetText(Enum enumValue) { return Get<TMP_Text>(enumValue); }
        protected Button GetButton(Enum enumValue) { return Get<Button>(enumValue); }
        protected bool ListenButtonClick(Enum enumValue, UnityAction call)
        {
            var button = Get<Button>(enumValue);
            if (button == null)
            {
                return false;
            }
            button.onClick.AddListener(call);
            return true;
        }
        protected Image GetImage(Enum enumValue) { return Get<Image>(enumValue); }
        protected ToggleTap GetToggleTap(Enum enumValue) { return Get<ToggleTap>(enumValue); }
        protected RawImage GetRawImage(Enum enumValue) { return Get<RawImage>(enumValue); }

        protected Slider GetSlider(Enum enumValue) { return Get<Slider>(enumValue); }

        protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
        protected Button GetButton(int idx) { return Get<Button>(idx); }
        protected bool ListenButtonClick(int idx, UnityAction call)
        {
            var button = Get<Button>(idx);
            if (button == null)
            {
                return false;
            }
            button.onClick.AddListener(call);
            return true;
        }
        protected Image GetImage(int idx) { return Get<Image>(idx); }
        #endregion Utils

        public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_EventHandler evt = Utils.Statics.GetOrAddComponent<UI_EventHandler>(go);
            switch (type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;

                case Define.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
                case Define.UIEvent.Drop:
                    evt.OnDropHandler -= action;
                    evt.OnDropHandler += action;
                    break;
                case Define.UIEvent.PointUp:
                    evt.OnPointerUpHandler -= action;
                    evt.OnPointerUpHandler += action;
                    break;
                case Define.UIEvent.PointDown:
                    evt.OnPointerDownHandler -= action;
                    evt.OnPointerDownHandler += action;
                    break;
            }
        }
    }
}

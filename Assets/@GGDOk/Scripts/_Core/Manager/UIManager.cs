using System.Collections.Generic;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Manager
{
    // [Manager("UI", "Core")]
    public class UIManager : BaseManager
    {
        private int _order = 10;
        private readonly Stack<UI_Popup> _popupStack = new();
        private UI_Loading _loadingUI;

        private static GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Root");
                if (root == null)
                    root = new GameObject { name = "@UI_Root" };
                return root;
            }
        }

        private static GameObject GlobalUIRoot
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Global_Root");
                if (root == null)
                {
                    root = new GameObject { name = "@UI_Global_Root" };
                    GameObject.DontDestroyOnLoad(root);
                }
                return root;
            }
        }

        public override void Init()
        {
            _ = Root;
        }
        public override async UniTask InitAsync()
        {
            await UniTask.CompletedTask;
        }

        public Canvas SetCanvas(GameObject go, bool sort = true, bool story = false)
        {
            Canvas canvas = Utils.Statics.GetOrAddComponent<Canvas>(go);
            canvas.overrideSorting = true;
            if (story)
            {
                for (int i = 0; i < (int)Define.StoryUIOder.MaxCount; i++)
                {
                    Define.StoryUIOder storyUIOder = (Define.StoryUIOder)i;
                    if (storyUIOder.ToString() == go.name)
                    {
                        canvas.sortingOrder = i;
                    }
                }

                return canvas;
            }
            if (sort)
            {
                canvas.sortingOrder = _order;
                _order++;
            }
            else
            {
                canvas.sortingOrder = 0;
            }

            return canvas;
        }
        public T ShowStoryUI<T>(string name = null) where T : UI_Story
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            GameObject go = Managers.Resource.Instantiate($"UI/Story/{name}.prefab");
            T sceneUI = Utils.Statics.GetOrAddComponent<T>(go);
            go.transform.SetParent(Root.transform);
            SetCanvas(go, false, true);
            return sceneUI;
        }
        public T ShowLoadingUI<T>(string name = null) where T : UI_Loading
        {
            if (_loadingUI != null)
            {
                if (_loadingUI.gameObject.activeSelf)
                {
                    Debug.LogError("로딩 중에 다른 로딩을 띄울 수 없습니다.");
                    return null;
                }
                Managers.Resource.Destroy(_loadingUI.gameObject);
                _loadingUI = null;
            }
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            GameObject go = Managers.Resource.Instantiate($"UI/Loading/{name}.prefab");
            T sceneUI = Utils.Statics.GetOrAddComponent<T>(go);
            _loadingUI = sceneUI;
            go.transform.SetParent(GlobalUIRoot.transform);
            var canvas = SetCanvas(go, false);
            canvas.sortingOrder = 9999;
            return sceneUI;
        }
        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}.prefab");
            T sceneUI = Utils.Statics.GetOrAddComponent<T>(go);
            go.transform.SetParent(Root.transform);
            return sceneUI;
        }
        public T ShowPopupUI<T>(string name = null) where T : UI_Popup
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            var go = Managers.Resource.Instantiate($"UI/Popup/{name}.prefab");
            var popup = Utils.Statics.GetOrAddComponent<T>(go);
            _popupStack.Push(popup);

            go.transform.SetParent(Root.transform);
            return popup;
        }

        public void CloseLoadingUI()
        {
            UI_Loading loadingUI = Managers.UI._loadingUI;
            if (loadingUI == null)
                return;
            loadingUI.DoTransition(false, () =>
            {
                Managers.Resource.Destroy(loadingUI.gameObject);
                Managers.UI._loadingUI = null;
            });
        }

        public void ClosePopupUI(UI_Popup popup)
        {
            if (_popupStack.Count == 0)
                return;
            if (_popupStack.Peek() != popup)
            {
                Debug.Log("Close Popup Failed");
                return;
            }
            ClosePopupUI();
        }
        public void ClosePopupUI()
        {
            if (_popupStack.Count == 0)
                return;
            UI_Popup popup = _popupStack.Pop();
            if (_popupStack.Count != 0)
                _popupStack.Peek().Refresh();
            Managers.Resource.Destroy(popup.gameObject);
            popup = null;
            _order--;
        }
        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0)
                ClosePopupUI();
        }

        #region Miscellaneous
        public void TryShowTouchEffectUI()
        {
            var name = "UI_PointerEffects";
            var existingObj = GlobalUIRoot.transform.Find(name);
            if (existingObj != null) {
                return;
            }
            
            var obj = Managers.Resource.Instantiate($"UI/Screen/{name}.prefab");
            obj.name = name;
            obj.transform.SetParent(GlobalUIRoot.transform);
        }

        #endregion

   
    }
}
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Manager
{
    
    /// <summary>
    /// 기본 Manager 클래스 입니다. - 자동 생성된 정적 프로퍼티는 다른파일에 있습니다.
    /// 경로 - @HaMa/Scrips/Generated/ManagersGenerated.cs
    /// </summary>
    public partial class Managers : MonoBehaviour
    {
        private static Managers _instance;
        private static bool _init = false;
        public static Managers Instance
        {
            get
            {
                if (_init == false)
                {
                    _init = true;
                    if (_instance == null)
                    {
                        GameObject go = GameObject.Find("@Managers");
                        if (go == null)
                        {
                            go = new GameObject { name = "@Managers" };
                            go.AddComponent<Managers>();
                        }
                        DontDestroyOnLoad(go);
                        _instance = go.GetComponent<Managers>();
                    }
                }
                return _instance;
            }
        }

        private static bool _isLoadedAsset = false;

        public Define.OnInitDel OnInit;

        private readonly List<Func<UniTask>> _loadingSquences = new List<Func<UniTask>>();
        private int _actionIndex = 0;
        

        /// <summary>
        /// BaseScene에서 Init()을 호출하기 전에 미리 로드할 에셋을 등록.
        /// </summary>
        /// <param name="label"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterPreloadAssets<T>(string label) where T : UnityEngine.Object
        {
            RegisterPreloadActions(async () => await Resource.LoadAllAsync<T>(label, Define.LoadType.Stage));
        }
        /// <summary>
        /// BaseScene에서 Init()을 호출하기 전에 미리 실행할 비동기 기능을 등록
        /// </summary>
        /// <param name="action"></param>
        public void RegisterPreloadActions(Func<UniTask> action)
        {
            _loadingSquences.Add(action);
        }
        public void SetLoadingSequence()
        {
            _loadingSquences.Clear();
            _loadingSquences.Add(async () =>
            {
                Debug.Log("LoadAllAsync GameObject - global");
                await Resource.LoadAllAsync<GameObject>("global", Define.LoadType.Global);
                Debug.Log("Done");
            });
        }
        public async UniTask LoadAssets()
        {
            // await UniTask.WhenAll();
            // foreach (var func in _loadingSquences)
            // {
            //     Debug.Log("loading sequence");
            //     await func.Invoke();
            // }
            
            await UniTask.WhenAll(_loadingSquences.Select(func => func.Invoke()));
            UI.TryShowTouchEffectUI();
            Debug.Log("OnInit");
            OnInit.Invoke();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            if (_init != false) return;
            _init = true;
            if (Instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);
                _instance = go.GetComponent<Managers>();
            }
            UIManager.Init();
            Sound.Init();
            Scene.Init();
            Story.Init();
            UI.ClosePopupUI();
            Input.Init();
        }
        public static async UniTask InitAsync()
        {
        }
    }
}


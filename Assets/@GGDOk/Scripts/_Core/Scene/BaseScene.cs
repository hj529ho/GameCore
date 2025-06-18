using Core.Manager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace Core.Scene
{
    /// <summary>
    /// 모든 씬의 엔트리 포인트입니다.
    /// 에셋 로드, 초기화, 씬 UI 로드 등의 작업을 수행합니다.
    /// </summary>
    public abstract class BaseScene : MonoBehaviour
    {
        
        protected bool IsInit = false;
        protected virtual void Awake()
        {
            //TODO 이 부분 교체
            Managers.Instance.OnInit = Init;
            Managers.Instance.SetLoadingSequence();
            PreInitialize();
            Managers.Instance.LoadAssets().Forget(e => Debug.LogException(e));
            Debug.Log($"Awake {GetType().Name}");

        }
        /// <summary>
        /// 로딩팝업이 사라지고 실행됨
        /// </summary>
        public virtual void Init()
        {
            Object obj = FindFirstObjectByType(typeof(EventSystem));
            if (obj == null)
                Managers.Resource.Instantiate("UI/EventSystem.prefab").name = "@EventSystem";
            Managers.Scene.IsRunningLoading = false;
            Managers.UI.CloseLoadingUI();
            Debug.Log($"Init {GetType().Name}");
            IsInit = true;
        }

        private void OnDestroy()
        {
            // Managers.Instance.OnInit = null;
            //TODO 이거 다음씬 Awake랑 순서 확인해야할듯 잘못하면 Scene 로드 안될지도
        }
        
        /// <summary>
        /// 로딩 팝업이 사라지기 전 실행됨
        /// 로드할 에셋들을 등록하는 함수. Awake에서 호출됨.
        /// 이거 다 끝나면 Init이 실행됨.
        /// Managers.Instance.RegisterPreloadAssets&lt;T&gt;(string label)로 등록할 수 있음.
        /// </summary>
        protected abstract void PreInitialize();
    }
}

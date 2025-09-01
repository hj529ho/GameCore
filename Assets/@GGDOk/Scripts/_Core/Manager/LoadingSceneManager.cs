using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Core.Manager
{
    // [Manager("Scene", "Core")]
    public class LoadingSceneManager : BaseManager
    {
        [System.NonSerialized] public string PreviousScene = "";
        [System.NonSerialized] public string CurrentScene = "";
        private float _time;
        public bool IsRunningLoading = false;
        public override void Init()
        {
            CurrentScene = SceneManager.GetActiveScene().name;
        }
        public override async UniTask InitAsync()
        {
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneName">Name or path of the Scene to load.</param>
        public void LoadScene(string sceneName, bool isAddressable = false)
        {
            
            if (IsRunningLoading)
            {
                Debug.LogError("Load Scene is already running");
                return;
            }
            PreLoadScene();
            PreviousScene = SceneManager.GetActiveScene().name;
            CurrentScene = sceneName;
            // CoroutineHelper.StartCoroutine(LoadAsyncSceneCoroutine(sceneName));
            if (isAddressable)
            {
                LoadAddressableSceneAsync(sceneName).Forget();
            }
            else
            {
                LoadSceneAsync(sceneName).Forget();
            }
        }

        private void PreLoadScene()
        {
            Managers.Pool.Clear();
            // Managers.Game.StopEffectAll();
        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            IsRunningLoading = true;
            Managers.UI.ShowLoadingUI<UI_Loading>();
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            await UniTask.WhenAll(UniTask.WaitUntil(() => operation.progress >= 0.9f), UniTask.WaitForSeconds(1));
            operation.allowSceneActivation = true;

        }
        public async UniTask LoadAddressableSceneAsync(string location)
        {
            IsRunningLoading = true;
            Managers.UI.ShowLoadingUI<UI_Loading>();
            var operation = Addressables.LoadSceneAsync(location, activateOnLoad: false);
            await UniTask.WhenAll(UniTask.WaitUntil(() => operation.IsDone), UniTask.WaitForSeconds(1));
            await operation.Result.ActivateAsync();
        }
    }
}

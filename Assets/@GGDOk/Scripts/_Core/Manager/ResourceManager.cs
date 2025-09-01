using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Core.Manager
{
    // [Manager("Resource","Core")]
    public class ResourceManager : BaseManager
    {
        private readonly Dictionary<string, Object> _global = new();
        private readonly Dictionary<string, Object> _stage = new();
        
        
        public override void Init()
        {
        }

        public override async UniTask InitAsync()
        {
            await UniTask.CompletedTask;
        }
        
        private void LoadAsync<T>(string key,Define.LoadType loadType=Define.LoadType.Stage,Action<T> callback = null) where T : Object
        {
            if (_global.TryGetValue(key, out Object resourceGlobal))
            {
                callback?.Invoke(resourceGlobal as T);
                return;
            }

            if (_stage.TryGetValue(key, out Object resourceStage))
            {
                callback?.Invoke(resourceStage as T);
                return;
            }
            var asynceOperation =  Addressables.LoadAssetAsync<T>(key);
            if (loadType == Define.LoadType.Global)
            {
                asynceOperation.Completed += (handler) =>
                {
                    Debug.Log(key);
                    _global.Add(key, handler.Result);
                    callback?.Invoke(handler.Result);
                };
            }
            else if(loadType == Define.LoadType.Stage)
            {
                asynceOperation.Completed += (handler) =>
                {
                    _stage.Add(key, handler.Result);
                    callback?.Invoke(handler.Result);
                };
            }
        }

        /// <summary>
        /// 같은 라벨의 모든 객체를 로드하는 메서드
        /// </summary>
        /// <param name="label">어드레서블 라벨</param>
        /// <param name="loadType"></param>
        /// <param name="callback">(key, loadcount, totalcount)</param>
        /// <typeparam name="T">Object</typeparam>
        private void LoadAllAsync<T>(string label,Define.LoadType loadType = Define.LoadType.Stage,Action<string, int , int> callback = null) where T : Object
        {
            var asyncOperation =  Addressables.LoadResourceLocationsAsync(label,typeof(T));
            asyncOperation.Completed += (handler) =>
            {
                int loadCount = 0;
                int totalCount = handler.Result.Count;
                if (totalCount == 0)
                {
                    Debug.LogError($"no resources found label : {label}");
                    callback?.Invoke("", 0, 0);
                    return;
                }

                foreach (var result in handler.Result)
                {
                    LoadAsync<T>(result.PrimaryKey,loadType,(obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
            };
        }
        
        
        public async UniTask<T> LoadAsync<T>(string key,Define.LoadType loadType=Define.LoadType.Stage) where T : Object
        {
            if (_global.TryGetValue(key, out Object resourceGlobal))
            {
                return resourceGlobal as T;
            }
            if (_stage.TryGetValue(key, out Object resourceStage))
            {
                return resourceStage as T;
            }
            var taskCompletionSource = new UniTaskCompletionSource<T>();
            LoadAsync<T>(key, loadType, (obj) => taskCompletionSource.TrySetResult(obj));
            var result = await taskCompletionSource.Task;
            return result;
        }
        
        public async UniTask<List<T>> LoadAllAsync<T>(string label, Define.LoadType loadType = Define.LoadType.Stage) where T : Object
        {
            var taskCompletionSource = new UniTaskCompletionSource<List<T>>();
            List<T> list = new List<T>();
            LoadAllAsync<T>(label, loadType, (key, loadCount, totalCount) =>
            {
                if (loadCount == totalCount)
                {
                    taskCompletionSource.TrySetResult(list);
                }
            });
            return await taskCompletionSource.Task;
        }
        
        /// <summary>
        /// 로드라기보단 캐싱된 리소스를 반환하는 메서드
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>(string key) where T : Object
        {
            if (_global.TryGetValue(key, out Object resource))
                return resource as T;
            if (_stage.TryGetValue(key, out Object resourceStage))
                return resourceStage as T;
            Debug.LogError($"Resource Not Loaded: {key}");
            return null;
        }
        
        /// <summary>
        /// 로드라기보단 캐싱된 리소스를 반환하는 메서드
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> LoadAll<T>() where T : Object
        {
            List<T> list = new List<T>();
            
            var globals= _global.Values;
            var stages= _stage.Values;
          
            foreach (var obj in globals)
            {
                if (obj.GetType() == typeof(T))
                {
                    list.Add(obj as T);
                }
            }
            foreach (var obj in stages)
            {
                if (obj.GetType() == typeof(T))
                {
                    list.Add(obj as T);
                }
            }
            return list;
        }
        /// <summary>
        /// 로드라기보단 캐싱된 리소스를 반환하는 메서드
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> LoadAllStaged<T>() where T : Object
        {
            List<T> list = new List<T>();

            var stages = _stage.Values;

            foreach (var obj in stages)
            {
                if (obj.GetType() == typeof(T))
                {
                    list.Add(obj as T);
                }
            }
            return list;
        }

        public void ReleaseStage()
        {
            foreach (var pair in _stage)
            {
                Addressables.Release(pair.Value);
            }
            _stage.Clear();
        }

        public void Release(string key)
        {
            if (_stage.TryGetValue(key, out Object resourceStage))
            {
                Addressables.Release(resourceStage);
                _stage.Remove(key);
            }
        }

        public async UniTask<GameObject> InstantiateWithLoadAsync(string path,Define.LoadType loadType , Transform parent = null, bool pooling = false)
        {
            GameObject prefab = await LoadAsync<GameObject>($"Prefabs/{path}", loadType);
            if(prefab == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }
            if (pooling)
                return Managers.Pool.Pop(prefab);
            GameObject go = Object.Instantiate(prefab,parent);
            go.name = prefab.name;
            return go;
        }

        public GameObject Instantiate(string path,Transform parent = null,bool pooling = false)
        {
            GameObject prefab = Load<GameObject>($"Prefabs/{path}");
            if(prefab == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }
            //Pooling
            if (pooling)
                return Managers.Pool.Pop(prefab);
        
            GameObject go = Object.Instantiate(prefab,parent);
            go.name = prefab.name;
            return go;
        }
        

        public void Destroy(GameObject go)
        {
            if(go ==null)
                return;
            if(Managers.Pool.Push(go))
                return;
            Object.Destroy(go);
        }


        public void RegisterPreloadAssets<T>(string label) where T : UnityEngine.Object
            => Managers.Instance.RegisterPreloadAssets<T>(label);


    }
}
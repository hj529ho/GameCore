using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Core.Manager
{
    // [Manager("Localization","Core")]
    public class LocalizationManager : BaseManager
    {
        private readonly string _defaultLanguage = "ko-KR";
        IEnumerator LoadSetting()
        {
            yield return LocalizationSettings.InitializationOperation;
        }
        
        private async UniTask LoadSettingAsync()
        {
            await LocalizationSettings.InitializationOperation;
        }

        public string GetLocale()
        {
            return LocalizationSettings.SelectedLocale.Identifier.Code;
        }

        void OnException(AsyncOperationHandle handle, Exception exception)
        {
            Debug.Log("Locale Error: " + exception);
            if (exception != null && exception.Message.Contains("SelectedLocale is null"))
            {
                Debug.LogError("LocalizationSettings Force !!!!!!!");
                LoadLocale(_defaultLanguage);
            }
        }


        public override void Init()
        {
            
        }

        public override async UniTask InitAsync()
        {
            UnityEngine.ResourceManagement.ResourceManager.ExceptionHandler += OnException;
            await LoadSettingAsync();
            var taskCompletionSource = new UniTaskCompletionSource<AsyncOperationHandle<LocalizationSettings>>();
            LocalizationSettings.InitializationOperation.Completed += (obj) => { taskCompletionSource.TrySetResult(obj);};
            var obj = await taskCompletionSource.Task;
            Debug.Log("LocalizationSettings.SelectedLocale: " + LocalizationSettings.SelectedLocale);
            var task = new UniTaskCompletionSource<AsyncOperationHandle<Locale>>();
            LocalizationSettings.Instance.GetSelectedLocaleAsync().Completed += (handle) => { task.TrySetResult(handle);};
            var handle = await task.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("LocalizationSettings.SelectedLocale: " + LocalizationSettings.SelectedLocale);
            }
            else
            {
                LoadLocale(_defaultLanguage);
            }
        }
        
        public void Init(Action<bool> callback)
        {
            UnityEngine.ResourceManagement.ResourceManager.ExceptionHandler += OnException;
            LocalizationSettings.InitializationOperation.Completed += (obj) =>
            {
                // Debug.Log("LocalizationSettings.InitializationOperation.Completed");
                Debug.Log("LocalizationSettings.SelectedLocale: " + LocalizationSettings.SelectedLocale);
                LocalizationSettings.Instance.GetSelectedLocaleAsync().Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("LocalizationSettings.SelectedLocale: " + LocalizationSettings.SelectedLocale);
                        callback?.Invoke(true);
                    }
                    else
                    {
                        LoadLocale(_defaultLanguage);
                        callback?.Invoke(true);
                    }
                };
            };
            CoroutineHelper.StartCoroutine(LoadSetting());
        }

        public void Get(string table, string key, Action<string> callback)
        {
            CoroutineHelper.StartCoroutine(GetCoroutine(table, key, callback));
        }

        public string GetString(string key, string table = "CodeTable")
        {
            if (!LocalizationSettings.SelectedLocale)
            {
                LoadLocale(_defaultLanguage);
            }

            Locale currentLanguage = LocalizationSettings.SelectedLocale; 
            string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(table, key, currentLanguage);
            if (localizedString.Contains("No translation found for"))
            {
                return key;
            }
            return localizedString;
        }

        public void GetSpriteAsync<T>(string key, string table, Action<Sprite> callback) where T : Object
        {
            CoroutineHelper.StartCoroutine(GetSpriteCoroutine(key, table, callback));
        }
        IEnumerator GetSpriteCoroutine(string key, string table, Action<Sprite> callback)
        {
            Locale currentLanguage = LocalizationSettings.SelectedLocale; 
            var result = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<Sprite>(table, key);
            yield return new WaitUntil(() => result.IsDone);
            if (result.Status == AsyncOperationStatus.Succeeded)
            {
                callback.Invoke(result.Result);
            }
            else
            {
                Debug.LogError("No Sprite Found : " + key);
            }
        }

        public void GetAsset<T>(string table, string key, Action<string,T> callback)where T : Object
        {
            CoroutineHelper.StartCoroutine(LoadLocalizedAsset<T>(table, key, callback));
        }

        public string Get(string tableName, string keyName) {
            LocalizedString localizeString = new LocalizedString() { TableReference = tableName, TableEntryReference = keyName};
            var stringOperation = localizeString.GetLocalizedStringAsync();
    
            if (stringOperation.IsDone && stringOperation.Status == AsyncOperationStatus.Succeeded) {
                return stringOperation.Result;
            } else {
                return null;
            }
        }
        IEnumerator GetCoroutine(string tableName, string keyName,Action<string> callback)
        {
            var stringOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableName, keyName);
            yield return new WaitUntil(() => stringOperation.IsDone);
            if (stringOperation.IsDone && stringOperation.Status == AsyncOperationStatus.Succeeded) {
                callback.Invoke(stringOperation.Result);
            }
            else
            {
                callback.Invoke(keyName);
            }
        }

        public void LoadAllLocalizedAsset<T>(string table, Action<Dictionary<string, T>> callback) where T : Object
        {
            var AssetTable =  LocalizationSettings.AssetDatabase.GetTable(table);
            var values = AssetTable.Values;
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            int loaded = 0;
            for (int i =0; i<values.Count; i++ )
            {
                var VARIABLE = values.ElementAt(i);
                GetAsset<T>(table,VARIABLE.Key, (key, value) =>
                {
                    loaded++;
                    dictionary.Add(key,value);
                    if (values.Count == loaded)
                    {
                        callback.Invoke(dictionary);   
                    }
                });
            }
        }

        public IEnumerator LoadLocalizedAsset<T>(string table, string key, Action<string,T> callback) where T : Object
        {
            LocalizedAsset<T> localizedAsset = new LocalizedAsset<T>(){TableReference = table,TableEntryReference = key};
            var operation = localizedAsset.LoadAssetAsync();
            while (!operation.IsDone)
            {
                yield return null;
            }

            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                callback.Invoke(key,operation.Result as T);
            }
            else
            {
                Debug.LogError(operation.Status);
            }
        }

        public void LoadLocale(string languageIdentifier)
        {
            LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);
            foreach (var aLocale in LocalizationSettings.AvailableLocales.Locales)
            {
                LocaleIdentifier anIdentifier = aLocale.Identifier;
                if(anIdentifier == localeCode) {
                    LocalizationSettings.SelectedLocale = aLocale;
                }
            }
        }
    }
}

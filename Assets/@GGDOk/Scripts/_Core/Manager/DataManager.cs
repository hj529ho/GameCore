
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Manager
{
    // [Manager("Data","Core")]
    public class DataManager : BaseManager
    {
        public Dictionary<string, string> KV { get; private set; } = new();
        public override void Init() { }
        public override async UniTask InitAsync()
        {
            await LoadAsync();
        }
        public void Save(string key, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            KV[key] = json;
            SaveAsync().Forget();
        }
        public void Save() => SaveAsync().Forget();
        
        private async UniTask SaveAsync()
        {
            // 서버는 { data: <dict> } 형태를 요구
            var body = new { data = KV };
            //TODO body 시리얼라이즈해서 저장
            await UniTask.CompletedTask; 
        }
        private async UniTask LoadAsync()
        {
            //데이터 로드해서 KV에 집어넣기
            await UniTask.CompletedTask; 
        }
        public T Load<T>(string key) where T : class, new()
        {
            if (KV != null && KV.TryGetValue(key, out var jsonFromKv) && !string.IsNullOrWhiteSpace(jsonFromKv))
            {
                var ok = TryDeserialize<T>(jsonFromKv, out var fromKv);
                if (ok) return fromKv;

                // 손상됐으면 즉시 정리
                Debug.LogWarning($"[Data] KV corrupted for {key}. Resetting.");
                KV.Remove(key);
                SaveAsync().Forget(); // 서버 스냅샷도 정리 반영(비동기)
                return new T();               // 안전 기본값
            }
            return new T();
        }
        private static bool TryDeserialize<T>(string json, out T obj) where T : class
        {
            try
            {
                obj = JsonConvert.DeserializeObject<T>(json);
                return obj != null;
            }
            catch (Exception)
            {
                obj = null;
                return false;
            }
        }
    }
}
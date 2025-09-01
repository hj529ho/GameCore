using System.Text;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Manager
{
    public class NetworkManager : BaseManager
    {
        private const string BaseURL = "";

        public override void Init() { }
        
        public override async UniTask InitAsync() { await UniTask.CompletedTask; }

        public static async UniTask<string> Get(string uri)
        {
            UnityWebRequest www = UnityWebRequest.Get(BaseURL + uri);
            await www.SendWebRequest();
            if(www.result != UnityWebRequest.Result.Success)
                Debug.LogError(www.error);
            return www.downloadHandler.text;
        }

        public static async UniTask<string> Post(string uri, string data)
        {
            var www = new UnityWebRequest(BaseURL + uri, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            await www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log(www.error);
            return www.downloadHandler.text;
        }
    }
}
using System.Collections;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static MonoBehaviour monoInstance;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Initializer()
    {
        Debug.Log("coroutine helper init");
        monoInstance = new GameObject(
            $"[{nameof(CoroutineHelper)}]"
        ).AddComponent<CoroutineHelper>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }
}

using System.Collections;
using UnityEngine;

namespace Core.Manager
{
    // [Manager("Sound","Core")]
    public class SoundManager
    {
        //AudioSource

        //AudioClip

        //AudioListener\
        private readonly AudioSource[] _audioSource = new AudioSource[(int)Define.Sound.MaxCOUNT];
        public void Init()
        {
            GameObject root = GameObject.Find("@Sound");
            if(root == null)
            {
                root = new GameObject("@Sound");
                Object.DontDestroyOnLoad(root);
                string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
                for(int i = 0; i <soundNames.Length-1;i++)
                {
                    GameObject go = new GameObject(soundNames[i]);
                    _audioSource[i] = go.AddComponent<AudioSource>();
                    go.transform.parent = root.transform;
                }
                _audioSource[(int)Define.Sound.BGM].loop = true;
                Debug.Log("SoundManager Init");
            }

        }

        public void Play(string path,Define.Sound type=Define.Sound.SFX, float pitch = 1.0f)
        {
            if(path.Contains("Sounds/")== false)
            {
                path = $"Sounds/{path}";
            }
            if(type == Define.Sound.BGM)
            {
                AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
                if(audioClip == null)
                {
                    Debug.Log($"AudioClip Missing! {path}");
                    return;
                }
                AudioSource audioSource = _audioSource[(int)Define.Sound.BGM];
                if(audioSource.isPlaying)
                    audioSource.Stop();
                //TODO 설정창에서 저장한 볼륨 가져오기
                audioSource.volume = 1;
                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
                if(audioClip == null)
                {
                    Debug.Log($"AudioClip Missing! {path}");
                    return;
                }
                AudioSource audioSource = _audioSource[(int)Define.Sound.SFX];
                //TODO 설정창에서 저장한 볼륨 가져오기
                audioSource.volume = 1;
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip); 
            }
        }
        public void BGM_Fadeout(float duration = 1)
        {
            CoroutineHelper.StartCoroutine(BGM_FadeoutCoroutine(duration));
        }
    
        IEnumerator BGM_FadeoutCoroutine(float duration)
        {
            AudioSource audioSource = _audioSource[(int)Define.Sound.BGM];
            float volume = 1;
            while(audioSource.volume > 0)
            {
                volume -= 0.01f;
                audioSource.volume = volume;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}

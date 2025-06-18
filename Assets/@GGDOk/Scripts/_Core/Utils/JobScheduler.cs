using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;


namespace HaMa.Scripts._Core.Utils
{
    public class JobScheduler : MonoBehaviour
    {
        private static JobScheduler _instance;
        private static bool _init = false;
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initializer()
        {
            Debug.Log("JobScheduler init");
            _instance = new GameObject(
                $"[{nameof(JobScheduler)}]"
            ).AddComponent<JobScheduler>();
            DontDestroyOnLoad(_instance.gameObject);
            _init = true;
        }
        private Dictionary<JobToken,ICustomJob> _jobs = new Dictionary<JobToken, ICustomJob>();

        
        /// <summary>
        /// Job을 시작합니다. cooldown 시간이 지날때마다 job이 실행됩니다. 전역적으로 실행되므로 Monobehaviour를 상속받은 클래스에서 사용할때는 주의합니다.
        /// 그 외의 클래스에서도 객체 소멸 시점을 주의합니다.
        /// </summary>
        /// <param name="job">Execute() 메소드가 정의된 객체</param>
        /// <param name="cooldown"> 반복 실행 대기 시간</param>
        /// <typeparam name="T"> ICustomJob interface</typeparam>
        /// <returns>JobToken</returns>
        public static JobToken StartJob<T>(T job,float cooldown) where T : ICustomJob
        {
            var token = new JobToken(_instance._jobs.Count,cooldown);
            _instance._jobs.Add(token,job);
            return token;
        }
        
        public static void StopJob(JobToken job)
        {
            _instance._jobs.Remove(job, out var customJob);
        }

        private void Update()
        {
            if(_init == false)
                return;
            foreach (var pair in _instance._jobs)
            {
                pair.Key.Time -= Time.deltaTime;
                if (pair.Key.Time <= 0)
                {
                    pair.Key.Time += pair.Key.Cooldown;
                    pair.Value.Execute();
                }
            }
        }
    }
}
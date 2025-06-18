
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Manager
{
    /// <summary>
    /// 간단한 메세지 버스
    /// 추후 성능 및 고급 기능 필요시 MessagePipe로 마이그레이션
    /// </summary>
    // [Manager("Event", "Core")]
    public class EventManager
    {
        private readonly Dictionary<Type, EventHandler> _handlers = new ();

        private EventHandler<T> GetHandler<T>() where T : class
        {
            if (!_handlers.TryGetValue(typeof(T), out var handler) || handler is not EventHandler<T> handlerT)
            {
                handlerT = new EventHandler<T>();
                _handlers[typeof(T)] = handlerT;
            }
            return handlerT;
        }

        #region Public methods
        /// <returns>마지막으로 전송된 파라미터</returns>
        public T GetCache<T>() where T : class
        {
            return GetHandler<T>().CachedParam;
        }

        /// <summary>
        /// 이벤트 구독
        /// </summary>
        /// <typeparam name="T">이벤트 타입</typeparam>
        /// <param name="observer">리스너</param>
        /// <param name="notify">구독시점에 캐싱된 값이 존재하면 OnNotify를 호출할지 여부</param>
        public void Subscribe<T>(IObserver<T> observer, bool notify = false) where T : class
        {
            GetHandler<T>().observers.Add(observer);
        }
        /// <summary>
        /// 이벤트 구독 해제
        /// </summary>
        /// <typeparam name="T">이벤트 타입</typeparam>
        /// <param name="observer">리스너</param>
        public void Unsubscribe<T>(IObserver<T> observer) where T : class
        {
            Debug.Log($"ss Unsubscribe {typeof(T).Name}");
            GetHandler<T>().observers.Remove(observer);
        }

        /// <summary>
        /// 해당 이벤트에 존재하는 리스너 모두 해제
        /// </summary>
        /// <typeparam name="T">이벤트 타입</typeparam>
        /// <param name="observer">리스너</param>
        public void UnsubscribeAll<T>() where T : class
        {
            GetHandler<T>().Clear();
        }
        public void UnsubscribeAllEvents()
        {
            foreach (var handler in _handlers.Values)
            {
                handler.Clear();
            }
        }
        
        public void Notify<T>(T param) where T : class
        {
            GetHandler<T>().NotifyT(param);
        }
        #endregion

        private abstract class EventHandler
        {
            public abstract void Clear();
            public abstract void Notify(object param);
        }
        private class EventHandler<T> : EventHandler where T : class
        {
            public readonly List<IObserver<T>> observers = new();
            public T CachedParam
            {
                get;
                private set;
            }

            public override void Clear()
            {
                observers.Clear();
            }
            public override void Notify(object param)
            {
                if (param is not T paramT)
                {
                    Debug.LogWarning($"Invalid parameter type. Expect \"${typeof(T).FullName}\", Received \"${param.GetType().FullName}\"");
                    return;
                }
                NotifyT(paramT);
            }
            public void NotifyT(T param)
            {
                CachedParam = param;
                foreach (var observer in observers)
                {
                    try
                    {
                        observer.OnNotify(param);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }
    }
}
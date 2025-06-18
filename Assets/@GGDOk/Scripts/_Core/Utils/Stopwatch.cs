using UnityEngine;

namespace Game.Utils
{
    public interface IStopwatch
    {
        float Elapsed { get; }
        bool IsPaused { get; }

        void Start();
        void Pause();
        void Resume();
    }
    /// <summary>
    /// Wrapper of Unity Time and provide Time.time(or Time.unscaledTime)
    /// </summary>
    public class RootStopwatch : IStopwatch
    {
        private readonly bool scaled;
        public bool Scaled => scaled;

        public float Elapsed => Scaled ? Time.time : Time.unscaledTime;
        public bool IsPaused => false;

        public RootStopwatch(bool scaled = true)
        {
            this.scaled = scaled;
        }

        #region Not implemented
        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Start()
        {

        }
        #endregion Not implemented
    }

    public class Stopwatch : IStopwatch
    {
        readonly IStopwatch parent;

        public float BeginAt { get; private set; }
        public float? PauseAt { get; private set; }
        bool isPaused;
        public bool IsPausedSelf => isPaused;
        public bool IsPaused => isPaused || parent.IsPaused;

        public float Elapsed
        {
            get
            {
                if (PauseAt.HasValue)
                {
                    var pauseSince = PauseAt.Value;
                    return pauseSince - BeginAt;
                }
                return parent.Elapsed - BeginAt;
            }
        }

        public Stopwatch(IStopwatch parent = null)
        {
            this.parent = parent ?? new RootStopwatch();
            Start();
        }

        public void Start()
        {
            BeginAt = parent.Elapsed;
            PauseAt = null;
            isPaused = false;
        }
        public void Pause()
        {
            if (PauseAt.HasValue)
            {
                return;
            }
            PauseAt = parent.Elapsed;
            isPaused = true;
        }
        public void Resume()
        {
            if (!PauseAt.HasValue)
            {
                return;
            }
            var pausedTime = parent.Elapsed - PauseAt.Value;
            BeginAt += pausedTime;
            PauseAt = null;
            isPaused = false;
        }
    }
}
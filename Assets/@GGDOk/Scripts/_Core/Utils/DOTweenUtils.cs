using DG.Tweening;
using UnityEngine;

namespace Core.Utils
{
    public static class DOTweenUtils
    {
        public static void SafeKill(this Tween t, bool complete = false)
        {
            if (t == null || !t.IsActive()) return;
            t.Kill(complete);
        }
        public static void SafeComplete(this Tween t, bool complete = false)
        {
            if (t == null || !t.IsActive() || !t.IsPlaying()) return;
            t.Complete(complete);
        }
    }
}
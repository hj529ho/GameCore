using UnityEngine;

namespace Core.Utils
{
    public static class FloatUtils
    {
        public static float MoveTowards(this float previous, float target, float speed)
        {
            if (previous < target)
            {
                var next = previous + speed;
                if (next > target)
                {
                    return target;
                }
                return next;
            } else
            {
                var next = previous - speed;
                if (next < target)
                {
                    return target;
                }
                return next;
            }
        }
    }
}
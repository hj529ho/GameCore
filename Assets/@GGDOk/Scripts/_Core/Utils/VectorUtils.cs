using UnityEngine;

namespace Core.Utils
{
    public static class VectorUtils
    {
        public static Vector2 Rotate(this Vector2 v, float radians)
        {
            var cos = Mathf.Cos(radians);
            var sin = Mathf.Sin(radians);
            return new Vector2(
                v.x * cos - v.y * sin,
                v.x * sin + v.y * cos
            );
        }
        public static Vector3 Rotate(this Vector3 v, float degrees, Vector3 axis)
        {
            return Quaternion.AngleAxis(degrees, axis) * v;
        }

        public static Vector2 SquareNormalize(this Vector2 vector)
        {
            var maxValue = System.Math.Max(System.Math.Abs(vector.x), System.Math.Abs(vector.y));
            return vector / maxValue;
        }
        // https://stackoverflow.com/a/69330927
        public static Vector3 SquareNormalize(this Vector3 vector)
        {
            var bounds = new Bounds(Vector3.zero, Vector3.one * 2f);
            return bounds.ClosestPoint(vector);
        }

        public static Vector2 Bezier(this Vector2 start, Vector2 middle, Vector2 end, float t)
        {
            var p1 = Vector2.Lerp(start, middle, t);
            var p2 = Vector2.Lerp(middle, end, t);
            return Vector2.Lerp(p1, p2, t);
        }
        public static Vector3 Bezier(this Vector3 start, Vector3 middle, Vector3 end, float t)
        {
            var p1 = Vector3.Lerp(start, middle, t);
            var p2 = Vector3.Lerp(middle, end, t);
            return Vector3.Lerp(p1, p2, t);
        }
        
        public static Vector2 Bezier(this Vector2 start, Vector2 middle1, Vector2 middle2, Vector2 end, float t)
        {
            var p1 = Vector2.Lerp(start, middle1, t);
            var p2 = Vector2.Lerp(middle1, middle2, t);
            var p3 = Vector2.Lerp(middle2, end, t);
            return p1.Bezier(p2, p3, t);
        }
        public static Vector3 Bezier(this Vector3 start, Vector3 middle1, Vector3 middle2, Vector3 end, float t)
        {
            var p1 = Vector3.Lerp(start, middle1, t);
            var p2 = Vector3.Lerp(middle1, middle2, t);
            var p3 = Vector3.Lerp(middle2, end, t);
            return p1.Bezier(p2, p3, t);
        }
    }
}
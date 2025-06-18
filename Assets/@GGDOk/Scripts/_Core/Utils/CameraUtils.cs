using UnityEngine;

namespace Core.Utils {
    public static class CameraUtils
    {
        /// <summary>
        /// Orthographics일때 렌더링되는 월드 기준 크기 흭득
        /// </summary>
        public static Vector2 GetHalfSize(this Camera camera)
        {
            var height = camera.orthographicSize;
            var width = height * camera.aspect;
            return new Vector2(width, height);
        }
    }
}
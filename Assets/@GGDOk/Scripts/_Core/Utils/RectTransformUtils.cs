using UnityEngine;

namespace Core.Utils
{
    public static class RectTransformUtils
    {
        public static void SetAnchoredY(this RectTransform @this, float y)
        {
            var pos = @this.anchoredPosition;
            pos.y = y;
            @this.anchoredPosition = pos;
        }
        public static float GetTopWorld(this RectTransform @this)
        {
            Vector3[] corners = new Vector3[4];
            @this.GetWorldCorners(corners);
            return corners[1].y; // Top-left corner Y position
        }
        public static float GetBottomWorld(this RectTransform @this)
        {
            Vector3[] corners = new Vector3[4];
            @this.GetWorldCorners(corners);
            return corners[0].y; // Bottom-left corner Y position
        }
        public static Vector2 GetRandomPoint(this RectTransform @this, Space space = Space.World)
        {
            Vector3[] corners = new Vector3[4];
            switch (space)
            {
                case Space.Self:
                    @this.GetLocalCorners(corners);
                    break;
                case Space.World:
                    @this.GetWorldCorners(corners);
                    break;
            }
            
            Vector2 bottomLeft = corners[0];
            Vector2 topRight = corners[2];
            Debug.Log(bottomLeft);
            Debug.Log(topRight);

            float x = Random.Range(bottomLeft.x, topRight.x);
            float y = Random.Range(bottomLeft.y, topRight.y);

            return new Vector2(x, y) + (space == Space.Self ? @this.anchoredPosition : @this.position);
        }
    }
}
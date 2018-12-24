using UnityEngine;

/*
Reference: https://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/
*/
namespace Scripts
{
    public static class RectUtil
    {
        private static Texture2D _whiteTexture;

        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static void DrawScreenRect(Rect rect, Color color)
        {
            var tempColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = color;
            UnityEngine.GUI.DrawTexture(rect, WhiteTexture);
            UnityEngine.GUI.color = tempColor;
        }

        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            // Top
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Left
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            // Bottom
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Rect GetScreenRect(Vector3 p1, Vector3 p2)
        {
            p1.y = Screen.height - p1.y;
            p2.y = Screen.height - p2.y;
            var topLeft = Vector3.Min(p1, p2);
            var bottomRight = Vector3.Max(p1, p2);
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        public static Bounds GetViewportBound(Camera camera, Vector3 p1, Vector3 p2)
        {
            var v1 = Camera.main.ScreenToViewportPoint(p1);
            var v2 = Camera.main.ScreenToViewportPoint(p2);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = camera.nearClipPlane;
            max.z = camera.farClipPlane;
            var bound = new Bounds();
            bound.SetMinMax(min, max);
            return bound;
        }
    }
}
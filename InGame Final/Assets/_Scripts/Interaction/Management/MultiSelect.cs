using UnityEngine;

namespace _Scripts.Interaction.Management
{
    public static class MultiSelect
    {
        private static Texture2D _whiteTexture;

        private static Texture2D WhiteTexture
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

        public static void DrawScreenRectangle(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
        }

        public static void DrawScreenRectangleBorder(Rect rect, float thickness, Color color)
        {
            // Top
            MultiSelect.DrawScreenRectangle(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Left
            MultiSelect.DrawScreenRectangle(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            MultiSelect.DrawScreenRectangle(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            // Bottom
            MultiSelect.DrawScreenRectangle(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Rect GetScreenRectangle(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;

            Vector3 bR = Vector3.Max(screenPosition1, screenPosition2);
            Vector3 tL = Vector3.Min(screenPosition1, screenPosition2);

            return Rect.MinMaxRect(tL.x, tL.y, bR.x, bR.y);
        }

        public static Bounds GetViewportBounds(Camera cam, Vector3 screenPosition1, Vector3 screenPosition2)
        {
            Vector3 pos1 = cam.ScreenToViewportPoint(screenPosition1);
            Vector3 pos2 = cam.ScreenToViewportPoint(screenPosition2);

            Vector3 min = Vector3.Min(pos1, pos2);
            Vector3 max = Vector3.Max(pos1, pos2);

            min.z = cam.nearClipPlane;
            max.z = cam.farClipPlane;

            Bounds bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }
    }
}

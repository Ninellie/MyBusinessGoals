using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash
{
    class CommonMethods
    {
        public static readonly Rect UVAbsolute = new Rect(0f, 0f, 1f, 1f);
        internal static T EnsureComponent<T>(GameObject obj,bool hide = false) where T : Component
        {
            T comp = obj.GetComponent<T>();
            if (comp == null)
                comp = obj.AddComponent<T>();
            if (Application.isPlaying == false && Application.isEditor == true)
            {
              //  comp.tag = "EditorOnly";
                comp.hideFlags = HideFlags.DontSaveInEditor;
            }
            return comp;
        }

        public static void HideObject(GameObject obj)
        {
            if (Application.isPlaying == false && Application.isEditor == true)
            {
                obj.tag = "EditorOnly";
                obj.hideFlags = HideFlags.DontSaveInEditor;
            }
            obj.hideFlags = obj.hideFlags | HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
        
        public static void SafeDestroy( UnityEngine.Object obj)
        {
            if (obj == null)
                return;
            if (Application.isEditor && Application.isPlaying == false)
                UnityEngine.Object.DestroyImmediate(obj);
            else
                UnityEngine.Object.Destroy(obj);
            obj = null;

        }
        public static Rect HorizontalTextureTile(float amount)
        {
            return new Rect(0f, 0f, amount, 1f);
        }
        public static Rect VerticalTextureTile(float amount)
        {
            return new Rect(0f, 0f,1f, amount);
        }

        public static float InterpolateInRectX(float v,Rect r)
        {
            return r.x + v * r.width;
        }
        public static float InterpolateInRectY(float v, Rect r)
        {
            return r.y + v * r.height;
        }

        public static Vector2 InterpolateInRect(Vector2 v,Rect r)
        {
            return new Vector2(r.x + v.x * r.width, r.y + v.y * r.height);
        }
        public static void DrawVertical(float x, Rect bounds, float thickness, Rect uv, Color color, VertexHelper vh)
        {
            Rect r = new Rect(x-thickness*0.5f, bounds.y, thickness,bounds.height);
            DrawRect(r, uv, color, vh);
        }

        public static void DrawHorizontal(float y,Rect bounds,float thickness,Rect uv, Color color,VertexHelper vh)
        {
            Rect r = new Rect(bounds.x, y - thickness * 0.5f, bounds.width, thickness);
            DrawRect(r, uv, color, vh);
        }
        public static void DrawRect(Rect rect,Rect uv,Color color,VertexHelper vh)
        {
            int index = vh.currentVertCount;
            vh.AddVert(new Vector3(rect.xMin, rect.yMin, 0f), color, new Vector2(uv.xMin, uv.yMin));
            vh.AddVert(new Vector3(rect.xMin, rect.yMax, 0f), color, new Vector2(uv.xMin, uv.yMax));
            vh.AddVert(new Vector3(rect.xMax, rect.yMax, 0f), color, new Vector2(uv.xMax, uv.yMax));
            vh.AddVert(new Vector3(rect.xMax, rect.yMin, 0f), color, new Vector2(uv.xMax, uv.yMin));
            vh.AddTriangle(index, index+ 1, index+2);
            vh.AddTriangle(index+2, index+3, index);
        }
        public static bool IsRangeSelected(DateTime from, DateTime to, HashSet<DateTime> selection)
        {
            from = from.Date;
            to = to.Date;
            if (from == to)
            {
                return selection.Count == 1 && selection.Contains(from);
            }
            if (from > to)
            {
                DateTime tmp = from;
                from = to;
                to = tmp;
            }

            DateTime iterator = from;
            int count = 0;
            while (iterator <= to)
            {
                if (selection.Contains(iterator.Date) == false)
                    return false;
                count++;
                iterator += TimeSpan.FromDays(1);
            }
            if (selection.Count != count)
                return false;
            return true;
        }
        public static void SelectRange(DateTime from, DateTime to, HashSet<DateTime> selection)
        {
            selection.Clear();
            from = from.Date;
            to = to.Date;
            if (from == to)
            {
                selection.Add(from);
                return;
            }
            if (from > to)
            {
                DateTime tmp = from;
                from = to;
                to = tmp;
            }

            DateTime iterator = from;
            while (iterator <= to)
            {
                selection.Add(iterator.Date);
                iterator += TimeSpan.FromDays(1);
            }
        }

    }
}

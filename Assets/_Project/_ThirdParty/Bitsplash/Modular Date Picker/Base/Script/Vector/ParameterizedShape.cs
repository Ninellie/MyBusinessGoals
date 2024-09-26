using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.Vector
{
    public class ParameterizedShape : MaskableGraphic
    {
        public Sprite sprite;
        public int EdgeCount=3;
        public float EdgeRoundingOffset;
        public int EdgeRoundingSegments;
        public float Rotation = 0;


        // Update is called once per frame
        void Update()
        {

        }
        public override Texture mainTexture
        {
            get
            {
                if (sprite == null)
                    return base.mainTexture;
                return sprite.texture;
            }
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (EdgeCount < 3)
                return;
            var rect = GetPixelAdjustedRect();
            float radius = Mathf.Min(rect.width, rect.height) * 0.5f;
            var list = CommonVectors.mTmpList;
            list.Clear();
            CommonVectors.NPolygon(EdgeCount, radius, radius, list);
            if (EdgeRoundingOffset > 0f)
            {
                var smoothList = CommonVectors.mTmpSmoothList;
                smoothList.Clear();
                CommonVectors.SmoothCorners(list, EdgeRoundingOffset, EdgeRoundingSegments, smoothList);
                list = smoothList;
            }
            Matrix4x4 m = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, Rotation));
            for (int i = 0; i < list.Count; i++)
                list[i] = m*list[i];
            Vector2 centroid = new Vector2();
            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;
            for(int i=0; i<list.Count; i++)
            {
                var v = list[i];
                centroid += v;
                minX = Mathf.Min(minX, v.x);
                minY = Mathf.Min(minY, v.y);
                maxX = Mathf.Max(maxX, v.x);
                maxY = Mathf.Max(maxY, v.y);
            }
            Vector2 minV = new Vector2(minX, minY);
            Vector2 sizeV = new Vector2(maxX - minX, maxY - minY);
            centroid *= 1f / list.Count;
            vh.Clear();
            
            Vector2 add = rect.center - centroid;
            vh.AddVert(rect.center, color, CommonVectors.InterpolateUV(centroid, minV, sizeV));
            for (int i=0; i<list.Count; i++)
            {
                vh.AddTriangle(0, i+1, ((i+1) % list.Count) +1);
                vh.AddVert(list[i] + add, color, CommonVectors.InterpolateUV(list[i], minV, sizeV));
                
            }
        }
    }
}

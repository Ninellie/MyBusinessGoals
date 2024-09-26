using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.Vector
{
    class CommonVectors
    {
        public static List<Vector2> mTmpList = new List<Vector2>();
        public static List<Vector2> mTmpSmoothList = new List<Vector2>();
        public static Vector2 LineInsersection(Vector2 a1,Vector2 a2,Vector2 b1,Vector2 b2)
        {
            float A1 = a2.y - a1.y;
            float B1 = a1.x - a2.x;
            float C1 = A1 * a1.x + B1 * a1.y;

            float A2 = b2.y - b1.y;
            float B2 = b1.x - b2.x;
            float C2 = A2 * b1.x + B2 * b1.y;

            float delta = A1 * B2 - A2 * B1;

            float x = (B2 * C1 - B1 * C2) / delta;
            float y = (A1 * C2 - A2 * C1) / delta;
            return new Vector2(x, y);
        }
        static Vector2 Orthogonal(Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }
        static Vector2 FromPolar(float angle,float radius)
        {
            return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        }
        public static Vector2 InterpolateUV(Vector2 v,Vector2 min,Vector2 size)
        {
            return new Vector2((v.x - min.x) / size.x, (v.y - min.y) / size.y);
        }
        public static void NPolygon(int edgeCount,float radius,float alternateRadius,List<Vector2> res)
        {
            float startAngle = 90 - (180f / (float)edgeCount);
            for(int i=0; i<edgeCount; i++)
            {
                float angle = startAngle + ((float)i) / ((float)edgeCount) * 360;
                res.Add(FromPolar(angle * Mathf.Deg2Rad, radius));
            }
        }
        public static void SmoothCorners(List<Vector2> polygon,float offset,float segments, List<Vector2> res)
        {
            for(int i=0; i<polygon.Count; i++)
            {
                Vector2 p1 = polygon[i];
                Vector2 p2 = polygon[(i+1)%polygon.Count];
                Vector2 p3 = polygon[(i+2)% polygon.Count];

                Vector2 s1Dir = (p2 - p1).normalized;
                Vector2 s2Dir = (p3 - p2).normalized;
                Vector2 arcStart = p2 - s1Dir * offset;
                Vector2 arcEnd = p2 + s2Dir * offset;

                Vector2 arcCenter = LineInsersection(arcStart, arcStart + Orthogonal(s1Dir),arcEnd ,arcEnd+ Orthogonal(s2Dir));
                float startAngle = Mathf.Atan2(arcStart.y - arcCenter.y, arcStart.x - arcCenter.x);
                float endAngle = Mathf.Atan2(arcEnd.y - arcCenter.y, arcEnd.x - arcCenter.x);
                if (endAngle < startAngle)
                    startAngle -= Mathf.PI * 2f;
                float radius = (arcStart - arcCenter).magnitude;
                for (int j=0; j<=segments; j++)
                {
                    float factor = ((float)j) / ((float)segments);
                    float angle = Mathf.Lerp(startAngle, endAngle, factor);
                    res.Add(arcCenter + FromPolar(angle, radius));
                }

            }
        }
    }
}

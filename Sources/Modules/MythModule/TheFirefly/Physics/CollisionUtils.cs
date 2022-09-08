using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Physics
{
    internal class CollisionUtils
    {
        public static float CrossProduct(Vector2 A, Vector2 B)
        {
            return A.X * B.Y - A.Y * B.X;
        }

        public static bool PointOnLine(Vector2 A, Vector2 d, Vector2 C)
        {
            return Math.Abs(CrossProduct(d, C - A)) < 1e-2;
        }
        public static bool PointOnSegment(Vector2 A, Vector2 B, Vector2 C)
        {
            return Math.Abs(CrossProduct(B - A, C - A)) < 1e-2
                && Vector2.Dot(C - A, B - A) * Vector2.Dot(C - B, A - B) >= 0;
        }

        private static bool IsInValidTime(float t)
        {
            return t < 0 || float.IsNaN(t) || float.IsInfinity(t);
        }

        /// <summary>
        /// 获取AB两动点形成线段和C点的连续碰撞结果，并返回时间
        /// </summary>
        /// <param name="A"></param>
        /// <param name="vA"></param>
        /// <param name="B"></param>
        /// <param name="vB"></param>
        /// <param name="C"></param>
        /// <param name="vC"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool CCD_SegmentPoint(Vector2 A, Vector2 vA, Vector2 B, Vector2 vB, 
            Vector2 C, Vector2 vC, out float t)
        {
            Vector2 E0 = B - A;
            Vector2 E1 = vB - vA;
            Vector2 E2 = C - A;
            Vector2 E3 = vC - vA;


            //Vector2 a1 = E0.X * E2.Y + E0.X * E3.Y * t + E1.X * t * E2.Y + E1.X * E3.Y * t * t;
            //Vector2 a2 = E0.Y * E2.X + E0.Y * E3.X * t + E1.Y * t * E2.X + E1.Y * E3.X * t * t;

            double a = E1.X * E3.Y - E1.Y * E3.X;
            double b = E0.X * E3.Y - E0.Y * E3.X + E1.X * E2.Y - E1.Y * E2.X;
            double c = E0.X * E2.Y - E0.Y * E2.X;

            if (Math.Abs(a) < 1e-4)
            {
                t = (float)(-c / b);
                if (IsInValidTime(t))
                    return false;
                return PointOnSegment(A + vA * t, B + vB * t, C + vC * t);
            }

            double det = b * b - 4 * a * c;

            t = 0;

            if (det < 0)
            {
                return false;
            }

            double detSqrt = Math.Sqrt(det);
            double q;
            if (b < 0)
                q = -.5 * (b - detSqrt);
            else
                q = -.5 * (b + detSqrt);

            float t1 = (float)(q / a);
            float t2 = (float)(c / q);

            if (IsInValidTime(t1) && IsInValidTime(t2))
            {
                return false;
            }
            else if (t1 >= 0 && IsInValidTime(t2))
            {
                t = t1;
                return PointOnSegment(A + vA * t, B + vB * t, C + vC * t);
            }
            else if(IsInValidTime(t1) && t2 >= 0)
            {
                t = t2;
                return PointOnSegment(A + vA * t, B + vB * t, C + vC * t);
            }
            else if (t1 >= 0 && t2 >= 0)
            {
                if (t1 > t2)
                {
                    float tmp = t1;
                    t1 = t2;
                    t2 = tmp;
                }
                if (PointOnSegment(A + vA * t1, B + vB * t1, C + vC * t1))
                {
                    t = t1;
                    return true;
                }
                t = t2;
                return PointOnSegment(A + vA * t, B + vB * t, C + vC * t);
            }
            return false;
        }
    }
}

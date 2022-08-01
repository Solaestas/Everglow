using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core
{
    internal static class MathUtils
    {
        public static float Approach(float val, float target, float maxMove)
        {
            if (val <= target)
            {
                return Math.Min(val + maxMove, target);
            }
            return Math.Max(val - maxMove, target);
        }
        public static Vector2 Approach(Vector2 val, Vector2 target, float maxMove)
        {
            if (val.Distance(target) < maxMove)
            {
                return target;
            }
            return val + (target - val).NormalizeSafe() * maxMove;
        }
        /// <summary>
        /// :[)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Wrap(float from, float to, float t)
        {
            while (t >= to)
            {
                t -= to - from;
            }

            while (t < from)
            {
                t += to - from;
            }

            return t;
        }
        /// <summary>
        /// 将<paramref name="vector"/>投影到<paramref name="axis"/>上的长度
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Projection(Vector2 axis, Vector2 vector)
        {
            return Vector2.Dot(axis, vector) / axis.Length();
        }
        public static float Clamp(float from, float to, float t)
        {
            if (t < from)
            {
                return from;
            }
            else if (t > to)
            {
                return to;
            }

            return t;
        }
        public static float Step(float from, float to)
        {
            return from <= to ? 1 : 0;
        }
        public static float SmoothStep(float from, float to, float t)
        {
            t = Clamp(0, 1, (t - from) / (to - from));
            return t * t * (3 - 2 * t);
        }
        public static float SmoothStepMinus(float from, float to, float t, float width)
        {
            return SmoothStep(from, from + width, t) - SmoothStep(to - width, to, t);
        }
        /// <summary>
        /// 返回<paramref name="a"/>与<paramref name="b"/>中绝对值较大的值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float AbsMax(float a, float b)
        {
            if (Math.Abs(a) > Math.Abs(b))
            {
                return a;
            }
            return b;
        }
        /// <summary>
        /// 返回<paramref name="a"/>与<paramref name="b"/>中绝对值较小的值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float AbsMin(float a, float b)
        {
            if (Math.Abs(a) > Math.Abs(b))
            {
                return b;
            }
            return a;
        }
        /// <summary>
        /// 返回<paramref name="a"/>与<paramref name="b"/>中由x，y分量绝对值最大值构成的新向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 AbsMax(Vector2 a, Vector2 b)
        {
            return new Vector2(AbsMax(a.X, b.X), AbsMax(a.Y, b.Y));
        }
        /// <summary>
        /// 返回<paramref name="a"/>与<paramref name="b"/>中由x，y分量绝对值最小值构成的新向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 AbsMin(Vector2 a, Vector2 b)
        {
            return new Vector2(AbsMin(a.X, b.X), AbsMin(a.Y, b.Y));
        }
        /// <summary>
        /// 返回<paramref name="vector"/>的单位向量，若为零向量则返回UnitX
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 NormalizeSafe(this Vector2 vector)
        {
            float len = Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (len == 0)
            {
                return Vector2.UnitX;
            }
            else
            {
                return new Vector2(vector.X / len, vector.Y / len);
            }
        }
        public static Vector2 NormalLine(this Vector2 vec) => new Vector2(-vec.Y, vec.X).NormalizeSafe();
        public static float ToRotationSafe(this Vector2 vector)
        {
            float len = Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (len == 0)
            {
                return 0;
            }
            else
            {
                return (float)Math.Atan2(vector.Y, vector.X);
            }
        }
        public static Rotation ToRot(this Vector2 vector) => new Rotation(vector.ToRotation());
        public static Rotation ToRotSafe(this Vector2 vector) => new Rotation(vector.ToRotationSafe());
        public static float Sqrt(float num) => (float)Math.Sqrt(num);
        public static float Cos(float num) => (float)Math.Cos(num);
        public static float Sin(float num) => (float)Math.Sin(num);
        public static float Lerp(float from, float to, float t) => (1 - t) * from + to * t;
    }
}

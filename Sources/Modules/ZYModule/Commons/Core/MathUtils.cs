using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
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
            return val + (target - val).Normalize_S() * maxMove;
        }

        public static float Sqrt(float num) => (float)Math.Sqrt(num);
    }
}

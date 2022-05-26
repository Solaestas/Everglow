using Terraria.GameContent;
namespace Everglow.Sources.Commons.Function.BezierCurve
{
    public class Bezier
    {
        /// <summary>
        /// 根据输入点的List获得贝塞尔曲线
        /// </summary>
        /// <param name="OrigLine"></param>
        /// <param name="aimCount"></param>
        /// <returns></returns>
        public static List<Vector2> GetBezier(List<Vector2> OrigLine, int aimCount)
        {
            if(OrigLine.Count < 4)
            {
                switch (OrigLine.Count)
                {
                    case 0:
                        {
                            return new List<Vector2>();//空传入返回空
                        }
                    case 1:
                        {
                            for (int x = 1; x < aimCount; x++)
                            {
                                OrigLine.Add(OrigLine[0]);
                            }
                            return OrigLine;//1长度传入,直接返回[目标长度]个原值
                        }
                    case 2:
                        {
                            List<Vector2> Line = new List<Vector2>();
                            for (int x = 0; x < aimCount; x++)
                            {
                                float t = (x + 0.5f) / (float)aimCount;
                                Line.Add(OrigLine[0] * (1 - t) + OrigLine[1] * t);
                            }
                            return Line;//2长度传入,返回1阶贝塞尔曲线(直线)
                        }
                    case 3:
                        {
                            List<Vector2> Line = new List<Vector2>();
                            for (int x = 0; x < aimCount; x++)
                            {
                                float t = (x + 0.5f) / (float)aimCount;
                                Line.Add(OrigLine[0] * (1 - t * t) + OrigLine[1] * (2 * t) * (1 - t) + OrigLine[2] * t * t);
                            }            
                            return Line;//3长度传入,返回2阶贝塞尔曲线
                        }
                }
                return OrigLine;
            }
            else
            {
                List<Vector2> Line = new List<Vector2>();
                float OrigCount = OrigLine.Count;
                float TrueCount = aimCount;
                for (int x = 0;x < aimCount; x++)
                {
                    float amo = x / TrueCount * OrigCount / 4f;//进度插值
                    int Addx = (int)(Math.Clamp(x / TrueCount * OrigCount, 0, OrigCount - 4));//换点增量
                    float Tamo = amo % 1f;//插值处理，这里不是很理解
                    if(Addx >= OrigCount - 7)
                    {
                        Tamo = amo % 3f;
                    }
                    float AimX = MathHelper.CatmullRom(OrigLine[0 + Addx].X, OrigLine[1 + Addx].X, OrigLine[2 + Addx].X, OrigLine[3 + Addx].X, Tamo);
                    float AimY = MathHelper.CatmullRom(OrigLine[0 + Addx].Y, OrigLine[1 + Addx].Y, OrigLine[2 + Addx].Y, OrigLine[3 + Addx].Y, Tamo);
                    Line.Add(new Vector2(AimX, AimY));
                }
                return Line;//超过4长度,返回Catmull-Rom曲线
            }
        }
    }
}

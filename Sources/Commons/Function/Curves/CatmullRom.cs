namespace Everglow.Sources.Commons.Function.Curves;

namespace Everglow.Sources.Commons.Function.Curves
{
public static class CatmullRom
{
    /// <summary>
    /// 根据输入点的List获得一个使用CatmullRom样条平滑过后的路径
    /// </summary>
    /// <param name="OrigLine"></param>
    /// <param name="aimCount"></param>
    /// <returns></returns>
    public static List<Vector2> SmoothPath(IEnumerable<Vector2> origPath)
    {
        int count = origPath.Count();
        if (count <= 2)
        {
            return origPath.ToList();
        }

        Vector2[] path = new Vector2[count + 2];
        var it = origPath.GetEnumerator();
        int index = 0;
        while (it.MoveNext())
        {
            path[++index] = it.Current;
        }
        // 头尾增加两个不影响曲线效果的点
        path[0] = path[1] * 2 - path[2];
        path[^1] = path[^2] * 2 - path[^3];

        List<Vector2> result = new List<Vector2>(count * 3);

        for (int i = 1; i < count; i++)
        {
            float rotCurrent = (path[i] - path[i - 1]).ToRotation();
            float rotNext = (path[i + 2] - path[i + 1]).ToRotation();

            // 根据当前和下一个节点所代表的向量的旋转差异来增加采样数量
            // 如果旋转差异越大，采样数量就越大
            float dis = Math.Abs(rotCurrent - rotNext);
            int dom = (int)((dis >= MathHelper.Pi ? MathHelper.TwoPi - dis : dis) / 0.22f + 2);
            float factor = 1.0f / dom;
            for (float j = 0; j < 1.0f; j += factor)
            {
                result.Add(Vector2.CatmullRom(path[i - 1], path[i], path[i + 1], path[i + 2], j));
            }
        }
        result.Add(path[^2]);
        return result;

    }
}
}

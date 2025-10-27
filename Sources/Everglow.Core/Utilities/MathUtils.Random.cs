namespace Everglow.Commons.Utilities;

public static partial class MathUtils
{
	/// <summary>
	/// 返回标准正态分布的一个随机数
	/// </summary>
	/// <param name="random"></param>
	/// <returns></returns>
	public static float NormalDistribution(Random random)
	{
		double u = -2 * Math.Log(random.NextDouble());
		double v = 2 * Math.PI * random.NextDouble();
		return (float)Math.Max(0, Math.Sqrt(u) * Math.Cos(v) * 0.3 + 0.5);
	}
}
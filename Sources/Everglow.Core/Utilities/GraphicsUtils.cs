using Everglow.Commons.DataStructures;

namespace Everglow.Commons.Utilities;

public static class GraphicsUtils
{
	public static void Begin(this SpriteBatch spriteBatch, SpriteBatchState state)
	{
		spriteBatch.Begin(
			state.SortMode,
			state.BlendState,
			state.SamplerState,
			state.DepthStencilState,
			state.RasterizerState,
			state.Effect,
			state.TransformMatrix);
	}

	/// <summary>
	/// 根据输入点的List获得一个使用CatmullRom样条平滑过后的路径
	/// </summary>
	/// <param name="origPath"> </param>
	/// <param name="precision"> null : 根据角度差自动适配取点个数， not null ：最少为2 </param>
	/// <returns> </returns>
	public static List<Vector2> CatmullRom(IEnumerable<Vector2> origPath, int? precision = null)
	{
		int count = origPath.Count();
		if (count <= 2)
			return origPath.ToList();

		var path = new Vector2[count + 2];
		var it = origPath.GetEnumerator();
		int index = 0;
		while (it.MoveNext())
		{
			path[++index] = it.Current;
		}

		// 头尾增加两个不影响曲线效果的点
		path[0] = path[1] * 2 - path[2];
		path[^1] = path[^2] * 2 - path[^3];

		List<Vector2> result = new(count * 3);

		for (int i = 1; i < count; i++)
		{
			float rotCurrent = new Rotation(path[i] - path[i - 1]).Radian;
			float rotNext = new Rotation(path[i + 2] - path[i + 1]).Radian;
			int dom;
			if (precision is null)
			{
				// 根据当前和下一个节点所代表的向量的旋转差异来增加采样数量 如果旋转差异越大，采样数量就越大
				if (float.IsNaN(rotCurrent) || float.IsNaN(rotNext))
				{
					dom = 2;
				}
				else
				{
					float dis = Math.Abs(rotCurrent - rotNext);
					dom = (int)((dis >= MathHelper.Pi ? MathHelper.TwoPi - dis : dis) / 0.22f + 2);
				}
			}
			else
			{
				dom = precision.Value;
			}
			float factor = 1.0f / dom;
			for (float j = 0; j < 1.0f; j += factor)
			{
				result.Add(Vector2.CatmullRom(path[i - 1], path[i], path[i + 1], path[i + 2], j));
			}
		}
		result.Add(path[^2]);
		return result;
	}

	public static SpriteBatchState? GetState(this SpriteBatch spriteBatch)
	{
		if (!spriteBatch.beginCalled)
		{
			return null;
		}
		return new SpriteBatchState(
			spriteBatch.sortMode,
			spriteBatch.blendState,
			spriteBatch.samplerState,
			spriteBatch.depthStencilState,
			spriteBatch.rasterizerState,
			spriteBatch.transformMatrix,
			spriteBatch.customEffect);
	}
}
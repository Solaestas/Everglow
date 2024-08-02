using System;
using System.Linq;
using Everglow.Commons.DataStructures;
using Terraria;
using static Terraria.WorldBuilding.Actions;

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

	/// <summary>
	/// 根据输入点的List获得一条贝塞尔曲线
	/// </summary>
	/// <param name="origPath"> </param>
	/// <param name="precision"> 最少为2 ,不建议超过100</param>
	/// <returns> </returns>
	public static List<Vector2> BezierCurve(IEnumerable<Vector2> origPath, int precision = 10)
	{
		int count = origPath.Count() - 1;
		if (count <= 2)
		{
			return origPath.ToList();
		}
		if (precision < 2)
		{
			precision = 2;
		}
		Main.NewText(count);

		float factor = 1.00f / (precision*count);
		List<Vector2> result = new List<Vector2>();
		for (float t = 0; t < 1.0f; t += factor)
		{
			Vector2 point = Vector2.Zero;

			Vector2[] p = new Vector2[count + 1];
			var it = origPath.GetEnumerator();
			int index = 0;
			while (it.MoveNext())
			{
				p[index] = it.Current;
				index++;
			}

			for (int i = 0; i < count; i++)
			{
				point += p[i] * MathF.Pow(1 - t, count - i) * MathF.Pow(t, i) * MathUtils.Combination(count, i);
			}
			result.Add(point);
		}

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




	/// <summary>
	/// 
	/// </summary>
	/// <param name="t"></param>
	/// <param name="p"></param>
	/// <returns></returns>
	private static Vector2 GetBezierPoint(float t, params Vector2[] p)
	{
		int n = p.Length - 1;
		float u = 1 - t;

		Vector2 result = Vector2.Zero;

		for (int i = 0; i < p.Length; i++)
		{
			result += p[i] * MathF.Pow(u, n - i) * MathF.Pow(t, i) * MathUtils.Combination(n, i);
		}
		return result;
	}

}
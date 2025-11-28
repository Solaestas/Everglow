using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using static Everglow.Commons.Utilities.MathUtils;

namespace Everglow.Commons.DeveloperContent.Items;

[Pipeline(typeof(WCSPipeline))]
public class PolygonCollisionHelper : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public List<Vector2> PolygonVertexs = new List<Vector2>();

	public Player Owner = null;

	public Item TargetItem = null;

	public override void Update()
	{
		if (Owner is null)
		{
			return;
		}
		var heldItem = Owner.HeldItem;
		if (heldItem is null)
		{
			return;
		}
		if (heldItem.type == ModContent.ItemType<PolygonColiderItem>())
		{
			TargetItem = heldItem;
		}
		PolygonColiderItem pItem = TargetItem.ModItem as PolygonColiderItem;
		if (pItem is null)
		{
			return;
		}
		if (Owner.HeldItem != TargetItem)
		{
			pItem.ColiderVFXTimer = 0;
			Active = false;
			return;
		}
		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			PolygonVertexs.Add(Main.MouseWorld);
		}
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			for (int k = PolygonVertexs.Count - 1; k >= 0; k--)
			{
				Vector2 point = PolygonVertexs[k];
				if ((point - Main.MouseWorld).Length() < 20)
				{
					PolygonVertexs.RemoveAt(k);
				}
			}
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.LightPoint2.Value;
		for (int i = 0; i < PolygonVertexs.Count; i++)
		{
			Ins.Batch.Draw(tex, PolygonVertexs[i], null, new Color(1f, 1f, 1f, 0), 0, tex.Size() * 0.5f, 1f, SpriteEffects.None);
			if (PolygonVertexs.Count >= 2)
			{
				Vector2 next;
				if (i < PolygonVertexs.Count - 1)
				{
					next = PolygonVertexs[i + 1];
				}
				else
				{
					next = PolygonVertexs[0];
				}
				DrawLine(PolygonVertexs[i], next);
			}
		}
		Color color = new Color(0.2f, 0.2f, 0.2f, 0);
		if (IsPointInPolygon(PolygonVertexs, Main.MouseWorld))
		{
			color = new Color(0.2f, 0.2f, 1f, 0);
			Main.instance.MouseText("Inside");
		}
		Ins.Batch.Draw(tex, Main.MouseWorld, null, color, 0, tex.Size() * 0.5f, 2f, SpriteEffects.None);
	}

	public void DrawLine(Vector2 pos0, Vector2 pos1)
	{
		Texture2D tex = ModAsset.White.Value;
		List<Vertex2D> bars = new List<Vertex2D>();
		Vector2 dir = (pos0 - pos1).RotatedBy(MathHelper.PiOver2).NormalizeSafe();
		bars.Add(pos0 + dir, Color.White, new Vector3(0, 0, 0));
		bars.Add(pos0 - dir, Color.White, new Vector3(0, 0, 0));
		bars.Add(pos1 + dir, Color.White, new Vector3(0, 0, 0));
		bars.Add(pos1 - dir, Color.White, new Vector3(0, 0, 0));
		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);
	}

	/// <summary>
	/// 校验多边形边是否相交（相邻边除外，避免自相交多边形）暂时不用
	/// </summary>
	private static void CheckPolygonEdges(List<Vector2> polygon)
	{
		int n = polygon.Count;
		for (int i = 0; i < n; i++)
		{
			// 当前边：i -> (i+1)%n
			int i1 = i;
			int i2 = (i + 1) % n;
			Vector2 a1 = polygon[i1];
			Vector2 a2 = polygon[i2];

			// 检查与后续非相邻边是否相交
			for (int j = i + 2; j < n; j++)
			{
				// 跳过最后一条边与第一条边的重复检查（i=n-1时，j会到n，无需处理）
				if (i == n - 1 && j == n)
				{
					continue;
				}

				int j1 = j;
				int j2 = (j + 1) % n;
				Vector2 b1 = polygon[j1];
				Vector2 b2 = polygon[j2];

				// 检测两条线段是否相交
				// if (DoLineSegmentsIntersect(a1, a2, b1, b2))
				// {
				// return false;
				// }
			}
		}
	}
	/// <summary>
	/// 判断两条线段是否相交（含端点）
	/// </summary>
	private static bool DoLineSegmentsIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
	{
		// 跨立实验+叉积判断
		int o1 = Orientation(a1, a2, b1);
		int o2 = Orientation(a1, a2, b2);
		int o3 = Orientation(b1, b2, a1);
		int o4 = Orientation(b1, b2, a2);

		// 普通相交（非共线）
		if (o1 != o2 && o3 != o4)
		{
			return true;
		}

		// 共线情况：判断端点是否在线段上
		if (o1 == 0 && IsPointOnLineSegment(b1, a1, a2))
		{
			return true;
		}

		if (o2 == 0 && IsPointOnLineSegment(b2, a1, a2))
		{
			return true;
		}

		if (o3 == 0 && IsPointOnLineSegment(a1, b1, b2))
		{
			return true;
		}

		if (o4 == 0 && IsPointOnLineSegment(a2, b1, b2))
		{
			return true;
		}

		return false;
	}
}
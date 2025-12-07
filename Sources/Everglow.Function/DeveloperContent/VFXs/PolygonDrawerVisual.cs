using Everglow.Commons.DeveloperContent.Items;
using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using static Everglow.Commons.Utilities.MathUtils;

namespace Everglow.Commons.DeveloperContent.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class PolygonDrawerVisual : Visual
{
	public const int RightClickDetectionRange = 20;

	public List<Vector2> PolygonVertices = [];
	public Player Owner = null;
	public Item TargetItem = null;

	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public override void Update()
	{
		// Data validation
		if (Owner?.HeldItem is null)
		{
			throw new InvalidOperationException("Owner must be initialized correctly before updating.");
		}

		// Ensure player holding specfic item
		if (Owner.HeldItem.type != ModContent.ItemType<PolygonDrawerItem>()
			|| Owner.HeldItem.ModItem is not PolygonDrawerItem poly
			|| poly.Visual != this)
		{
			Active = false;
			return;
		}

		TargetItem = Owner.HeldItem;

		if (Main.mapFullscreen)
		{
			return;
		}

		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			PolygonVertices.Add(Main.MouseWorld);
		}

		if (Main.mouseRight && Main.mouseRightRelease)
		{
			for (int k = PolygonVertices.Count - 1; k >= 0; k--)
			{
				Vector2 point = PolygonVertices[k];
				if ((point - Main.MouseWorld).Length() < RightClickDetectionRange)
				{
					PolygonVertices.RemoveAt(k);
				}
			}
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.LightPoint2.Value;
		for (int i = 0; i < PolygonVertices.Count; i++)
		{
			Ins.Batch.Draw(tex, PolygonVertices[i], null, new Color(1f, 1f, 1f, 0), 0, tex.Size() * 0.5f, 1f, SpriteEffects.None);
			if (PolygonVertices.Count >= 2)
			{
				Vector2 next;
				if (i < PolygonVertices.Count - 1)
				{
					next = PolygonVertices[i + 1];
				}
				else
				{
					next = PolygonVertices[0];
				}
				DrawLine(PolygonVertices[i], next);
			}
		}
		Color color = new Color(0.2f, 0.2f, 0.2f, 0);
		if (IsPointInPolygon(PolygonVertices, Main.MouseWorld))
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
}
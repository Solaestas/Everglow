using Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs.Arena;

[Pipeline(typeof(WCSPipeline))]
public class NPCBossValueBar : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public NPC TargetBoss = null;

	public override void Update()
	{
		if (TargetBoss == null || !TargetBoss.active)
		{
			foreach (var npc in Main.npc)
			{
				if (npc != null && npc.active && npc.ModNPC is TownNPC_LiveInYggdrasil)
				{
					TargetBoss = npc;
					break;
				}
			}
		}
		if (TargetBoss == null)
		{
			return;
		}
		var tNLIY = TargetBoss.ModNPC as TownNPC_LiveInYggdrasil;
		if (tNLIY == null)
		{
			return;
		}
		if (!YggdrasilTownCentralSystem.InArena_YggdrasilTown())
		{
			Active = false;
			return;
		}
		base.Update();
	}

	public override void Draw()
	{
		if (Main.mapFullscreen)
		{
			return;
		}
		if (TargetBoss == null || !TargetBoss.active)
		{
			return;
		}
		var tNLIY = TargetBoss.ModNPC as TownNPC_LiveInYggdrasil;
		if (tNLIY == null)
		{
			return;
		}
		if (!tNLIY.ShouldDrawHealthBar)
		{
			return;
		}
		var lightColor = Lighting.GetColor(TargetBoss.Center.ToTileCoordinates()) * 2;
		var backgroundColor = new Color(0, 0, 0, 1f);
		float value = Math.Clamp(tNLIY.Resilience / tNLIY.ResilienceMax, 0, 1);
		Texture2D resilienceBar = Commons.ModAsset.TileBlock.Value;
		float hBScale = tNLIY.HealthBarScale;
		var drawPos = tNLIY.HealthBarPos + new Vector2(0, 15 * hBScale);
		float widthBar = 16;
		float heightBar = 3;
		List<Vertex2D> bars = new List<Vertex2D>();

		// left bound
		bars.Add(drawPos + new Vector2(-widthBar, -heightBar) * hBScale, backgroundColor, new Vector3(0f, 0, 0));
		bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, backgroundColor, new Vector3(0f, 1, 0));
		bars.Add(drawPos + new Vector2(-widthBar + 2, -heightBar) * hBScale, backgroundColor, new Vector3(2 / 16f, 0, 0));

		bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, backgroundColor, new Vector3(0f, 1, 0));
		bars.Add(drawPos + new Vector2(-widthBar + 2, -heightBar) * hBScale, backgroundColor, new Vector3(2 / 16f, 0, 0));
		bars.Add(drawPos + new Vector2(-widthBar + 2, heightBar) * hBScale, backgroundColor, new Vector3(2 / 16f, 1, 0));

		// right bound
		bars.Add(drawPos + new Vector2(widthBar, -heightBar) * hBScale, backgroundColor, new Vector3(1f, 0, 0));
		bars.Add(drawPos + new Vector2(widthBar, heightBar) * hBScale, backgroundColor, new Vector3(1f, 1, 0));
		bars.Add(drawPos + new Vector2(widthBar - 2, -heightBar) * hBScale, backgroundColor, new Vector3(14 / 16f, 0, 0));

		bars.Add(drawPos + new Vector2(widthBar, heightBar) * hBScale, backgroundColor, new Vector3(1f, 1, 0));
		bars.Add(drawPos + new Vector2(widthBar - 2, -heightBar) * hBScale, backgroundColor, new Vector3(14 / 16f, 0, 0));
		bars.Add(drawPos + new Vector2(widthBar - 2, heightBar) * hBScale, backgroundColor, new Vector3(14 / 16f, 1, 0));

		// body
		bars.Add(drawPos + new Vector2(-widthBar, -heightBar) * hBScale, backgroundColor, new Vector3(0.5f, 0, 0));
		bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, backgroundColor, new Vector3(0.5f, 1, 0));
		bars.Add(drawPos + new Vector2(widthBar, -heightBar) * hBScale, backgroundColor, new Vector3(0.5f, 0, 0));

		bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, backgroundColor, new Vector3(0.5f, 1, 0));
		bars.Add(drawPos + new Vector2(widthBar, -heightBar) * hBScale, backgroundColor, new Vector3(0.5f, 0, 0));
		bars.Add(drawPos + new Vector2(widthBar, heightBar) * hBScale, backgroundColor, new Vector3(0.5f, 1, 0));

		if (widthBar * 2 * value > 2)
		{
			// left bound_bar value
			bars.Add(drawPos + new Vector2(-widthBar, -heightBar) * hBScale, lightColor, new Vector3(0f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, lightColor, new Vector3(0f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar + 2, -heightBar) * hBScale, lightColor, new Vector3(2 / 16f, 0, 0));

			bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, lightColor, new Vector3(0f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar + 2, -heightBar) * hBScale, lightColor, new Vector3(2 / 16f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar + 2, heightBar) * hBScale, lightColor, new Vector3(2 / 16f, 1, 0));

			// right bound_bar value
			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value, -heightBar) * hBScale, lightColor, new Vector3(1f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value, heightBar) * hBScale, lightColor, new Vector3(1f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value - 2, -heightBar) * hBScale, lightColor, new Vector3(14 / 16f, 0, 0));

			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value, heightBar) * hBScale, lightColor, new Vector3(1f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value - 2, -heightBar) * hBScale, lightColor, new Vector3(14 / 16f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value - 2, heightBar) * hBScale, lightColor, new Vector3(14 / 16f, 1, 0));

			// body_bar value
			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value - 2, -heightBar) * hBScale, lightColor, new Vector3(0.5f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value - 2, heightBar) * hBScale, lightColor, new Vector3(0.5f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar, -heightBar) * hBScale, lightColor, new Vector3(0.5f, 0, 0));

			bars.Add(drawPos + new Vector2(-widthBar + widthBar * 2 * value - 2, heightBar) * hBScale, lightColor, new Vector3(0.5f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar, -heightBar) * hBScale, lightColor, new Vector3(0.5f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, lightColor, new Vector3(0.5f, 1, 0));
		}
		else
		{
			// only a line now.
			float widthSide = widthBar * 2 * value;
			bars.Add(drawPos + new Vector2(-widthBar, -heightBar) * hBScale, lightColor, new Vector3(0f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, lightColor, new Vector3(0f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthSide, -heightBar) * hBScale, lightColor, new Vector3(2 / 16f, 0, 0));

			bars.Add(drawPos + new Vector2(-widthBar, heightBar) * hBScale, lightColor, new Vector3(0f, 1, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthSide, -heightBar) * hBScale, lightColor, new Vector3(2 / 16f, 0, 0));
			bars.Add(drawPos + new Vector2(-widthBar + widthSide, heightBar) * hBScale, lightColor, new Vector3(2 / 16f, 1, 0));
		}

		Main.graphics.GraphicsDevice.Textures[0] = resilienceBar;
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}
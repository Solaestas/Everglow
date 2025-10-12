using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Terraria.Audio;

namespace Everglow.Commons.TileHelper;

[Pipeline(typeof(WCSPipeline_PointWrap))]
public class HangingTile_LengthAdjustingSystem : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public Point FixPoint;

	public int Style;
	public int Timer = 0;
	public int TimeToKill = -1;

	public float StartFrameY60;
	public float PanelRange = 0;
	public float HandleRotation;

	public Vector2 OldMouseDirection = new Vector2(0, -1);
	public Queue<float> OldRotatedSpeeds = new Queue<float>();

	public override void OnSpawn()
	{
		HandleRotation = 0;
		Timer = 0;
	}

	public override void Update()
	{
		Player player = Main.LocalPlayer;
		int i = FixPoint.X;
		int j = FixPoint.Y;
		if (i < 20 || i > Main.maxTilesX - 20 || j < 20 || j > Main.maxTilesY - 20)
		{
			Active = false;
			return;
		}
		Tile tile = Main.tile[i, j];
		HangingTile hangingTile = TileLoader.GetTile(tile.type) as HangingTile;
		if (hangingTile == null)
		{
			Active = false;
			return;
		}

		// identify the player.
		if (hangingTile.KnobAdjustingPlayers.ContainsKey(FixPoint))
		{
			Player player2;
			hangingTile.KnobAdjustingPlayers.TryGetValue(FixPoint, out player2);
			if (player2 != player)
			{
				Active = false;
				return;
			}
		}

		Timer++;
		if (TimeToKill > 0)
		{
			TimeToKill--;
			if (TimeToKill <= 0)
			{
				Kill(hangingTile, player);
			}
		}
		if (Timer < 12)
		{
			PanelRange = (MathF.Sin(Timer / 10f * MathHelper.Pi - MathHelper.PiOver2) + 1.2f) * 0.5f;
		}
		else
		{
			PanelRange = 1;
		}
		if (TimeToKill < 12 && TimeToKill > 0)
		{
			PanelRange *= (MathF.Sin(TimeToKill / 10f * MathHelper.Pi - MathHelper.PiOver2) + 1.2f) * 0.5f;
		}
		else
		{
			PanelRange *= 1;
		}
		if (player.HeldItem.createTile != tile.TileType)
		{
			EndAdjustment();
			return;
		}
		if ((player.MouseWorld() - FixPoint.ToWorldCoordinates()).Length() > 400)
		{
			EndAdjustment();
			return;
		}

		// Only display when mouse over.
		if (Style == 0)
		{
			int x = (int)(player.MouseWorld().X / 16f);
			int y = (int)(player.MouseWorld().Y / 16f);
			if (x != FixPoint.X || y != FixPoint.Y)
			{
				Active = false;
				if (hangingTile.MouseOverWinchPlayers.ContainsKey(player))
				{
					hangingTile.MouseOverWinchPlayers.Remove(player);
				}
				return;
			}
		}
		if (Style == 1)
		{
			if (!hangingTile.KnobAdjustingPlayers.ContainsKey(FixPoint))
			{
				EndAdjustment();
				return;
			}
			if (!hangingTile.KnobAdjustingPlayers.ContainsKey(FixPoint))
			{
				EndAdjustment();
				return;
			}
			float nowFrameY = StartFrameY60 / 60f + HandleRotation * 2;
			bool endChain = false;
			if (nowFrameY < 2)
			{
				endChain = true;
			}
			if (nowFrameY > hangingTile.MaxCableLength - 2)
			{
				endChain = true;
			}
			if (tile.TileFrameY != (short)Math.Clamp(nowFrameY, 1, hangingTile.MaxCableLength - 1))
			{
				if (!endChain)
				{
					SoundEngine.PlaySound(SoundID.Unlock.WithVolume(0.5f), FixPoint.ToWorldCoordinates());
				}
				tile.TileFrameY = (short)Math.Clamp(nowFrameY, 1, hangingTile.MaxCableLength - 1);
			}

			Vector2 handleDir = new Vector2(1, 0).RotatedBy(HandleRotation);
			Vector2 newDir = Utils.SafeNormalize(FixPoint.ToWorldCoordinates() - player.MouseWorld(), new Vector2(0, -1));
			float addAccRot = MathF.Asin(-Vector3.Cross(new Vector3(newDir, 0), new Vector3(handleDir, 0)).Z);
			float addOldToNewRot = MathF.Asin(-Vector3.Cross(new Vector3(newDir, 0), new Vector3(OldMouseDirection, 0)).Z);
			OldMouseDirection = newDir;
			OldRotatedSpeeds.Enqueue(addOldToNewRot);
			if (OldRotatedSpeeds.Count > 3)
			{
				OldRotatedSpeeds.Dequeue();
			}
			float cosValue = Vector2.Dot(handleDir, newDir);
			if (nowFrameY >= 1 && nowFrameY <= hangingTile.MaxCableLength - 1)
			{
				HandleRotation += addAccRot;
			}
			else if (nowFrameY < 1 && RotatedClockwise() && cosValue > 0.8f)
			{
				HandleRotation += addAccRot;
			}
			else if (nowFrameY > hangingTile.MaxCableLength - 1 && RotatedCounterclockwise() && cosValue > 0.8f)
			{
				HandleRotation += addAccRot;
			}
			int x = (int)(player.MouseWorld().X / 16f);
			int y = (int)(player.MouseWorld().Y / 16f);
			if (Main.mouseRight && Main.mouseRightRelease && (x != FixPoint.X || y != FixPoint.Y))
			{
				EndAdjustment();
				return;
			}
		}
		base.Update();
	}

	public bool RotatedClockwise()
	{
		for (int i = 0; i < OldRotatedSpeeds.Count; i++)
		{
			if (OldRotatedSpeeds.ToArray()[i] <= 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool RotatedCounterclockwise()
	{
		for (int i = 0; i < OldRotatedSpeeds.Count; i++)
		{
			if (OldRotatedSpeeds.ToArray()[i] >= 0)
			{
				return false;
			}
		}
		return true;
	}

	public void EndAdjustment()
	{
		if (TimeToKill < 0)
		{
			TimeToKill = 12;
		}
	}

	public void Kill(HangingTile hangingTile, Player owner)
	{
		Active = false;
		if (hangingTile.MouseOverWinchPlayers.ContainsKey(owner))
		{
			hangingTile.MouseOverWinchPlayers.Remove(owner);
		}
		if (hangingTile.KnobAdjustingPlayers.ContainsKey(FixPoint))
		{
			hangingTile.KnobAdjustingPlayers.Remove(FixPoint);
		}
	}

	public override void Draw()
	{
		int i = FixPoint.X;
		int j = FixPoint.Y;
		if (i < 20 || i > Main.maxTilesX - 20)
		{
			if (j < 20 || j > Main.maxTilesY - 20)
			{
				Active = false;
				return;
			}
		}
		Player player = Main.LocalPlayer;
		Tile tile = Main.tile[i, j];
		HangingTile hangingTile = TileLoader.GetTile(tile.type) as HangingTile;
		if (hangingTile == null)
		{
			Active = false;
			return;
		}
		Color drawColor = Color.Lerp(new Color(0.75f, 0.75f, 1f, 0.5f), new Color(0.85f, 0.85f, 0.75f, 0.5f), MathF.Sin((float)Main.timeForVisualEffects * 0.08f) * 0.5f + 0.5f);
		Color origDrawColor = drawColor;
		if (Style == 1)
		{
			float nowFrameY = StartFrameY60 / 60f + HandleRotation * 2;
			if (nowFrameY < 5)
			{
				drawColor = Color.Lerp(drawColor, new Color(1f, 0f, 0f, 0.8f), (5 - nowFrameY) / 4f);
			}
			if (nowFrameY > hangingTile.MaxCableLength - 5)
			{
				drawColor = Color.Lerp(drawColor, new Color(1f, 0f, 0f, 0.8f), (nowFrameY - (hangingTile.MaxCableLength - 5)) / 4f);
			}
		}

		// 不同种类物块标红
		if (player.HeldItem.createTile != tile.type)
		{
			drawColor = new Color(1f, 0, 0, 0.5f);
			Main.instance.MouseText("Different Type Error", ItemRarityID.Red);
		}

		// The circle panel
		Vector2 rotCenter = FixPoint.ToWorldCoordinates();
		Vector2 cut = new Vector2(1, 0).RotatedBy(HandleRotation + MathHelper.Pi);
		if (hangingTile.RopesOfAllThisTileInTheWorld.ContainsKey(FixPoint))
		{
			if (Style == 1)
			{
				DrawBackgroundPanel(i, j, Color.White * 0.5f);
				float maxCos = 0;
				int maxK = 0;

				// Calculate the bloom effect of calibration tails.
				for (int k = -10; k < 10; k++)
				{
					Vector2 cut2 = new Vector2(0, -1).RotatedBy(k / 20f * MathHelper.TwoPi);
					float cosValue = Vector2.Dot(cut, cut2);
					if (cosValue > maxCos)
					{
						maxCos = cosValue;
						maxK = k;
					}
				}

				// Draw calibration tails.
				Color newDrawColor = origDrawColor;
				float nowFrameY = StartFrameY60 / 60f + HandleRotation * 2;
				if (nowFrameY < 5)
				{
					newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), (5 - nowFrameY) / 4f);
				}
				if (nowFrameY > hangingTile.MaxCableLength - 5)
				{
					newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), (nowFrameY - (hangingTile.MaxCableLength - 5)) / 4f);
				}
				float tK = 12f;
				if (TimeToKill > 0)
				{
					tK = TimeToKill;
				}
				float fade = Math.Min(Timer, tK) / 12f;
				for (int k = -10; k < 10; k++)
				{
					Vector2 cut2 = new Vector2(0, -1).RotatedBy(k / 20f * MathHelper.TwoPi);
					float cosValue = Vector2.Dot(cut, cut2);

					if (k == maxK)
					{
						DrawLine_Black(rotCenter + cut2 * (4 * PanelRange - 12), rotCenter + cut2 * (4 * PanelRange + 36), 16);
						DrawLine(rotCenter + cut2 * (4 * PanelRange - 12), rotCenter + cut2 * (4 * PanelRange + 36), 16, newDrawColor * fade, 1f);
					}
					else
					{
						DrawLine(rotCenter + cut2 * 4 * PanelRange, rotCenter + cut2 * (4 * PanelRange + 24), 12, newDrawColor * fade, cosValue);
					}
				}

				// Draw bound.
				DrawBound(origDrawColor, player);
				DrawDirectionRing(FixPoint.X, FixPoint.Y, drawColor, HandleRotation - MathHelper.PiOver2);
			}
		}
		DrawBlockBound(FixPoint.X, FixPoint.Y, drawColor, HandleRotation);
	}

	public void DrawBound(Color color, Player player)
	{
		Vector2 rotCenter = FixPoint.ToWorldCoordinates();
		if ((player.MouseWorld() - rotCenter).Length() > 240)
		{
			Vector2 cut3 = (player.MouseWorld() - rotCenter).NormalizeSafe();
			float colorValue = ((player.MouseWorld() - rotCenter).Length() - 240f) / 160f;
			colorValue = Math.Min(colorValue, 1);
			Color newDrawColor = Color.Lerp(color, new Color(1f, 0f, 0f, 0f), colorValue);
			newDrawColor.A = 0;
			float colorFade = 1f;
			if(TimeToKill > 0)
			{
				colorFade = TimeToKill / 12f;
			}
			Color backgroundColor = Color.White * 0.6f * colorFade;
			newDrawColor *= colorFade;
			Main.instance.MouseText("Drag out of circle to cancel", ItemRarityID.White);

			List<Vertex2D> bars_left = new List<Vertex2D>();
			List<Vertex2D> bars_right = new List<Vertex2D>();

			List<Vertex2D> bars_left_black = new List<Vertex2D>();
			List<Vertex2D> bars_right_black = new List<Vertex2D>();
			for (int k = 0; k < 30; k++)
			{
				float xCoord = (colorValue - k / 30f) * 0.8f;
				xCoord = Math.Clamp(xCoord, 0, 1);
				bars_left_black.Add(rotCenter + cut3.RotatedBy(k / 30f) * 400f, backgroundColor, new Vector3(xCoord, 0.5f, 0));
				bars_left_black.Add(rotCenter + cut3.RotatedBy(k / 30f) * 350f, backgroundColor, new Vector3(xCoord, 0.8f, 0));

				bars_right_black.Add(rotCenter + cut3.RotatedBy(-k / 30f) * 400f, backgroundColor, new Vector3(xCoord, 0.5f, 0));
				bars_right_black.Add(rotCenter + cut3.RotatedBy(-k / 30f) * 350f, backgroundColor, new Vector3(xCoord, 0.8f, 0));

				bars_left.Add(rotCenter + cut3.RotatedBy(k / 30f) * 400f, newDrawColor, new Vector3(xCoord, 0.5f, 0));
				bars_left.Add(rotCenter + cut3.RotatedBy(k / 30f) * 350f, newDrawColor, new Vector3(xCoord, 0.8f, 0));

				bars_right.Add(rotCenter + cut3.RotatedBy(-k / 30f) * 400f, newDrawColor, new Vector3(xCoord, 0.5f, 0));
				bars_right.Add(rotCenter + cut3.RotatedBy(-k / 30f) * 350f, newDrawColor, new Vector3(xCoord, 0.8f, 0));
			}
			Ins.Batch.Draw(ModAsset.SparkDark.Value, bars_left_black, PrimitiveType.TriangleStrip);
			Ins.Batch.Draw(ModAsset.SparkDark.Value, bars_right_black, PrimitiveType.TriangleStrip);

			Ins.Batch.Draw(ModAsset.SparkLight.Value, bars_left, PrimitiveType.TriangleStrip);
			Ins.Batch.Draw(ModAsset.SparkLight.Value, bars_right, PrimitiveType.TriangleStrip);

			List<Vertex2D> text_bars = new List<Vertex2D>();
			Color textColor = Color.White;
			for (int k = -30; k < 30; k++)
			{
				float xCoord = -k / 15f;
				float fade = (30 * colorValue - Math.Abs(k)) / 15f;
				fade = Math.Clamp(fade, 0f, 1f);
				fade = MathF.Sin(fade);
				text_bars.Add(rotCenter + cut3.RotatedBy(k / 30f) * 420f, textColor * fade * colorFade, new Vector3(xCoord, 1f, 0));
				text_bars.Add(rotCenter + cut3.RotatedBy(k / 30f) * 400f, textColor * fade * colorFade, new Vector3(xCoord, 0f, 0));
			}
			Ins.Batch.Draw(ModAsset.HangingExit.Value, text_bars, PrimitiveType.TriangleStrip);
		}
	}

	public void DrawDirectionRing(int i, int j, Color color, float rotation)
	{
		color.A = 0;
		Vector2 pos = new Vector2(i, j) * 16 + new Vector2(8);
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int k = 0; k <= 60; k++)
		{
			bars.Add(pos + new Vector2(0, PanelRange * 24).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.5f, k / 15f, 0));
			bars.Add(pos + new Vector2(0, PanelRange * 0).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.3f, k / 15f, 0));
		}
		Ins.Batch.Draw(ModAsset.StarSlash.Value, bars, PrimitiveType.TriangleStrip);
		bars = new List<Vertex2D>();
		for (int k = 0; k <= 60; k++)
		{
			bars.Add(pos + new Vector2(0, PanelRange * 12).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.5f, k / 30f, 0));
			bars.Add(pos + new Vector2(0, PanelRange * 0).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.3f, k / 30f, 0));
		}
		Ins.Batch.Draw(ModAsset.StarSlash.Value, bars, PrimitiveType.TriangleStrip);
		bars = new List<Vertex2D>();
		for (int k = 0; k <= 60; k++)
		{
			bars.Add(pos + new Vector2(0, PanelRange * 22).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.5f, k / 60f, 0));
			bars.Add(pos + new Vector2(0, PanelRange * 12).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.3f, k / 60f, 0));
		}
		Ins.Batch.Draw(ModAsset.StarSlash.Value, bars, PrimitiveType.TriangleStrip);
		bars = new List<Vertex2D>();
		for (int k = 0; k <= 60; k++)
		{
			bars.Add(pos + new Vector2(0, PanelRange * 26).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.5f, 0.5f, 0));
			bars.Add(pos + new Vector2(0, PanelRange * 24).RotatedBy(k / 60f * MathHelper.TwoPi + rotation), color, new Vector3(0.5f, 0.5f, 0));
		}
		Ins.Batch.Draw(ModAsset.StarSlash.Value, bars, PrimitiveType.TriangleStrip);
	}

	public void DrawBackgroundPanel(int i, int j, Color color)
	{
		Vector2 pos = new Vector2(i, j) * 16 + new Vector2(8);
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int k = 0; k <= 60; k++)
		{
			bars.Add(pos + new Vector2(0, PanelRange * 70).RotatedBy(k / 60f * MathHelper.TwoPi), color, new Vector3(k / 60f, 0, 0));
			bars.Add(pos + new Vector2(0, PanelRange * 0).RotatedBy(k / 60f * MathHelper.TwoPi), color, new Vector3(k / 60f, 0.5f, 0));
		}
		Ins.Batch.Draw(ModAsset.Trail_7_black.Value, bars, PrimitiveType.TriangleStrip);
	}

	public void DrawBlockBound(int i, int j, Color color, float rotation)
	{
		Vector2 pos = new Vector2(i, j) * 16 + new Vector2(8);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(pos + new Vector2(-8, -8).RotatedBy(rotation), color, new Vector3(0, 0, 0)),
			new Vertex2D(pos + new Vector2(8, -8).RotatedBy(rotation), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(-8, 8).RotatedBy(rotation), color, new Vector3(0, 1, 0)),

			new Vertex2D(pos + new Vector2(-8, 8).RotatedBy(rotation), color, new Vector3(0, 1, 0)),
			new Vertex2D(pos + new Vector2(8, -8).RotatedBy(rotation), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(8, 8).RotatedBy(rotation), color, new Vector3(1, 1, 0)),
		};

		Ins.Batch.Draw(ModAsset.TileBlock.Value, bars, PrimitiveType.TriangleList);
	}

	public void DrawLine(Vector2 pos1, Vector2 pos2, float width, Color color, float highlight = 0)
	{
		Vector2 normal = Utils.SafeNormalize(pos1 - pos2, Vector2.zeroVector).RotatedBy(MathHelper.PiOver2) * width / 2f;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(pos1 + normal, color, new Vector3(0, 0, 0)),
			new Vertex2D(pos2 + normal, color, new Vector3(1, 0, 0)),
			new Vertex2D(pos1 - normal, color, new Vector3(0, 1, 0)),

			new Vertex2D(pos1 - normal, color, new Vector3(0, 1, 0)),
			new Vertex2D(pos2 + normal, color, new Vector3(1, 0, 0)),
			new Vertex2D(pos2 - normal, color, new Vector3(1, 1, 0)),
		};

		Ins.Batch.Draw(ModAsset.SquarePiece.Value, bars, PrimitiveType.TriangleList);

		if (highlight > 0)
		{
			Color bloomColor = color * highlight;
			bloomColor.A = 0;
			bars = new List<Vertex2D>()
			{
				 new Vertex2D(pos1 + normal, bloomColor, new Vector3(0, 0, 0)),
				 new Vertex2D(pos2 + normal, bloomColor, new Vector3(1, 0, 0)),
				 new Vertex2D(pos1 - normal, bloomColor, new Vector3(0, 1, 0)),

				 new Vertex2D(pos1 - normal, bloomColor, new Vector3(0, 1, 0)),
				 new Vertex2D(pos2 + normal, bloomColor, new Vector3(1, 0, 0)),
				 new Vertex2D(pos2 - normal, bloomColor, new Vector3(1, 1, 0)),
			};

			Ins.Batch.Draw(ModAsset.SquareBloom.Value, bars, PrimitiveType.TriangleList);
		}
	}

	public void DrawLine_Black(Vector2 pos1, Vector2 pos2, float width)
	{
		Vector2 normal = Utils.SafeNormalize(pos1 - pos2, Vector2.zeroVector).RotatedBy(MathHelper.PiOver2) * width / 2f;
		Color bloomColor = Color.White;
		float tK = 12f;
		if(TimeToKill > 0)
		{
			tK = TimeToKill;
		}
		float fade = Math.Min(Timer, tK) / 12f;
		bloomColor *= fade;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(pos1 + normal, bloomColor, new Vector3(0, 0, 0)),
			new Vertex2D(pos2 + normal, bloomColor, new Vector3(1, 0, 0)),
			new Vertex2D(pos1 - normal, bloomColor, new Vector3(0, 1, 0)),

			new Vertex2D(pos1 - normal, bloomColor, new Vector3(0, 1, 0)),
			new Vertex2D(pos2 + normal, bloomColor, new Vector3(1, 0, 0)),
			new Vertex2D(pos2 - normal, bloomColor, new Vector3(1, 1, 0)),
		};
		Ins.Batch.Draw(ModAsset.SquareBloom_black.Value, bars, PrimitiveType.TriangleList);
	}
}
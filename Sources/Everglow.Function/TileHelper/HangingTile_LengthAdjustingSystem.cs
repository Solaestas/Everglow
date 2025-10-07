using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Terraria.Audio;

namespace Everglow.Commons.TileHelper;

[Pipeline(typeof(WCSPipeline))]
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
		if (hangingTile.ChainPlayer.ContainsKey(FixPoint))
		{
			Player player2;
			hangingTile.ChainPlayer.TryGetValue(FixPoint, out player2);
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
			PanelRange = MathF.Sin(Timer / 10f * MathHelper.Pi - MathHelper.PiOver2) + 1.4f;
		}
		else
		{
			PanelRange = 1;
		}
		if (TimeToKill < 12 && TimeToKill > 0)
		{
			PanelRange *= MathF.Sin(TimeToKill / 10f * MathHelper.Pi - MathHelper.PiOver2) + 1.4f;
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
				if (hangingTile.MouseOverPoint.ContainsKey(player))
				{
					hangingTile.MouseOverPoint.Remove(player);
				}
				return;
			}
		}
		if (Style == 1)
		{
			if (!hangingTile.ChainPlayer.ContainsKey(FixPoint))
			{
				EndAdjustment();
				return;
			}
			if (!hangingTile.ChainPlayer.ContainsKey(FixPoint))
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
		if (hangingTile.MouseOverPoint.ContainsKey(owner))
		{
			hangingTile.MouseOverPoint.Remove(owner);
		}
		if (hangingTile.ChainPlayer.ContainsKey(FixPoint))
		{
			hangingTile.ChainPlayer.Remove(FixPoint);
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
				float maxCos = 0;
				int maxK = 0;
				for (int k = -10; k < 10; k++)
				{
					Vector2 cut2 = new Vector2(0, -1).RotatedBy(k / 20f * MathHelper.TwoPi);
					float cosValue = Vector2.Dot(cut, cut2);
					if (cosValue > maxCos)
					{
						maxCos = cosValue;
						maxK = k;
					}
					Color newDrawColor = origDrawColor;
					float thisFrameY = StartFrameY60 / 60f + (HandleRotation + k / 20f * MathHelper.TwoPi) * 2;
					if (thisFrameY < 5)
					{
						newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), (5 - thisFrameY) / 4f);
					}
					if (thisFrameY > hangingTile.MaxCableLength - 5)
					{
						newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), (thisFrameY - (hangingTile.MaxCableLength - 5)) / 4f);
					}
					DrawLine(rotCenter + cut2 * 16 * PanelRange, rotCenter + cut2 * (16 * PanelRange + 24), 12, newDrawColor, cosValue);
				}
				Vector2 cut3 = new Vector2(0, -1).RotatedBy(maxK / 20f * MathHelper.TwoPi);
				DrawLine(rotCenter + cut3 * 12 * PanelRange, rotCenter + cut3 * (12 * PanelRange + 36), 16, drawColor, 1f);
				if ((player.MouseWorld() - rotCenter).Length() > 240)
				{
					for (int k = -10; k < 10; k++)
					{
						Vector2 cut4 = cut.RotatedBy((k - 2.4f) * 0.04f);
						Vector2 cut5 = cut.RotatedBy((k + 2.4f) * 0.04f);
						float colorValue = ((player.MouseWorld() - rotCenter).Length() - 240f) / 160f;
						Color newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), colorValue);
						colorValue *= (100f - k * k) / 100f;
						DrawLine(rotCenter + cut4 * 390f, rotCenter + cut5 * 390f, 20f, newDrawColor * colorValue * 2);
					}
					Main.instance.MouseText("Drag out of circle to cancel", ItemRarityID.White);
				}
			}
		}
		DrawBlockBound(FixPoint.X, FixPoint.Y, drawColor, HandleRotation);
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
}
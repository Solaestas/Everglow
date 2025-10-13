using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Terraria.Audio;
using static Spine.SkeletonBinary;

namespace Everglow.Commons.TileHelper;

[Pipeline(typeof(WCSPipeline_PointWrap))]
public class HangingTile_LengthAdjustingSystem : Visual
{
	public struct DrawStack
	{
		public Texture2D Texture;

		public IEnumerable<Vertex2D> Vertices;

		public PrimitiveType PType;

		public DrawStack(Texture2D texture, IEnumerable<Vertex2D> vertices, PrimitiveType pType)
		{
			Texture = texture;
			Vertices = vertices;
			PType = pType;
		}
	}

	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public Point FixPoint;

	/// <summary>
	/// 0: mouse over;<br/>
	/// 1: full system.
	/// </summary>
	public int Style;
	public int Timer = 0;
	public int TimeToKill = -1;

	public float StartFrameY60;
	public float PanelRange = 0;
	public float HandleRotation;

	public Vector2 OldMouseDirection = new Vector2(0, -1);
	public Queue<float> OldRotatedSpeeds = new Queue<float>();

	/// <summary>
	/// Allow custon drawing styles.
	/// </summary>
	public delegate void CustomDrawPanel(HangingTile_LengthAdjustingSystem hangingAdj, Player player, Color color, ref Queue<DrawStack> drawStacks);

	public event CustomDrawPanel CustomPanel;

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
		float nowFrameY = StartFrameY60 / 60f + HandleRotation * 2;
		if (nowFrameY < 5)
		{
			drawColor = Color.Lerp(drawColor, new Color(1f, 0f, 0f, 0.8f), (5 - nowFrameY) / 4f);
		}
		if (nowFrameY > hangingTile.MaxCableLength - 5)
		{
			drawColor = Color.Lerp(drawColor, new Color(1f, 0f, 0f, 0.8f), (nowFrameY - (hangingTile.MaxCableLength - 5)) / 4f);
		}

		// If your HeldItem doesn't match with the tile type, it turns red.
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
				Queue<DrawStack> drawStacks = new Queue<DrawStack>();
				CustomPanel?.Invoke(this, player, origDrawColor, ref drawStacks);
				while(drawStacks.Count > 0)
				{
					DrawStack currentDS = drawStacks.Dequeue();
					Ins.Batch.Draw(currentDS.Texture, currentDS.Vertices, currentDS.PType);
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

	public void RegisterCustomPanelDrawing(CustomDrawPanel customDrawing)
	{
		CustomPanel += customDrawing;
	}

	/// <param name="customDrawing"></param>
	public void UnregisterCustomPanelDrawing(CustomDrawPanel customDrawing)
	{
		CustomPanel -= customDrawing;
	}
}
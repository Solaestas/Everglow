using Everglow.Commons.Enums;
using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// HangingTile, can be used to make special chandelier with adjustable cable length.
/// TileFrameY is  cable length.
/// </summary>
public abstract class HangingTile : ModTile, ITileFluentlyDrawn
{
	public override string Texture => "Everglow/Commons/TileHelper/HangingWinch";

	/// <summary>
	/// Max cable length : default 60
	/// </summary>
	public int MaxCableLength = 60;

	/// <summary>
	/// Lamp item mass : default 8
	/// </summary>
	public float SingleLampMass = 8;

	/// <summary>
	/// Cable mass : default 0.5f
	/// </summary>
	public float RopeUnitMass = 0.5f;

	/// <summary>
	/// Max style counts when hit wire.
	/// </summary>
	public int MaxWireStyle = 2;

	/// <summary>
	/// Elasticity of cable : default 150
	/// </summary>
	public float Elasticity = 150;

	/// <summary>
	/// Allow rotating joint to adjust length : default true
	/// </summary>
	public bool LengthAdjustable = true;

	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.BlocksWaterDrawingBehindSelf[Type] = true;

		// MyTileEntity refers to the tile entity mentioned in the previous section
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<CableEneity>().Hook_AfterPlacement, -1, 0, true);

		// This is required so the hook is actually called.
		TileObjectData.newTile.UsesCustomCanPlace = true;

		AddMapEntry(new Color(59, 67, 67));
	}

	/// <summary>
	/// Winch position and rope
	/// </summary>
	public Dictionary<Point, Rope> RopesOfAllThisTileInTheWorld = new Dictionary<Point, Rope>();

	/// <summary>
	/// Winch position and rope
	/// </summary>
	public Dictionary<Point, Player> ChainPlayer = new Dictionary<Point, Player>();

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		resetFrame = false;
		noBreak = true;
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public override void HitWire(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		tile.TileFrameX += 18;
		if (tile.TileFrameX > 54)
		{
			tile.TileFrameX = 0;
			tile.TileFrameY += 18;
		}
		int style = tile.TileFrameX / 18 + (tile.TileFrameY / 18) * 4;
		MaxWireStyle = Math.Min(MaxWireStyle, 16);
		if (style >= MaxWireStyle)
		{
			tile.TileFrameX = 0;
			tile.TileFrameY = 0;
		}
		base.HitWire(i, j);
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		if (RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			RopesOfAllThisTileInTheWorld.Remove(new Point(i, j));
		}
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
	}

	/// <summary>
	/// 靠近时根据TE挂绳
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="closer"></param>
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (!RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			AddRope(i, j);
		}
		base.NearbyEffects(i, j, closer);
	}

	/// <summary>
	/// 挂上绳子
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="rope"></param>
	public virtual void AddRope(int i, int j)
	{
		Rope rope = ConnectRope(i, j);
		if (rope == null)
		{
			return;
		}
		if (RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			return;
		}
		var masses = rope.Masses;
		RopesOfAllThisTileInTheWorld.Add(new Point(i, j), rope);
		TryGetCableEntityAs(i, j, out CableEneity cableEneity);
		if (cableEneity == null)
		{
			TileEntity.PlaceEntityNet(i, j, ModContent.TileEntityType<CableEneity>());
			TryGetCableEntityAs(i, j, out cableEneity);
		}
	}

	/// <summary>
	/// 挂绳
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="i2"></param>
	/// <param name="j2"></param>
	/// <returns></returns>
	public virtual Rope ConnectRope(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		int counts = MaxCableLength;
		int restCount = tile.TileFrameY;
		Rope rope = Rope.CreateWithHangHead(new Point(i, j).ToWorldCoordinates(), counts, Elasticity, RopeUnitMass, SingleLampMass, MaxCableLength - restCount);
		return rope;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Color lightColor = Lighting.GetColor(i, j);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}

		spriteBatch.Draw(ModAsset.HangingWinch.Value, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(0, 0, 16, 16), lightColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
		TileFluentDrawManager.AddFluentPoint(this, i, j);
		return false;
	}

	public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
	{
		base.DrawEffects(i, j, spriteBatch, ref drawData);
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		Rope rope;
		RopesOfAllThisTileInTheWorld.TryGetValue(pos, out rope);
		if (rope != null)
		{
			DrawCable(rope, pos, spriteBatch, tileDrawing);
		}
	}

	public string BulbTexturePath;

	/// <summary>
	/// 绘制挂物
	/// </summary>
	/// <param name="rope"></param>
	/// <param name="pos"></param>
	/// <param name="spriteBatch"></param>
	/// <param name="tileDrawing"></param>
	/// <param name="color"></param>
	public virtual void DrawCable(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = default(Color))
	{
		// 回声涂料
		if (!TileDrawing.IsVisible(Main.tile[pos]))
		{
			return;
		}

		var tile = Main.tile[pos];
		ushort type = tile.TileType;
		int paint = Main.tile[pos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(BulbTexturePath, type, 1, paint, tileDrawing);

		// 获取发绳端物块信息
		var masses = rope.Masses;
		for (int i = 0; i < masses.Length; i++)
		{
			Mass thisMass = masses[i];
			if (i < MaxCableLength - tile.TileFrameY)
			{
				thisMass.IsStatic = true;
				thisMass.Position = pos.ToWorldCoordinates();
				continue;
			}
			else
			{
				thisMass.IsStatic = false;
			}
			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1))
			{
				windCycle = tileDrawing.GetWindCycle((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), tileDrawing._sunflowerWindCounter);
			}

			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			if (!Main.gamePaused)
			{
				if (i < masses.Length - 1)
				{
					rope.ApplyForceSpecial(i, new Vector2(windCycle / 4.0f, 0.4f * thisMass.Value));
				}
				else
				{
					rope.ApplyForceSpecial(i, new Vector2(windCycle * 10.0f, 0.4f * thisMass.Value));
				}
			}

			// 支持发光涂料
			Color tileLight;
			if (color != default)
			{
				tileLight = color;
			}
			else
			{
				tileLight = Lighting.GetColor((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f));
			}

			Vector2 toNextMass;
			if (i < masses.Length - 1)
			{
				Mass nextMass = masses[i + 1];
				toNextMass = nextMass.Position - thisMass.Position;
			}
			else
			{
				Mass passedMass = masses[i - 1];
				toNextMass = thisMass.Position - passedMass.Position;
			}
			Vector2 drawPos = thisMass.Position - Main.screenPosition;
			if (i < masses.Length - 1)
			{
				spriteBatch.Draw(tex, drawPos, new Rectangle(8 * (i % 4), 0, 8, 10), tileLight, toNextMass.ToRotation() - MathHelper.PiOver2, new Vector2(4f, 0), 1f, SpriteEffects.None, 0);
			}
			else
			{
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, 12, 32, 40), tileLight, toNextMass.ToRotation() - MathHelper.PiOver2, new Vector2(16f, 0), 1f, SpriteEffects.None, 0);
				Lighting.AddLight(drawPos + Main.screenPosition, new Vector3(0.8f, 0.8f, 0.2f));
			}
		}
	}

	/// <summary>
	/// Try to get the cable entity bound at (<paramref name="i"/>, <paramref name="j"/>).
	/// </summary>
	/// <typeparam name="T">The type to get the entity as</typeparam>
	/// <param name="i">The tile X-coordinate</param>
	/// <param name="j">The tile Y-coordinate</param>
	/// <param name="entity">The found <typeparamref name="T"/> instance, if there was one.</param>
	/// <returns><see langword="true"/> if there was a <typeparamref name="T"/> instance, or <see langword="false"/> if there was no entity present OR the entity was not a <typeparamref name="T"/> instance.</returns>
	public static bool TryGetCableEntityAs<T>(int i, int j, out T entity)
		where T : TileEntity
	{
		Point16 origin = new Point16(i, j);

		// TileEntity.ByPosition is a Dictionary<Point16, TileEntity> which contains all placed TileEntity instances in the world
		// TryGetValue is used to both check if the dictionary has the key, origin, and get the value from that key if it's there
		if (TileEntity.ByPosition.TryGetValue(origin, out TileEntity existing) && existing is T existingAsT)
		{
			entity = existingAsT;
			return true;
		}

		entity = null;
		return false;
	}

	/// <summary>
	/// 鼠标划过点位
	/// </summary>
	public Dictionary<Player, Point> MouseOverPoint = new Dictionary<Player, Point>();

	public override void MouseOver(int i, int j)
	{
		if(LengthAdjustable)
		{
			if (!MouseOverPoint.ContainsKey(Main.LocalPlayer))
			{
				MouseOverPoint.Add(Main.LocalPlayer, new Point(i, j));
				if (Main.LocalPlayer.HeldItem.createTile == Type)
				{
					HangingTileLengthAdjustingSystem vfx = new HangingTileLengthAdjustingSystem { FixPoint = new Point(i, j), Active = true, Visible = true, Style = 0 };
					Ins.VFXManager.Add(vfx);
				}
			}
			else if (MouseOverPoint[Main.LocalPlayer] != new Point(i, j))
			{
				MouseOverPoint.Remove(Main.LocalPlayer);
			}
		}
	}

	/// <summary>
	/// Right click to adjust cable length.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	public override bool RightClick(int i, int j)
	{
		if (LengthAdjustable)
		{
			// 旋转
			Tile tile = Main.tile[i, j];
			if (Main.LocalPlayer.HeldItem.createTile == Main.tile[i, j].TileType && !ChainPlayer.ContainsKey(new Point(i, j)))
			{
				HangingTileLengthAdjustingSystem vfx = new HangingTileLengthAdjustingSystem { FixPoint = new Point(i, j), Active = true, Visible = true, Style = 1, StartFrameY60 = tile.TileFrameY * 60 };
				Ins.VFXManager.Add(vfx);
				SoundEngine.PlaySound(SoundID.Item17, new Vector2(i, j) * 16);
				ChainPlayer.Add(new Point(i, j), Main.LocalPlayer);
			}
		}
		return base.RightClick(i, j);
	}

	public override void PlaceInWorld(int i, int j, Item item)
	{
		Tile tile = Main.tile[i, j];
		tile.TileFrameY = 18;
	}
}

[Pipeline(typeof(WCSPipeline))]
public class HangingTileLengthAdjustingSystem : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public Texture2D Texture;
	public Point FixPoint;
	public int Style;
	public float StartFrameY60;
	public Vector2 StartRotation;
	public float AccumulateRotation;
	public Vector2 OldRotation;

	public override void OnSpawn()
	{
		StartRotation = Utils.SafeNormalize(FixPoint.ToWorldCoordinates() - Main.MouseWorld, new Vector2(0, -1));
		OldRotation = StartRotation;
		AccumulateRotation = 0;
		Texture = ModAsset.TileBlock.Value;
	}

	public override void Update()
	{
		Player player = Main.LocalPlayer;
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
		Tile tile = Main.tile[i, j];
		HangingTile hangingTile = TileLoader.GetTile(tile.type) as HangingTile;
		if (hangingTile == null)
		{
			Active = false;
			return;
		}
		if (hangingTile.ChainPlayer.ContainsKey(FixPoint))
		{
			Player player2;
			hangingTile.ChainPlayer.TryGetValue(FixPoint, out player2);
			if (player2 != player)
			{
				KillMe(hangingTile, player);
				return;
			}
		}
		if (player.HeldItem.createTile != tile.TileType)
		{
			KillMe(hangingTile, player);
			return;
		}
		if ((Main.MouseWorld - FixPoint.ToWorldCoordinates()).Length() > 400)
		{
			KillMe(hangingTile, player);
			return;
		}
		if (Style == 0)
		{
			int x = (int)(Main.MouseWorld.X / 16f);
			int y = (int)(Main.MouseWorld.Y / 16f);
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
				KillMe(hangingTile, player);
				return;
			}
			if (!hangingTile.ChainPlayer.ContainsKey(FixPoint))
			{
				KillMe(hangingTile, player);
				return;
			}
			float addAccRot = MathF.Asin(-Vector3.Cross(new Vector3(Utils.SafeNormalize(FixPoint.ToWorldCoordinates() - Main.MouseWorld, new Vector2(0, -1)), 0), new Vector3(OldRotation, 0)).Z);
			AccumulateRotation += addAccRot;
			OldRotation = Utils.SafeNormalize(FixPoint.ToWorldCoordinates() - Main.MouseWorld, new Vector2(0, -1));
			float nowFrameY = StartFrameY60 / 60f + AccumulateRotation * 2;
			if (nowFrameY < 1)
			{
				AccumulateRotation -= addAccRot;
			}
			if (nowFrameY > hangingTile.MaxCableLength - 1)
			{
				AccumulateRotation -= addAccRot;
			}
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

			int x = (int)(Main.MouseWorld.X / 16f);
			int y = (int)(Main.MouseWorld.Y / 16f);
			if (Main.mouseRight && Main.mouseRightRelease && (x != FixPoint.X || y != FixPoint.Y))
			{
				KillMe(hangingTile, player);
				return;
			}
		}
		base.Update();
	}

	public void KillMe(HangingTile hangingTile, Player owner)
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
			float nowFrameY = StartFrameY60 / 60f + AccumulateRotation * 2;
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
		Ins.Batch.BindTexture<Vertex2D>(Texture);

		Vector2 rotCenter = FixPoint.ToWorldCoordinates();
		Vector2 cut = -Utils.SafeNormalize(rotCenter - Main.MouseWorld, new Vector2(0, -1));
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
					float thisFrameY = StartFrameY60 / 60f + (AccumulateRotation + k / 20f * MathHelper.TwoPi) * 2;
					if (thisFrameY < 5)
					{
						newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), (5 - thisFrameY) / 4f);
					}
					if (thisFrameY > hangingTile.MaxCableLength - 5)
					{
						newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), (thisFrameY - (hangingTile.MaxCableLength - 5)) / 4f);
					}
					DrawLine(rotCenter + cut2 * 15, rotCenter + cut2 * 20, 1.5f, newDrawColor);
				}
				Vector2 cut3 = new Vector2(0, -1).RotatedBy(maxK / 20f * MathHelper.TwoPi);
				DrawLine(rotCenter + cut3 * 12, rotCenter + cut3 * 26, 2, drawColor);
				if ((Main.MouseWorld - rotCenter).Length() > 240)
				{
					for (int k = -10; k < 10; k++)
					{
						Vector2 cut4 = cut.RotatedBy((k - 0.5f) * 0.04f);
						Vector2 cut5 = cut.RotatedBy((k + 0.5f) * 0.04f);
						float colorValue = ((Main.MouseWorld - rotCenter).Length() - 240f) / 160f;
						Color newDrawColor = origDrawColor;
						newDrawColor = Color.Lerp(origDrawColor, new Color(1f, 0f, 0f, 0.8f), colorValue);
						colorValue *= (100f - k * k) / 100f;
						DrawLine(rotCenter + cut4 * 390f, rotCenter + cut5 * 390f, 2f, newDrawColor * colorValue * 2);
					}
					Main.instance.MouseText("Drag out of circle to cancel", ItemRarityID.White);
				}
			}
		}
		DrawBlockBound(FixPoint.X, FixPoint.Y, drawColor, AccumulateRotation);
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

		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}

	public void DrawLine(Vector2 pos1, Vector2 pos2, float width, Color color)
	{
		Vector2 normal = Utils.SafeNormalize(pos1 - pos2, Vector2.zeroVector).RotatedBy(MathHelper.PiOver2) * width / 2f;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(pos1 + normal, color, new Vector3(0, 0, 0)),
			new Vertex2D(pos2 + normal, color, new Vector3(0.1f, 0, 0)),
			new Vertex2D(pos1 - normal, color, new Vector3(0, 1, 0)),

			new Vertex2D(pos1 - normal, color, new Vector3(0, 1, 0)),
			new Vertex2D(pos2 + normal, color, new Vector3(0.1f, 0, 0)),
			new Vertex2D(pos2 - normal, color, new Vector3(0.1f, 1, 0)),
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}

public class HangingTileUpdate : ModSystem
{
	/// <summary>
	/// 物块质点系统
	/// </summary>
	public static MassSpringSystem HangingTileMassSpringSystem = new MassSpringSystem();
	public static EulerSolver HangingTileEulerSolver = new EulerSolver(8);
	public static PBDSolver HangingTilePBDSolver = new PBDSolver(8);

	public override void PostUpdateEverything()
	{
		HangingTileMassSpringSystem = new MassSpringSystem();
		foreach (var HangingTile in TileLoader.tiles.OfType<HangingTile>())
		{
			foreach (var rope in HangingTile.RopesOfAllThisTileInTheWorld.Values)
			{
				HangingTileMassSpringSystem.AddMassSpringMesh(rope);
			}
		}
		HangingTileEulerSolver.Step(HangingTileMassSpringSystem, 1);
	}

	public override void OnWorldLoad()
	{
		foreach (var HangingTile in TileLoader.tiles.OfType<HangingTile>())
		{
			HangingTile.RopesOfAllThisTileInTheWorld.Clear();
		}
		base.OnWorldLoad();
	}

	public override void OnWorldUnload()
	{
		foreach (var HangingTile in TileLoader.tiles.OfType<HangingTile>())
		{
			HangingTile.RopesOfAllThisTileInTheWorld.Clear();
		}
		base.OnWorldUnload();
	}
}
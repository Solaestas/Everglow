using Everglow.Commons.Physics.MassSpringSystem;
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

	/// <summary>
	/// 重写该方法来重新对HangingTile属性赋值，
	/// 默认值：
	/// MaxCableLength = 60;
	/// SingleLampMass = 8;
	/// RopeUnitMass = 0.5f;
	/// MaxWireStyle = 2;
	/// Elasticity = 150;
	/// LengthAdjustable = true;
	/// 重写 SetStaticDefaults()时，请在其中调用我。
	/// </summary>
	public virtual void InitHanging()
	{
	}

	public override void SetStaticDefaults()
	{
		InitHanging();
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
	/// Current Player who is handling with the HangingTile at Point.
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
	/// Paint the hanging chains.
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
	/// A mousePos-player Dictionary to prevent generating multiple VFX.
	/// </summary>
	public Dictionary<Player, Point> MouseOverPoint = new Dictionary<Player, Point>();

	public override void MouseOver(int i, int j)
	{
		if (LengthAdjustable)
		{
			if (!MouseOverPoint.ContainsKey(Main.LocalPlayer))
			{
				MouseOverPoint.Add(Main.LocalPlayer, new Point(i, j));
				if (Main.LocalPlayer.HeldItem.createTile == Type)
				{
					HangingTile_LengthAdjustingSystem vfx = new HangingTile_LengthAdjustingSystem { FixPoint = new Point(i, j), Active = true, Visible = true, Style = 0 };
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
				HangingTile_LengthAdjustingSystem vfx = new HangingTile_LengthAdjustingSystem { FixPoint = new Point(i, j), Active = true, Visible = true, Style = 1, StartFrameY60 = tile.TileFrameY * 60 };
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
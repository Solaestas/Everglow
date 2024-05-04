using Everglow.Commons.Enums;
using Everglow.Commons.Physics;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static Everglow.Commons.Physics.Rope;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// 缆绳物块,可用于制作彩灯,彩带等
/// </summary>
public abstract class CableTile : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.BlocksWaterDrawingBehindSelf[Type] = true;

		// MyTileEntity refers to the tile entity mentioned in the previous section
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<CableEneity>().Hook_AfterPlacement, -1, 0, true);

		// This is required so the hook is actually called.
		TileObjectData.newTile.UsesCustomCanPlace = true;

		AddMapEntry(new Color(151, 31, 32));
	}

	/// <summary>
	/// 绳头位置 绳子
	/// </summary>
	public Dictionary<Point, Rope> RopesOfAllThisTileInTheWorld = new Dictionary<Point, Rope>();

	public override void HitWire(int i, int j)
	{
	}

	/// <summary>
	/// 玩家拉绳点位
	/// </summary>
	public Dictionary<Player, Point> HasHoldRope = new Dictionary<Player, Point>();

	/// <summary>
	/// 右键效果。不拿绳拆绳，拿绳拉绳，拉了绳挂绳
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	public override bool RightClick(int i, int j)
	{
		if (HasHoldRope.ContainsKey(Main.LocalPlayer))
		{
			Point point = HasHoldRope[Main.LocalPlayer];
			if (i != point.X || j != point.Y)
			{
				if (new Vector2(point.X - i, point.Y - j).Length() * 16 < 600)
				{
					AddRope(i, j, ConnectRope(i, j, point.X, point.Y));
				}
			}
			HasHoldRope.Remove(Main.LocalPlayer);
			return base.RightClick(i, j);
		}
		if (Main.LocalPlayer.HeldItem.ModItem is CableTileItem)
		{
			HasHoldRope.Add(Main.LocalPlayer, new Point(i, j));
			CableTilePlaceHelpingSystem vfx = new CableTilePlaceHelpingSystem { FixPoint = new Point(i, j), Active = true, Visible = true, Style = 1 };
			Ins.VFXManager.Add(vfx);
			return base.RightClick(i, j);
		}
		RemoveAllRope(i, j);
		return base.RightClick(i, j);
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		RemoveAllRope(i, j);

		// ModTileEntity.Kill() handles checking if the tile entity exists and destroying it if it does exist in the world for you
		// The tile coordinate parameters already refer to the top-left corner of the multitile
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
	}

	/// <summary>
	/// 移除挂在这个点上所有的绳子无论头尾
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public virtual void RemoveAllRope(int i, int j)
	{
		RemoveRopeHeadAt(i, j);
		foreach (Rope rope in RopesOfAllThisTileInTheWorld.Values)
		{
			if ((rope.GetMassList.Last().Position - new Vector2(i + 0.5f, j + 0.5f) * 16f).Length() < 4)
			{
				foreach (Point point in RopesOfAllThisTileInTheWorld.Keys)
				{
					Rope tryRope;
					RopesOfAllThisTileInTheWorld.TryGetValue(point, out tryRope);
					if (tryRope == rope)
					{
						RemoveRopeEffect(rope);
						RopesOfAllThisTileInTheWorld.Remove(point);
						ModContent.GetInstance<CableEneity>().Kill(point.X, point.Y);
						break;
					}
				}
			}
		}
	}

	/// <summary>
	/// 移除此坐标为绳头的绳子
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public virtual void RemoveRopeHeadAt(int i, int j)
	{
		if (RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			Rope rope;
			RopesOfAllThisTileInTheWorld.TryGetValue(new Point(i, j), out rope);
			RemoveRopeEffect(rope);
			RopesOfAllThisTileInTheWorld.Remove(new Point(i, j));
			ModContent.GetInstance<CableEneity>().Kill(i, j);
		}
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
			CableEneity cableEneity;
			TryGetCableEntityAs<CableEneity>(i, j, out cableEneity);
			if (cableEneity != null)
			{
				AddRope(i, j, ConnectRope(i, j, i + cableEneity.ToTail.X, j + cableEneity.ToTail.Y));
			}
		}
		base.NearbyEffects(i, j, closer);
	}

	/// <summary>
	/// 掐掉绳子的视觉效果
	/// </summary>
	/// <param name="rope"></param>
	public virtual void RemoveRopeEffect(Rope rope)
	{
		for (int i = 0; i <= rope.GetMassList.Length - 1; i++)
		{
			_Mass thisMass = rope.GetMassList[i];
			Dust d0 = Dust.NewDustDirect(thisMass.Position - new Vector2(4), 0, 0, DustID.Asphalt);
			d0.noGravity = true;
			d0.velocity *= 0f;
			d0.scale = 0.8f;
			if (thisMass.Mass == 2)
			{
				for (int t = 0; t < 3; t++)
				{
					Dust d = Dust.NewDustDirect(thisMass.Position + new Vector2(0, 5) - new Vector2(4), 0, 0, DustID.Electric);
					d.velocity = new Vector2(0, Main.rand.NextFloat(0f, 3.5f)).RotatedByRandom(MathHelper.TwoPi);
					d.scale = Main.rand.NextFloat(0.1f, 0.3f);
				}
			}
			if (i == 0 || i == rope.GetMassList.Length - 1)
			{
				for (int t = 0; t < 5; t++)
				{
					Dust d = Dust.NewDustDirect(thisMass.Position + new Vector2(0, 5) - new Vector2(4), 0, 0, DustID.Electric);
					d.velocity = new Vector2(0, Main.rand.NextFloat(0.5f, 6.5f)).RotatedByRandom(MathHelper.TwoPi);
					d.scale = Main.rand.NextFloat(0.3f, 0.7f);
				}
			}
		}
	}

	/// <summary>
	/// 挂上绳子
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="rope"></param>
	public virtual void AddRope(int i, int j, Rope rope)
	{
		if (rope == null)
		{
			return;
		}
		if (RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			return;
		}
		RopesOfAllThisTileInTheWorld.Add(new Point(i, j), rope);

		CableEneity cableEneity;
		TryGetCableEntityAs<CableEneity>(i, j, out cableEneity);
		if (cableEneity == null)
		{
			TileEntity.PlaceEntityNet(i, j, ModContent.TileEntityType<CableEneity>());
			TryGetCableEntityAs<CableEneity>(i, j, out cableEneity);
		}
		if (cableEneity != null)
		{
			Vector2 tail = rope.GetMassList.Last().Position / 16f;
			Point toTail = tail.ToPoint() - new Point(i, j);
			cableEneity.ToTail = toTail;
		}
	}

	/// <summary>
	/// 在附近遍历物块链接绳子
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	public virtual Rope FindSameAndCreateRope(int i, int j)
	{
		for (int x = -30; x < 31; x++)
		{
			for (int y = -30; y < 31; y++)
			{
				Tile tile = Main.tile[i + x, j + y];
				if (tile.TileType == Type && !(x == 0 && y == 0))
				{
					int counts = (int)new Vector2(x, y).Length() * 4;

					Rope rope = new Rope(new Vector2(i, j) * 16 + new Vector2(8), new Vector2(i + x, j + y) * 16 + new Vector2(8), counts, 7, 0.05f, (Vector2) => Vector2.Zero, true, 16, 2);

					return rope;
				}
			}
		}
		return null;
	}

	/// <summary>
	/// 两点式挂绳
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="i2"></param>
	/// <param name="j2"></param>
	/// <returns></returns>
	public virtual Rope ConnectRope(int i, int j, int i2, int j2)
	{
		Tile tile = Main.tile[i2, j2];

		// 挂在自己身上报错
		if (i == i2 && j == j2)
		{
			CombatText.NewText(new Rectangle(i * 16, j * 16, 8, 8), Color.Red, "You can't connect rope in a same tile!");
			return null;
		}

		// 只有同一种块之间才能连绳
		if (tile.TileType == Type)
		{
			int counts = (int)new Vector2(i2 - i, j2 - j).Length() * 2;
			Rope rope = new Rope(new Vector2(i, j) * 16 + new Vector2(8), new Vector2(i2, j2) * 16 + new Vector2(8), counts, 7, 0.05f, (Vector2) => Vector2.Zero, true, 8, 2);
			return rope;
		}
		return null;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return base.PreDraw(i, j, spriteBatch);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		TileFluentDrawManager.AddFluentPoint(this, i, j);
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		Rope rope;
		RopesOfAllThisTileInTheWorld.TryGetValue(pos, out rope);
		if (rope != null)
		{
			if (!Main.gamePaused)
			{
				rope.Update(1);
			}
			DrawCable(rope, pos, spriteBatch, tileDrawing);
		}
	}

	public string BulbTexturePath;

	/// <summary>
	/// 绘制挂绳
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
		var tileSpriteEffect = SpriteEffects.None;
		for (int i = 0; i < rope.GetMassList.Length - 1; i++)
		{
			_Mass thisMass = rope.GetMassList[i];
			_Mass nextMass = rope.GetMassList[i + 1];

			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1))
			{
				windCycle = tileDrawing.GetWindCycle((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), tileDrawing._sunflowerWindCounter);
			}

			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			float rotation = -windCycle * 0.4f;
			if (!Main.gamePaused)
			{
				rope.ApplyForceSpecial(i, new Vector2(windCycle * 1, 4 * thisMass.Mass));
			}

			// 支持发光涂料
			Color tileLight;
			if (color != default(Color))
			{
				tileLight = color;
			}
			else
			{
				tileLight = Lighting.GetColor((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f)) * 10;
			}
			tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(pos.X, pos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
			tileLight = tileDrawing.DrawTiles_GetLightOverride(pos.X, pos.Y, tile, type, 0, 0, tileLight);
			Vector2 toNextMass = nextMass.Position - thisMass.Position;
			Vector2 drawPos = thisMass.Position - Main.screenPosition;
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, 2, 2, 2), tileLight, toNextMass.ToRotation(), new Vector2(1f), new Vector2(toNextMass.Length() / 2f, 1), tileSpriteEffect, 0);
			if (thisMass.Mass == 2)
			{
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, 6, 10, 18), tileLight, rotation, new Vector2(5f, 0), 1f, tileSpriteEffect, 0);
				Lighting.AddLight(thisMass.Position, new Vector3(0.7f, 0.4f, 0.3f));
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
		if (!MouseOverPoint.ContainsKey(Main.LocalPlayer))
		{
			MouseOverPoint.Add(Main.LocalPlayer, new Point(i, j));
			if (Main.LocalPlayer.HeldItem.ModItem is CableTileItem)
			{
				CableTilePlaceHelpingSystem vfx = new CableTilePlaceHelpingSystem { FixPoint = new Point(i, j), Active = true, Visible = true, Style = 0 };
				Ins.VFXManager.Add(vfx);
			}
		}
		else if(MouseOverPoint[Main.LocalPlayer] != new Point(i, j))
		{
			MouseOverPoint.Remove(Main.LocalPlayer);
		}
	}
}

public class CableEneity : ModTileEntity
{
	public Point ToTail;

	public override bool IsTileValidForEntity(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (TileLoader.GetTile(tile.TileType) is CableTile)
		{
			return tile.HasTile;
		}
		return false;
	}

	public override void OnNetPlace()
	{
		if (Main.netMode == NetmodeID.Server)
		{
			NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
		}
	}

	public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
	{
		if (Main.netMode == NetmodeID.MultiplayerClient)
		{
			// Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
			int width = 1;
			int height = 1;
			NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);

			// Sync the placement of the tile entity with other clients
			// The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
			NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
			return -1;
		}

		// ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
		int placedEntity = Place(i, j);
		return placedEntity;
	}

	public override void SaveData(TagCompound tag)
	{
		tag.Add("CableToTailX", ToTail.X);
		tag.Add("CableToTailY", ToTail.Y);
		base.SaveData(tag);
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		tag.TryGet<int>("CableToTailX", out ToTail.X);
		tag.TryGet<int>("CableToTailY", out ToTail.Y);
	}
}

[Pipeline(typeof(WCSPipeline))]
public class CableTilePlaceHelpingSystem : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public Texture2D Texture;
	public Point FixPoint;
	public int Style;

	public override void OnSpawn()
	{
		Texture = ModAsset.TileBlock.Value;
	}

	public override void Update()
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
		CableTile cableTile = TileLoader.GetTile(Main.tile[i, j].type) as CableTile;
		if (cableTile == null)
		{
			Active = false;
			return;
		}
		if (Style == 1)
		{
			if (!cableTile.HasHoldRope.ContainsKey(Main.LocalPlayer))
			{
				Active = false;
				return;
			}
		}
		if (Style == 0)
		{
			if (cableTile.HasHoldRope.ContainsKey(Main.LocalPlayer))
			{
				Active = false;
				if (cableTile.MouseOverPoint.ContainsKey(Main.LocalPlayer))
				{
					cableTile.MouseOverPoint.Remove(Main.LocalPlayer);
				}
				return;
			}
			int x = (int)(Main.MouseWorld.X / 16f);
			int y = (int)(Main.MouseWorld.Y / 16f);
			if (x != FixPoint.X || y != FixPoint.Y)
			{
				Active = false;
				if (cableTile.MouseOverPoint.ContainsKey(Main.LocalPlayer))
				{
					cableTile.MouseOverPoint.Remove(Main.LocalPlayer);
				}
				return;
			}
		}
		base.Update();
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
		CableTile cableTile = TileLoader.GetTile(Main.tile[i, j].type) as CableTile;
		if (cableTile == null)
		{
			Active = false;
			return;
		}
		Color drawColor = Color.Lerp(new Color(0.75f, 0.75f, 1f, 0.5f), new Color(0.85f, 0.85f, 0.75f, 0.5f), MathF.Sin((float)Main.timeForVisualEffects * 0.08f) * 0.5f + 0.5f);
		Ins.Batch.BindTexture<Vertex2D>(Texture);
		Vector2 fixPos = FixPoint.ToVector2() * 16f;

		Player player = Main.LocalPlayer;

		// 类型1 ： 牵线后
		if (Style == 1)
		{
			if (cableTile.HasHoldRope.ContainsKey(player))
			{
				if (player.HeldItem.ModItem is not CableTileItem)
				{
					cableTile.HasHoldRope.Remove(player);
					Active = false;
					return;
				}
				if (cableTile.HasHoldRope[player] == new Point(i, j))
				{
					// 校正鼠标坐标
					Vector2 toTarget = new Vector2(i, j) * 16 + new Vector2(8) - Main.MouseWorld;

					// 超过900距离取消选中
					if (toTarget.Length() > 1500)
					{
						cableTile.HasHoldRope.Remove(player);
						Active = false;
						return;
					}
					if (Main.mouseRight && Main.mouseRightRelease && toTarget.Length() > 8 * MathF.Sqrt(2))
					{
						cableTile.HasHoldRope.Remove(player);
						Active = false;
						return;
					}

					int x = (int)(Main.MouseWorld.X / 16f);
					int y = (int)(Main.MouseWorld.Y / 16f);

					// 超过600距离标红
					if (new Vector2(i - x, j - y).Length() * 16f > 600)
					{
						toTarget = Vector2.Normalize(toTarget) * 600.0001f;
						drawColor = new Color(1f, 0, 0, 0.3f);

						Vector2 destination = fixPos + new Vector2(8) - toTarget;
						DrawLine(destination + new Vector2(10, 10), destination - new Vector2(10, 10), 3, drawColor);
						DrawLine(destination + new Vector2(-10, 10), destination - new Vector2(-10, 10), 3, drawColor);
					}

					bool canDrawDestination = false;

					// 绘制选中块
					if (player.IsInTileInteractionRange(x, y, TileReachCheckSettings.Simple))
					{
						if (x > 20 && x < Main.maxTilesX - 20)
						{
							if (y > 20 && y < Main.maxTilesY - 20)
							{
								int type = Main.tile[x, y].TileType;
								ModTile modTile0 = TileLoader.GetTile(type);
								if (modTile0 is CableTile)
								{
									if (new Vector2(i - x, j - y).Length() * 16f <= 600)
									{
										drawColor = new Color(1f, 0.9f, 0, 0.4f);

										// 试图连接到自己标红
										if (i == x && j == y)
										{
											drawColor = new Color(1f, 0, 0, 0.5f);
										}

										// 已经被占据块标红
										else if (cableTile.RopesOfAllThisTileInTheWorld.ContainsKey(new Point(x, y)))
										{
											drawColor = new Color(1f, 0, 0, 0.5f);
											Vector2 anotherPoint = cableTile.RopesOfAllThisTileInTheWorld[new Point(x, y)].GetMassList.Last().Position;
											Vector2 start0 = anotherPoint;
											Vector2 end0 = fixPos + new Vector2(8) - toTarget;

											Vector2 direction0 = Utils.SafeNormalize(start0 - end0, Vector2.zeroVector) * 16;
											DrawLine(end0, end0 + direction0.RotatedBy(0.4), 3, drawColor);
											DrawLine(end0, end0 + direction0.RotatedBy(-0.4), 3, drawColor);

											DrawLine(anotherPoint, fixPos + new Vector2(8) - toTarget, 3, drawColor);
											DrawBlockBound((int)((anotherPoint.X - 8) / 16f), (int)((anotherPoint.Y - 8) / 16f), drawColor);
										}

										canDrawDestination = true;
									}
								}
							}
						}
					}

					// 正常绘制
					Vector2 start = fixPos + new Vector2(8);
					Vector2 end = start - toTarget;

					Vector2 direction = Utils.SafeNormalize(start - end, Vector2.zeroVector) * 16;
					if (new Vector2(i - x, j - y).Length() * 16f <= 600)
					{
						DrawLine(end, end + direction.RotatedBy(0.4), 3, drawColor);
						DrawLine(end, end + direction.RotatedBy(-0.4), 3, drawColor);
					}

					DrawLine(start, end, 3, drawColor);
					if (canDrawDestination)
					{
						// 绘制
						DrawBlockBound(x, y, drawColor);
					}
				}
			}
		}
		DrawBlockBound(FixPoint.X, FixPoint.Y, drawColor);
	}

	public void DrawBlockBound(int i, int j, Color color)
	{
		Vector2 pos = new Vector2(i, j) * 16;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(pos, color, new Vector3(0, 0, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),

			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(16), color, new Vector3(1, 1, 0)),
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
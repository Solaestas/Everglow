using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

internal class LargeFireBulbTestItem : ModItem
{
	public override string Texture => "Everglow/" + ModAsset.Tiles_LargeFireBulbPath;

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.maxStack = Item.CommonMaxStack;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.width = 30;
		Item.height = 30;
	}

	// 在多人模式不可用，不过这个是放置测试物品，就不适配了
	public override bool? UseItem(Player player)
	{
		if (player.ItemAnimationJustStarted)
		{
			var tileCoords = Main.MouseWorld.ToTileCoordinates();
			return LargeFireBulb.PlaceMe(tileCoords.X, tileCoords.Y, 0);
		}
		return true;
	}
}

internal class LargeFireBulb : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
		TileObjectData.newTile.Height = 4;
		TileObjectData.newTile.CoordinateHeights = new int[4] {
			16,
			16,
			16,
			16
		};

		TileObjectData.newTile.StyleWrapLimit = 111;
		TileObjectData.newTile.DrawYOffset = -6;
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(28, 132, 255), name);
	}

	/// <summary>
	/// 要用这个方法来放置LargeFireBulb到指定位置，这样可以保证生成出来就有根、果实上部、果实下部三个部分 <br/>
	/// 注意，不可在多人模式客户端调用
	/// </summary>
	/// <param name="x">根左上角的x坐标</param>
	/// <param name="y">根左上角的y坐标</param>
	/// <param name="stemLength">茎长，花有四个部分: 根、茎、果实上部、果实下部。其中茎可以没有，因此stemLength最小为0</param>
	/// <param name="broadcast">在服务器是否同步物块信息到所有客户端</param>
	/// <returns>放置是否成功</returns>
	public static bool PlaceMe(int x, int y, ushort stemLength = 0, bool broadcast = true)
	{
		if (Main.netMode is NetmodeID.MultiplayerClient)
			return false;

		var modTile = TileLoader.GetTile(ModContent.TileType<LargeFireBulb>()) as LargeFireBulb;

		// 根
		bool succeed = modTile.TryGrow(x, y, broadcast);
		if (!succeed)
			return false;

		// 茎
		int height = 4;
		for (int k = 0; k < stemLength; k++)
		{
			succeed &= modTile.TryGrow(x, y + height, broadcast);
			if (!succeed)
				return false;
			height += 4;
		}

		// 果实1
		succeed &= modTile.TryGrow(x, y + height, broadcast);
		height += 4;
		if (!succeed)
			return false;

		// 果实2
		succeed &= modTile.TryGrow(x, y + height, broadcast);
		return succeed;
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		var tile = Main.tile[i, j];
		int lx = i - tile.TileFrameX / 18;
		int ly = j - tile.TileFrameY / 18;

		noBreak = true;

		// 保证当前节结构完整
		for (int m = 0; m < 2; m++)
		{
			for (int n = 0; n < 4; n++)
			{
				noBreak &= HasSameTileAt(lx + m, ly + n);
			}
		}

		// 保证上方有连接物
		noBreak &= Main.tile[lx, ly - 1].HasTile;

		// 爬升到根
		int y = ly;
		while (y > 20 && HasSameTileAt(lx, y - 1))
		{
			y--;
		}
		// 保证至少有三节，否则绘制会出问题
		noBreak &= HasSameTileAt(lx, y + 4) && HasSameTileAt(lx, y + 8) && Main.tile[lx, y - 1].HasTile && Main.tile[lx + 1, y - 1].HasTile;

		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public override void RandomUpdate(int i, int j)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX != 0 || tile.TileFrameY != 0)
			return;

		int deltaY = 0;
		while (Main.tile[i, j - 4 - deltaY].TileType == Type)
		{
			deltaY += 4;
			if (deltaY > j - 1)
			{
				break;
			}
		}
		if (deltaY > 8 * 4 + Math.Sin(i + j) * 3)
		{
			return;
		}
		if (Main.rand.NextBool(Math.Max(1, deltaY * deltaY / 16)))
		{
			TryGrow(i, j + 4);
		}
		base.RandomUpdate(i, j);
	}

	private bool TryGrow(int x, int y, bool broadcast = true)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				var coords = new Point(x + i, y + j);
				var tile = Main.tile[coords];
				if (tile.HasTile)
					return false;
			}
		}

		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				var coords = new Point(x + i, y + j);
				var tile = Main.tile[coords];
				tile.TileType = Type;
				tile.HasTile = true;
				tile.TileFrameX = (short)(i * 18);
				tile.TileFrameY = (short)(j * 18);
			}
		}

		if (broadcast && Main.netMode is NetmodeID.Server)
			NetMessage.SendTileSquare(-1, x, y, 2, 4);

		return true;
	}

	private bool HasSameTileAt(int i, int j)
	{
		if (!WorldGen.InWorld(i, j, 10))
			return false;
		var tile = Main.tile[i, j];
		return tile != null && tile.HasTile && tile.type == Type;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX != 0 || tile.TileFrameY != 0)
			return false;

		Vector2 offscreenVector = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
		{
			offscreenVector = Vector2.Zero;
		}

		if (Main.LocalPlayer.HeldItem is not null && Main.LocalPlayer.HeldItem.type == ModContent.ItemType<LargeFireBulbTestItem>())
		{
			var hitboxPos = new Vector2(i, j) * 16f - Main.screenPosition + offscreenVector;
			var hitbox = new Rectangle((int)hitboxPos.X, (int)hitboxPos.Y, 32, 64);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, hitbox, Color.Orange * 0.4f);
		}

		var tileCoords = new Point(i + 1, j);
		var tileData = TileObjectData.GetTileData(Type, 0);
		var position = tileCoords.ToWorldCoordinates(0, 0) - Main.screenPosition + offscreenVector;
		position.X -= 40f;
		position.X += tileData.DrawXOffset;
		position.Y += tileData.DrawYOffset;

		var tex = ModAsset.LargeFireBulb_Root.Value;
		Texture2D glow = null;
		if (HasSameTileAt(i, j - 4)) // 上面有同样物块，不是根
		{
			if (!HasSameTileAt(i, j + 4)) // 下面没有同样物块，是果实2
			{
				tex = ModAsset.LargeFireBulb_Fruit2.Value;
				glow = ModAsset.LargeFireBulb_Fruit2_Glow.Value;
			}
			else if (!HasSameTileAt(i, j + 8)) // 下面有同样物块，但下面第二格有，是果实1
			{
				tex = ModAsset.LargeFireBulb_Fruit1.Value;
				glow = ModAsset.LargeFireBulb_Fruit1_Glow.Value;
			}
			else // 下面有同样物块，下面第二格也有，是茎
			{
				tex = ModAsset.LargeFireBulb_Stem.Value;
			}
		}

		Color tileLight = Lighting.GetColor(tileCoords);
		spriteBatch.Draw(tex, position, tileLight);
		var glowColor = new Color(255, 255, 255, 0);
		if (glow != null)
			spriteBatch.Draw(glow, position, glowColor);

		return false;
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		if (HasSameTileAt(i, j - 4)) // 上面有同样物块，不是根
		{
			if (!HasSameTileAt(i, j + 4)) // 下面没有同样物块，是果实2
			{
				r = 0;
				g = 1;
				b = 1;
			}
			else if (!HasSameTileAt(i, j + 8)) // 下面有同样物块，但下面第二格有，是果实1
			{
				r = 0;
				g = 1;
				b = 1;
			}
			else // 下面有同样物块，下面第二格也有，是茎
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
	}
}
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Localization;

namespace Everglow.Commons.Utilities;

public static class FurnitureUtils
{
	/// <summary>
	/// 用于宽度为1的Lantern的FluentDraw，用于带摆动绘制，适配油漆 <br/>
	/// 也可以用于旗帜，原版旗帜的摆动绘制和挂灯其实是有区别的，将风速调至最大对比一下就会发现，所以加了isBanner参数
	/// </summary>
	public static void LanternFluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, int sizeY = 2, bool isBanner = false)
	{
		var tile = Main.tile[pos];

		if (!tileDrawing.IsVisible(tile)) return;

		// 这就是油漆的奥秘，大概就是根据你给出的pos上的油漆给你这个物块的贴图上shader
		// 你可以把里面的代码抄来，把方法里面那个贴图改成一个参数，就可以做到给任意贴图上相应世界物块上的漆了
		Texture2D tex = tileDrawing.GetTileDrawTexture(tile, pos.X, pos.Y);
		
		short tileFrameX = tile.frameX;
		short tileFrameY = tile.frameY;

		int layer = tileFrameY / 18; // 这格物块处于整体的第几层（最上面的是第零层，往下递增）

		// 用于风速、推力等一系列判定的物块坐标，通常来说是挂在墙上的那一格
		int topTileX = pos.X;
		int topTileY = pos.Y - layer;

		int offsetX = 8;
		int offsetY = -2 - layer * 16;
		// 锤子是这样的
		if (WorldGen.IsBelowANonHammeredPlatform(topTileX, topTileY)) {
			offsetY -= 8;
		}

		// 物块的size
		int sizeX = 1;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(topTileX, topTileY, sizeX, sizeY))
			windCycle = tileDrawing.GetWindCycle(topTileX, topTileY, tileDrawing._sunflowerWindCounter);

		// 普通源码罢了
		int totalPushTime = 60;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(topTileX, topTileY, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;

		Rectangle rectangle = new Rectangle(tileFrameX, tileFrameY, 16, 16);
		Color tileLight = Lighting.GetColor(pos);
		tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(pos.X, pos.Y, tile, tile.type, tileFrameX, tileFrameY, ref tileLight, tileDrawing._rand.NextBool(4));
		tileLight = tileDrawing.DrawTiles_GetLightOverride(pos.Y, pos.X, tile, tile.type, tileFrameX, tileFrameY, tileLight);
		
		// 对于旗帜需要乘上一个长这样的系数来修正rotation
		float num = isBanner ? (tileFrameY / 18 + 1) / (float)sizeY : 1;
		float rotation = -windCycle * 0.15f * num;

		var origin = new Vector2(sizeX * 16 / 2f, 0);
		var tileSpriteEffect = SpriteEffects.None;

		var drawPos = pos.ToWorldCoordinates(0, 0) + new Vector2(offsetX, offsetY) - screenPosition;
		// 根据节修正position
		drawPos += new Vector2(0f, 16f).RotatedBy(rotation) * layer;

		// 原版中旗帜的旋转角越大，绘制向上偏移就越大，为了防止两个物块间贴图连不起来
		if (isBanner)
			drawPos.Y -= Math.Abs(rotation) * layer * 4;
		
		spriteBatch.Draw(tex, drawPos, rectangle, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
	}

	public static bool BedRightClick(int i, int j)
	{
		Player player = Main.LocalPlayer;
		Tile tile = Main.tile[i, j];
		int spawnX = i - tile.TileFrameX / 18 + (tile.TileFrameX >= 72 ? 5 : 2);
		int spawnY = j + 2;

		if (tile.TileFrameY % 38 != 0)
			spawnY--;
		if (!Player.IsHoveringOverABottomSideOfABed(i, j))
		{ // This assumes your bed is 4x2 with 2x2 sections. You have to write your own code here otherwise
			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
			{
				player.GamepadEnableGrappleCooldown();
				player.sleeping.StartSleeping(player, i, j);
			}
		}
		else
		{
			player.FindSpawn();

			if (player.SpawnX == spawnX && player.SpawnY == spawnY)
			{
				player.RemoveSpawn();
				Main.NewText(Language.GetTextValue("Game.SpawnPointRemoved"), byte.MaxValue, 240, 20);
			}
			else if (Player.CheckSpawn(spawnX, spawnY))
			{
				player.ChangeSpawn(spawnX, spawnY);
				Main.NewText(Language.GetTextValue("Game.SpawnPointSet"), byte.MaxValue, 240, 20);
			}
		}

		return true;
	}

	public static void BedMouseOver<T>(int i, int j)
		where T : ModItem
	{
		Player player = Main.LocalPlayer;

		if (!Player.IsHoveringOverABottomSideOfABed(i, j))
		{
			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
			{ // Match condition in RightClick. Interaction should only show if clicking it does something
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = ItemID.SleepingIcon;
			}
		}
		else
		{
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<T>();
		}
	}

	public static bool ChairRightClick(int i, int j)
	{
		Player player = Main.LocalPlayer;

		if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
		{ // Avoid being able to trigger it from long range
			player.GamepadEnableGrappleCooldown();
			player.sitting.SitDown(player, i, j);
		}
		return true;
	}

	public static void ChairMouseOver<T>(int i, int j)
		where T : ModItem
	{
		Player player = Main.LocalPlayer;

		if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			return;

		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<T>();

		if (Main.tile[i, j].TileFrameX / 18 < 1)
			player.cursorItemIconReversed = true;
	}

	public static bool ChestRightClick(int i, int j)
	{
		Player player = Main.LocalPlayer;
		Tile tile = Main.tile[i, j];
		Main.mouseRightRelease = false;
		int left = i;
		int top = j;
		if (tile.TileFrameX % 36 != 0)
			left--;

		if (tile.TileFrameY != 0)
			top--;

		player.CloseSign();
		player.SetTalkNPC(-1);
		Main.npcChatCornerItem = 0;
		Main.npcChatText = string.Empty;
		if (Main.editChest)
		{
			SoundEngine.PlaySound(SoundID.MenuTick);
			Main.editChest = false;
			Main.npcChatText = string.Empty;
		}

		if (player.editedChestName)
		{
			NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
			player.editedChestName = false;
		}

		if (Main.netMode == NetmodeID.MultiplayerClient)
		{
			if (left == player.chestX && top == player.chestY && player.chest >= 0)
			{
				player.chest = -1;
				Recipe.FindRecipes();
				SoundEngine.PlaySound(SoundID.MenuClose);
			}
			else
			{
				NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
				Main.stackSplit = 600;
			}
		}
		else
		{
			int chest = Chest.FindChest(left, top);
			if (chest >= 0)
			{
				Main.stackSplit = 600;
				if (chest == player.chest)
				{
					player.chest = -1;
					SoundEngine.PlaySound(SoundID.MenuClose);
				}
				else
				{
					player.chest = chest;
					Main.playerInventory = true;
					Main.recBigList = false;
					player.chestX = left;
					player.chestY = top;
					SoundEngine.PlaySound(SoundID.MenuOpen);
				}

				Recipe.FindRecipes();
			}
		}
		return true;
	}

	public static void ChestMouseOver<T>(string chestName, int i, int j)
		where T : ModItem
	{
		Player player = Main.LocalPlayer;
		Tile tile = Main.tile[i, j];
		int left = i;
		int top = j;
		if (tile.TileFrameX % 36 != 0)
			left--;

		if (tile.TileFrameY != 0)
			top--;
		int chest = Chest.FindChest(left, top);
		player.cursorItemIconID = -1;
		if (chest < 0)
			player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
		else
		{
			player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : chestName;
			if (player.cursorItemIconText == chestName)
			{
				player.cursorItemIconID = ModContent.ItemType<T>();

				player.cursorItemIconText = string.Empty;
			}
		}

		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
	}

	public static void ChestMouseFar<T>(string name, int i, int j)
		where T : ModItem
	{
		ChestMouseOver<T>(name, i, j);
		Player player = Main.LocalPlayer;
		if (player.cursorItemIconText == string.Empty)
		{
			player.cursorItemIconEnabled = false;
			player.cursorItemIconID = 0;
		}
	}

	public static bool ClockRightClick()
	{
		string text = "AM";
		double time = Main.time;
		if (!Main.dayTime)
			time += 54000.0;

		time = time / 86400.0 * 24.0;
		time = time - 7.5 - 12.0;
		if (time < 0.0)
			time += 24.0;

		if (time >= 12.0)
			text = "PM";

		int intTime = (int)time;
		double deltaTime = time - intTime;
		deltaTime = (int)(deltaTime * 60.0);
		string text2 = string.Concat(deltaTime);
		if (deltaTime < 10.0)
			text2 = "0" + text2;

		if (intTime > 12)
			intTime -= 12;

		if (intTime == 0)
			intTime = 12;
		Main.NewText($"Time: {intTime}:{text2} {text}", 255, 240, 20);
		return true;
	}

	public static bool DresserRightClick()
	{
		Player player = Main.LocalPlayer;
		if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY == 0)
		{
			Main.CancelClothesWindow(true);
			Main.mouseRightRelease = false;
			int left = Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameX / 18;
			left %= 3;
			left = Player.tileTargetX - left;
			int top = Player.tileTargetY - Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY / 18;
			if (player.sign > -1)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				player.sign = -1;
				Main.editSign = false;
				Main.npcChatText = string.Empty;
			}
			if (Main.editChest)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
				Main.editChest = false;
				Main.npcChatText = string.Empty;
			}
			if (player.editedChestName)
			{
				NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
				player.editedChestName = false;
			}
			if (Main.netMode == NetmodeID.Server)
			{
				if (left == player.chestX && top == player.chestY && player.chest != -1)
				{
					player.chest = -1;
					Recipe.FindRecipes();
					SoundEngine.PlaySound(SoundID.MenuClose);
				}
				else
				{
					NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top, 0f, 0f, 0, 0, 0);
					Main.stackSplit = 600;
				}
			}
			else
			{
				player.piggyBankProjTracker.Clear();
				int num213 = Chest.FindChest(left, top);
				if (num213 != -1)
				{
					Main.stackSplit = 600;
					if (num213 == player.chest)
					{
						player.chest = -1;
						Recipe.FindRecipes();
						SoundEngine.PlaySound(SoundID.MenuClose);
					}
					else if (num213 != player.chest && player.chest == -1)
					{
						player.chest = num213;
						Main.playerInventory = true;
						Main.recBigList = false;
						SoundEngine.PlaySound(SoundID.MenuOpen);
						player.chestX = left;
						player.chestY = top;
					}
					else
					{
						player.chest = num213;
						Main.playerInventory = true;
						Main.recBigList = false;
						SoundEngine.PlaySound(SoundID.MenuTick);
						player.chestX = left;
						player.chestY = top;
					}
					Recipe.FindRecipes();
					return true;
				}
			}
			return false;
		}
		Main.playerInventory = false;
		player.chest = -1;
		Recipe.FindRecipes(false);
		Main.interactedDresserTopLeftX = Player.tileTargetX;
		Main.interactedDresserTopLeftY = Player.tileTargetY;
		Main.OpenClothesWindow();
		return true;
	}

	public static void DresserMouseFar<T>(string chestName)
		where T : ModItem
	{
		Player player = Main.LocalPlayer;
		Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
		int left = Player.tileTargetX;
		int top = Player.tileTargetY;
		left -= tile.TileFrameX % 54 / 18;
		if (tile.TileFrameY % 36 != 0)
			top--;
		int chestIndex = Chest.FindChest(left, top);
		player.cursorItemIconID = -1;
		if (chestIndex < 0)
			player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
		else
		{
			string defaultName = TileLoader.DefaultContainerName(tile.TileType, left, top); // This gets the ContainerName text for the currently selected language
			if (player.cursorItemIconText == defaultName)
				player.cursorItemIconText = Main.chest[chestIndex].name;
			else
			{
				player.cursorItemIconText = chestName;
			}
			if (player.cursorItemIconText == chestName)
			{
				player.cursorItemIconID = ModContent.ItemType<T>();
				player.cursorItemIconText = string.Empty;
			}
		}
		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		if (player.cursorItemIconText == string.Empty)
		{
			player.cursorItemIconEnabled = false;
			player.cursorItemIconID = 0;
		}
	}

	public static void DresserMouseOver<T>(string chestName)
		where T : ModItem
	{
		Player player = Main.LocalPlayer;
		Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
		int left = Player.tileTargetX;
		int top = Player.tileTargetY;
		left -= tile.TileFrameX % 54 / 18;
		if (tile.TileFrameY % 36 != 0)
			top--;
		int chest = Chest.FindChest(left, top);
		player.cursorItemIconID = -1;
		if (chest < 0)
			player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
		else
		{
			if (Main.chest[chest].name != string.Empty)
				player.cursorItemIconText = Main.chest[chest].name;
			else
			{
				player.cursorItemIconText = chestName;
			}
			if (player.cursorItemIconText == chestName)
			{
				player.cursorItemIconID = ModContent.ItemType<T>();
				player.cursorItemIconText = string.Empty;
			}
		}
		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY > 0)
		{
			player.cursorItemIconID = ItemID.FamiliarShirt;
			player.cursorItemIconText = "  ";
		}
	}

	public static bool SofaRightClick(int i, int j)
	{
		Player player = Main.LocalPlayer;

		if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
		{ // Avoid being able to trigger it from long range
			player.GamepadEnableGrappleCooldown();
			player.sitting.SitDown(player, i, j);
		}

		return true;
	}

	public static void SofaMouseOver<T>(int i, int j)
		where T : ModItem
	{
		Player player = Main.LocalPlayer;

		if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			return;

		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<T>();

		if (Main.tile[i, j].TileFrameX / 18 < 1)
			player.cursorItemIconReversed = true;
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="type"></param>
	/// <param name="tileX"></param>物块组合体横向占多少块
	/// <param name="tileY"></param>物块组合体纵向占多少块
	/// <param name="coordinateX"></param>物块组合体横向每一帧宽度(单位像素)
	/// <param name="coordinateY"></param>物块组合体纵向每一帧高度(单位像素)
	public static void LightHitwire(int i, int j, int type, int tileX, int tileY, int coordinateX = 18, int coordinateY = 18)
	{
		Tile tile = Main.tile[i, j];
		int x = i - tile.TileFrameX / coordinateX % tileX;
		int y = j - tile.TileFrameY / coordinateY % tileY;
		for (int m = x; m < x + tileX; m++)
		{
			for (int n = y; n < y + tileY; n++)
			{
				if (!tile.HasTile)
					continue;
				if (tile.TileType == type)
				{
					tile = Main.tile[m, n];
					if (tile.TileFrameX < coordinateX * tileX)
					{
						tile = Main.tile[m, n];
						tile.TileFrameX += (short)(coordinateX * tileX);
					}
					else
					{
						tile = Main.tile[m, n];
						tile.TileFrameX -= (short)(coordinateX * tileX);
					}
				}
			}
		}
		if (!Wiring.running)
			return;
		for (int k = 0; k < tileX; k++)
		{
			for (int l = 0; l < tileY; l++)
			{
				Wiring.SkipWire(x + k, y + l);
			}
		}
	}
}

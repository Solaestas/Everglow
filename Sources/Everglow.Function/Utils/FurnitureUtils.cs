using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Commons.Utils;

public static class FurnitureUtils
{
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
			string defaultName = TileLoader.ContainerName(tile.TileType, left, top); // This gets the ContainerName text for the currently selected language
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

using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.Furnace;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

public class HeatproofDresser : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileSolidTop[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileTable[Type] = true;
		Main.tileContainer[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.BasicDresser[Type] = true;
		TileID.Sets.AvoidedByNPCs[Type] = true;
		TileID.Sets.InteractibleByNPCs[Type] = true;
		TileID.Sets.IsAContainer[Type] = true;
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

		AdjTiles = new int[] { TileID.Dressers };
		DustType = ModContent.DustType<Heatproof_Furniture_Dust>(); // You should set a kind of dust manually.

		// Names
		AddMapEntry(new Color(200, 200, 200), CreateMapEntryName(), MapChestName);

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
		TileObjectData.newTile.AnchorInvalidTiles = new int[] {
			TileID.MagicalIceBlock,
			TileID.Boulder,
			TileID.BouncyBoulder,
			TileID.LifeCrystalBoulder,
			TileID.RollingCactus
		};
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
	}

	public override LocalizedText DefaultContainerName(int frameX, int frameY) {
		return CreateMapEntryName();
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
		return true;
	}

	public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY) {
		width = 3;
		height = 1;
		extraY = 0;
	}

	public override bool RightClick(int i, int j) {
		return FurnitureUtils.DresserRightClick();
	}

	// This is not a hook, this is just a normal method used by the MouseOver and MouseOverFar hooks to avoid repeating code.
	public void MouseOverNearAndFarSharedLogic(Player player, int i, int j) {
		Tile tile = Main.tile[i, j];
		int left = i;
		int top = j;
		left -= tile.TileFrameX % 54 / 18;
		if (tile.TileFrameY % 36 != 0) {
			top--;
		}
		int chestIndex = Chest.FindChest(left, top);
		player.cursorItemIconID = -1;
		if (chestIndex < 0) {
			player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
		}
		else {
			string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY); // This gets the ContainerName text for the currently selected language

			if (Main.chest[chestIndex].name != "") {
				player.cursorItemIconText = Main.chest[chestIndex].name;
			}
			else {
				player.cursorItemIconText = defaultName;
			}
			if (player.cursorItemIconText == defaultName) {
				player.cursorItemIconID = ModContent.ItemType<HeatproofDresser_item>();
				player.cursorItemIconText = "";
			}
		}
		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
	}

	public override void MouseOverFar(int i, int j) {
		Player player = Main.LocalPlayer;
		MouseOverNearAndFarSharedLogic(player, i, j);
		if (player.cursorItemIconText == "") {
			player.cursorItemIconEnabled = false;
			player.cursorItemIconID = 0;
		}
	}

	public override void MouseOver(int i, int j) {
		Player player = Main.LocalPlayer;
		MouseOverNearAndFarSharedLogic(player, i, j);
		if (Main.tile[i, j].TileFrameY > 0) {
			player.cursorItemIconID = ItemID.FamiliarShirt;
			player.cursorItemIconText = "";
		}
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY) {
		Chest.DestroyChest(i, j);
	}

	public static string MapChestName(string name, int i, int j) {
		int left = i;
		int top = j;
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX % 36 != 0) {
			left--;
		}

		if (tile.TileFrameY != 0) {
			top--;
		}

		int chest = Chest.FindChest(left, top);
		if (chest < 0) {
			return Language.GetTextValue("LegacyDresserType.0");
		}

		if (Main.chest[chest].name == "") {
			return name;
		}

		return name + ": " + Main.chest[chest].name;
	}
}

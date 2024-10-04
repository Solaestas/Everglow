import fs from 'node:fs/promises';
import path from 'node:path';
import os from 'node:os';

let [, , name, tile, item, tileSpace, itemSpace, tileOutput, itemOutput] =
    process.argv;
const types = [
    'Bed',
    'Chair',
    'Chest',
    'Clock',
    'Door',
    'Dresser',
    'Platform',
    'Table',
    'Toilet',
    'Workbench',
    'Sink',
    'Chandelier',
    'Lantern',
    'Candle',
    'Candelabra',
    'Bathtub',
    'Bookcase',
    'Lamp',
    'Piano',
    'Sofa'
];

(async () => {
    if (!itemOutput) {
        const home = tileOutput
            ? tileOutput
            : path.join(os.homedir(), 'Documents');
        const modSource = `My Games/Terraria/tModLoader/ModSources/Everglow/Sources/Modules`;
        const dir = path.join(home, modSource);

        try {
            await fs.readdir(dir);
        } catch {
            console.error(
                `Cannot find documents dir in C:/~. Please deliver $6 and $7, or deliver $6 only with your Documents folder.`
            );
            return;
        }

        const tileSplit = tileSpace.split('.').slice(1).join('/');
        const itemSplit = itemSpace.split('.').slice(1).join('/');

        tileOutput = path.join(dir, tileSplit);
        itemOutput = path.join(dir, itemSplit);

        try {
            await fs.readdir(tileOutput);
        } catch {
            console.log(`默认输出文件夹不存在，已新建`);
            await fs.mkdir(tileOutput, { recursive: true });
        }

        try {
            await fs.readdir(itemOutput);
        } catch {
            console.log(`默认输出文件夹不存在，已新建`);
            await fs.mkdir(itemOutput, { recursive: true });
        }
    }
    if (tileOutput && !itemOutput) {
        console.error(
            `Arguments $6 and $7 need be delivered at the same time when needed.`
        );
        return;
    }

    const tileList = new Set(await fs.readdir(tile));
    const itemList = new Set(await fs.readdir(item));

    for (const type of types) {
        const realName = name + type;
        const relatedTile = [...tileList].filter(
            v =>
                (v.startsWith(type) || v.startsWith(realName)) &&
                v.endsWith('.png')
        );
        const relatedItem = [...itemList].filter(
            v =>
                (v.startsWith(type) || v.startsWith(realName)) &&
                v.endsWith('.png')
        );
        relatedTile.forEach(v => tileList.delete(v));
        relatedItem.forEach(v => itemList.delete(v));
        if (relatedTile.length === 0 || relatedItem.length === 0) continue;

        await Promise.all([
            ...relatedTile.map(v =>
                fs.copyFile(
                    path.resolve(tile, v),
                    path.resolve(
                        tileOutput,
                        v.startsWith(realName) ? v : name + v
                    )
                )
            ),
            ...relatedItem.map(v =>
                fs.copyFile(
                    path.resolve(item, v),
                    path.resolve(
                        itemOutput,
                        v.startsWith(realName) ? v : name + v
                    )
                )
            )
        ]);

        if (type === 'Door') {
            const itemCS = itemTemplate(realName, itemSpace, type, tileSpace);
            const tileOpen = tileTemplate.DoorOpen(
                realName,
                tileSpace,
                itemSpace
            );
            const tileClosed = tileTemplate.DoorClosed(
                realName,
                tileSpace,
                itemSpace
            );

            await Promise.all([
                fs.writeFile(
                    path.resolve(tileOutput, realName + 'Open.cs'),
                    tileOpen,
                    'utf-8'
                ),
                fs.writeFile(
                    path.resolve(tileOutput, realName + 'Closed.cs'),
                    tileClosed,
                    'utf-8'
                ),
                fs.writeFile(
                    path.resolve(itemOutput, realName + '.cs'),
                    itemCS,
                    'utf-8'
                )
            ]);
        } else {
            const tileCS = tileTemplate[type](realName, tileSpace, itemSpace);
            const itemCS = itemTemplate(realName, itemSpace, type, tileSpace);

            await Promise.all([
                fs.writeFile(
                    path.resolve(tileOutput, realName + '.cs'),
                    tileCS,
                    'utf-8'
                ),
                fs.writeFile(
                    path.resolve(itemOutput, realName + '.cs'),
                    itemCS,
                    'utf-8'
                )
            ]);
        }
    }

    if (tileList.size > 0) {
        tileList.forEach(v => {
            console.warn(`Unknown tile file: ${v}`);
        });
    }
    if (itemList.size > 0) {
        itemList.forEach(v => {
            console.warn(`Unknown item file: ${v}`);
        });
    }
})();

// ----- template
const tileTemplate = {
    Bed: (name, space, itemSpace) => /* cs */ `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public const int NextStyleHeight = 38; // Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2

	public override void SetStaticDefaults() {
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
		TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
		TileID.Sets.IsValidSpawnPoint[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair); // Beds count as chairs for the purpose of suitable room creation

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Beds };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); // this style already takes care of direction for us
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);

		// Etc
		AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.Bed"));
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
		return true;
	}

	public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY) {
		// Because beds have special smart interaction, this splits up the left and right side into the necessary 2x2 sections
		width = 2; // Default to the Width defined for TileObjectData.newTile
		height = 2; // Default to the Height defined for TileObjectData.newTile
		//extraY = 0; // Depends on how you set up frameHeight and CoordinateHeights and CoordinatePaddingFix.Y
	}

	public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info) {
		// Default values match the regular vanilla bed
		// You might need to mess with the info here if your bed is not a typical 4x2 tile
		info.VisualOffset.Y += 4f; // Move player down a notch because the bed is not as high as a regular bed
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = 1;
	}

	public override bool RightClick(int i, int j) {
		return FurnitureUtils.BedRightClick(i, j);
	}

	public override void MouseOver(int i, int j) {
		FurnitureUtils.BedMouseOver<${itemSpace}.${name}>(i, j);
	}
}
`,
    Chair: (name, space, itemSpace) => /* cs */ `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public const int NextStyleHeight = 40; // Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2

	public override void SetStaticDefaults() {
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Facilitates calling ModifySittingTargetInfo for NPCs
		TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Facilitates calling ModifySittingTargetInfo for Players
		TileID.Sets.DisableSmartCursor[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Chairs };

		// Names
		AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Chair"));

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
		// The following 3 lines are needed if you decide to add more styles and stack them vertically
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleHorizontal = true;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.addAlternate(1); // Facing right will use the second texture style
		TileObjectData.addTile(Type);
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
		return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
	}

	public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info) {
		// It is very important to know that this is called on both players and NPCs, so do not use Main.LocalPlayer for example, use info.restingEntity
		Tile tile = Framing.GetTileSafely(i, j);

		//info.directionOffset = info.restingEntity is Player ? 6 : 2; // Default to 6 for players, 2 for NPCs
		//info.visualOffset = Vector2.Zero; // Defaults to (0,0)

		info.TargetDirection = -1;
		if (tile.TileFrameX != 0) {
			info.TargetDirection = 1; // Facing right if sat down on the right alternate (added through addAlternate in SetStaticDefaults earlier)
		}

		// The anchor represents the bottom-most tile of the chair. This is used to align the entity hitbox
		// Since i and j may be from any coordinate of the chair, we need to adjust the anchor based on that
		info.AnchorTilePosition.X = i; // Our chair is only 1 wide, so nothing special required
		info.AnchorTilePosition.Y = j;

		if (tile.TileFrameY % NextStyleHeight == 0) {
			info.AnchorTilePosition.Y++; // Here, since our chair is only 2 tiles high, we can just check if the tile is the top-most one, then move it 1 down
		}
	}

	public override bool RightClick(int i, int j) {
		return FurnitureUtils.ChairRightClick(i, j);
	}

	public override void MouseOver(int i, int j) {
		FurnitureUtils.ChairMouseOver<${itemSpace}.${name}>(i, j);
	}
}
`,
    Chest: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileSpelunker[Type] = true;
		Main.tileContainer[Type] = true;
		Main.tileShine2[Type] = true;
		Main.tileShine[Type] = 1200;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileOreFinderPriority[Type] = 500;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.BasicChest[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.AvoidedByNPCs[Type] = true;
		TileID.Sets.InteractibleByNPCs[Type] = true;
		TileID.Sets.IsAContainer[Type] = true;
		TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
		TileID.Sets.GeneralPlacementTiles[Type] = false;

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Containers };

		// Other tiles with just one map entry use CreateMapEntryName() to use the default translationkey, "MapEntry"
		// Since ExampleChest needs multiple, we register our own MapEntry keys
		AddMapEntry(new Color(200, 200, 200), this.GetLocalization("MapEntry0"), MapChestName);
		AddMapEntry(new Color(0, 141, 63), this.GetLocalization("MapEntry1"), MapChestName);

		// Style 1 is ExampleChest when locked. We want that tile style to drop the ExampleChest item as well. Use the Chest Lock item to lock this chest.
		// No item places ExampleChest in the locked style, so the automatically determined item drop is unknown, this is why RegisterItemDrop is necessary in this situation. 
		RegisterItemDrop(ModContent.ItemType<${itemSpace}.${name}>(), 1);
		// Sometimes mods remove content, such as tile styles, or tiles accidentally get corrupted. We can, if desired, register a fallback item for any tile style that doesn't have an automatically determined item drop. This is done by omitting the tileStyles parameter.
		RegisterItemDrop(ItemID.Chest);

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.Origin = new Point16(0, 1);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
		TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
		TileObjectData.newTile.AnchorInvalidTiles = new int[] {
			TileID.MagicalIceBlock,
			TileID.Boulder,
			TileID.BouncyBoulder,
			TileID.LifeCrystalBoulder,
			TileID.RollingCactus
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
		TileObjectData.addTile(Type);
	}

	public override ushort GetMapOption(int i, int j) {
		return (ushort)(Main.tile[i, j].TileFrameX / 36);
	}

	public override LocalizedText DefaultContainerName(int frameX, int frameY) {
		int option = frameX / 36;
		return this.GetLocalization("MapEntry" + option);
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
		return true;
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
			return Language.GetTextValue("LegacyChestType.0");
		}

		if (Main.chest[chest].name == "") {
			return name;
		}

		return name + ": " + Main.chest[chest].name;
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = 1;
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY) {
		// We override KillMultiTile to handle additional logic other than the item drop. In this case, unregistering the Chest from the world
		Chest.DestroyChest(i, j);
	}

	public override bool RightClick(int i, int j) {
		return FurnitureUtils.ChestRightClick(i, j);
	}

	public override void MouseOver(int i, int j) {
		Player player = Main.LocalPlayer;
		Tile tile = Main.tile[i, j];
		int left = i;
		int top = j;
		if (tile.TileFrameX % 36 != 0) {
			left--;
		}

		if (tile.TileFrameY != 0) {
			top--;
		}

		int chest = Chest.FindChest(left, top);
		player.cursorItemIconID = -1;
		if (chest < 0) {
			player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
		}
		else {
			string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY); // This gets the ContainerName text for the currently selected language
			player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
			if (player.cursorItemIconText == defaultName) {
				player.cursorItemIconID = ModContent.ItemType<${itemSpace}.${name}>();
				if (Main.tile[left, top].TileFrameX / 36 == 1) {
					// player.cursorItemIconID = ModContent.ItemType<>();
				}

				player.cursorItemIconText = "";
			}
		}

		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
	}

	public override void MouseOverFar(int i, int j) {
		MouseOver(i, j);
		Player player = Main.LocalPlayer;
		if (player.cursorItemIconText == "") {
			player.cursorItemIconEnabled = false;
			player.cursorItemIconID = 0;
		}
	}
}
`,
    Clock: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.Clock[Type] = true;

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.GrandfatherClocks };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
		TileObjectData.addTile(Type);

		// Etc
		AddMapEntry(new Color(200, 200, 200), Language.GetText("ItemName.GrandfatherClock"));
	}

	public override bool RightClick(int x, int y) {
		return FurnitureUtils.ClockRightClick();
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}
}
`,
    DoorClosed: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name}Closed : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileSolid[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.NotReallySolid[Type] = true;
		TileID.Sets.DrawsWalls[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.OpenDoorID[Type] = ModContent.TileType<${name}Open>();

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.ClosedDoor };

		// Names
		AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Door"));

		TileObjectData.addTile(Type);
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
		return true;
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = 1;
	}

	public override void MouseOver(int i, int j) {
		Player player = Main.LocalPlayer;
		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<${itemSpace}.${name}>();
	}
}
`,
    DoorOpen: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name}Open : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = true;
		Main.tileNoSunLight[Type] = true;
		TileID.Sets.HousingWalls[Type] = true; // needed for non-solid blocks to count as walls
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.CloseDoorID[Type] = ModContent.TileType<${name}Closed>();

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.OpenDoor };
		// Tiles usually drop their corresponding item automatically, but RegisterItemDrop is needed here since the ExampleDoor item places ExampleDoorClosed, not this tile.
		RegisterItemDrop(ModContent.ItemType<${itemSpace}.${name}>(), 0);
		TileID.Sets.CloseDoorID[Type] = ModContent.TileType<${name}Closed>();

		// Names
		AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Door"));

		// Placement
		// The TileID.OpenDoor TileObjectData has incorrect anchor and StyleMultiplier values, so we will not be copying from it in this case
		// TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.OpenDoor, 0));
		TileObjectData.newTile.Width = 2;
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Origin = new Point16(0, 0);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
		TileObjectData.newTile.UsesCustomCanPlace = true;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.CoordinatePadding = 2;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleWrapLimit = 2; // Since the wrap limit is 2, a 2nd style will be below the first on the spritesheet even though this is StyleHorizontal = true
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Origin = new Point16(0, 1);
		TileObjectData.addAlternate(0);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Origin = new Point16(0, 2);
		TileObjectData.addAlternate(0);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Origin = new Point16(1, 0);
		TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
		TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
		TileObjectData.addAlternate(1);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Origin = new Point16(1, 1);
		TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
		TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
		TileObjectData.addAlternate(1);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Origin = new Point16(1, 2);
		TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
		TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
		TileObjectData.addAlternate(1);
		TileObjectData.addTile(Type);
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
		return true;
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = 1;
	}

	public override void MouseOver(int i, int j) {
		Player player = Main.LocalPlayer;
		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<${itemSpace}.${name}>();
	}
}
`,
    Dresser: (name, space, itemSpace) => `using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
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
		DustType = ModContent.DustType<>();

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
				player.cursorItemIconID = ModContent.ItemType<${itemSpace}.${name}>();
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
`,
    Platform: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileLighted[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileSolid[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileTable[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.Platforms[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
		AddMapEntry(new Color(200, 200, 200));

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Platforms };

		// Placement
		TileObjectData.newTile.CoordinateHeights = new[] { 16 };
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.CoordinatePadding = 2;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.StyleMultiplier = 27;
		TileObjectData.newTile.StyleWrapLimit = 27;
		TileObjectData.newTile.UsesCustomCanPlace = false;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.addTile(Type);
	}

	public override void PostSetDefaults() => Main.tileNoSunLight[Type] = false;

	public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}
`,
    Table: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileTable[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Tables };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.addTile(Type);

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

		// Etc
		AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Table"));
	}

	public override void NumDust(int x, int y, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}
}
`,
    Toilet: (name, space, itemSpace) => `using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public const int NextStyleHeight = 40; // Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2

	public override void SetStaticDefaults() {
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Facilitates calling ModifySittingTargetInfo for NPCs
		TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Facilitates calling ModifySittingTargetInfo for Players
		TileID.Sets.DisableSmartCursor[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Toilets }; // Consider adding TileID.Chairs to AdjTiles to mirror "(regular) Toilet" and "Golden Toilet" behavior for crafting stations

		// Names
		AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Toilet"));

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
		// The following 3 lines are needed if you decide to add more styles and stack them vertically
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleHorizontal = true;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.addAlternate(1); // Facing right will use the second texture style
		TileObjectData.addTile(Type);
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
		return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
	}

	public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info) {
		// It is very important to know that this is called on both players and NPCs, so do not use Main.LocalPlayer for example, use info.restingEntity
		Tile tile = Framing.GetTileSafely(i, j);

		//info.directionOffset = info.restingEntity is Player ? 6 : 2; // Default to 6 for players, 2 for NPCs
		//info.visualOffset = Vector2.Zero; // Defaults to (0,0)

		info.TargetDirection = -1;

		if (tile.TileFrameX != 0) {
			info.TargetDirection = 1; // Facing right if sat down on the right alternate (added through addAlternate in SetStaticDefaults earlier)
		}

		// The anchor represents the bottom-most tile of the chair. This is used to align the entity hitbox
		// Since i and j may be from any coordinate of the chair, we need to adjust the anchor based on that
		info.AnchorTilePosition.X = i; // Our chair is only 1 wide, so nothing special required
		info.AnchorTilePosition.Y = j;

		if (tile.TileFrameY % NextStyleHeight == 0) {
			info.AnchorTilePosition.Y++; // Here, since our chair is only 2 tiles high, we can just check if the tile is the top-most one, then move it 1 down
		}

		// Finally, since this is a toilet, it should generate Poo while any tier of Well Fed is active
		info.ExtraInfo.IsAToilet = true;

		// Here we add a custom fun effect to this tile that vanilla toilets do not have. This shows how you can type cast the restingEntity to Player and use visualOffset as well.
		if (info.RestingEntity is Player player && player.HasBuff(BuffID.Stinky)) {
			info.VisualOffset = Main.rand.NextVector2Circular(2, 2);
		}
	}

	public override bool RightClick(int i, int j) {
		Player player = Main.LocalPlayer;

		if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) { // Avoid being able to trigger it from long range
			player.GamepadEnableGrappleCooldown();
			player.sitting.SitDown(player, i, j);
		}

		return true;
	}

	public override void MouseOver(int i, int j) {
		Player player = Main.LocalPlayer;

		if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) { // Match condition in RightClick. Interaction should only show if clicking it does something
			return;
		}

		player.noThrow = 2;
		player.cursorItemIconEnabled = true;
		player.cursorItemIconID = ModContent.ItemType<${itemSpace}.${name}>();

		if (Main.tile[i, j].TileFrameX / 18 < 1) {
			player.cursorItemIconReversed = true;
		}
	}

	public override void HitWire(int i, int j) {
		// Spawn the toilet effect here when triggered by a signal
		Tile tile = Main.tile[i, j];

		int spawnX = i;
		int spawnY = j - (tile.TileFrameY % NextStyleHeight) / 18;

		Wiring.SkipWire(spawnX, spawnY);
		Wiring.SkipWire(spawnX, spawnY + 1);

		if (Wiring.CheckMech(spawnX, spawnY, 60)) {
			Projectile.NewProjectile(Wiring.GetProjectileSource(spawnX, spawnY), spawnX * 16 + 8, spawnY * 16 + 12, 0f, 0f, ProjectileID.ToiletEffect, 0, 0f, Main.myPlayer);
		}
	}
}
`,
    Workbench: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileTable[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.WorkBenches };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
		TileObjectData.newTile.CoordinateHeights = new[] { 18 };
		TileObjectData.addTile(Type);

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

		// Etc
		AddMapEntry(new Color(200, 200, 200), Language.GetText("ItemName.WorkBench"));
	}

	public override void NumDust(int x, int y, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}
}
`,
    Sink: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults()
	{
		TileID.Sets.CountsAsWaterSource[Type] = true;

		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = false;
		Main.tileFrameImportant[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.addTile(Type);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { Type };

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
}
`,
    Chandelier: (name, space, itemSpace) => `using System;
using Everglow.Commons.TileHelper;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile, ITileFluentlyDrawn, ITileFlameData
{
	private Asset<Texture2D> flameTexture;

	public override void SetStaticDefaults()
	{
		Main.tileFlame[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Chandeliers };

		// Placement - Standard Chandelier Setup Below
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.Origin = new Point16(1, 0);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
		TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.newTile.StyleWrapLimit = 37;
		TileObjectData.newTile.StyleHorizontal = false;
		TileObjectData.newTile.StyleLineSkip = 2;
		TileObjectData.newTile.DrawYOffset = -2;
		TileObjectData.addTile(Type);

		if (!Main.dedServ)
			flameTexture = ModContent.Request<Texture2D>("");

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 3, 3);
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX < 54)
		{
			r = 0.1f;
			g = 0.9f;
			b = 1f;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		TileFluentDrawManager.AddFluentPoint(this, i, j);
		return false;
	}
	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		FurnitureUtils.Chandelier3x3FluentDraw(screenPosition, pos, spriteBatch, tileDrawing);
	}

	public TileDrawing.TileFlameData GetTileFlameData(int tileX, int tileY, int type, int tileFrameY) =>
		new TileDrawing.TileFlameData() {
			flameCount = 7,
			flameTexture = flameTexture.Value,
			flameRangeXMin = -10,
			flameRangeXMax = 11,
			flameRangeMultX = 0.15f,
			flameRangeYMin = -10,
			flameRangeYMax = 1,
			flameRangeMultY = 0.35f,
			flameColor = new Color(30, 30, 30, 0)
		};
}
`,
    Lantern: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Everglow.Commons.TileHelper;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ObjectData;
using static Terraria.ModLoader.Default.LegacyUnloadedTilesSystem;
using Terraria.GameContent.Drawing;
using Everglow.Commons.DataStructures;
using Terraria;

namespace ${space};

public class ${name} : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;
		
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		AdjTiles = new int[] { TileID.HangingLanterns };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
		TileObjectData.newTile.DrawYOffset = -2;
		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.DrawYOffset = -10;
		TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.Platform, TileObjectData.newTile.Width, 0);
		TileObjectData.addAlternate(0);
		TileObjectData.addTile(Type); // addTile一定要放在addAlternate后面

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(251, 235, 127), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 2);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 18)
		{
			r = 0.1f;
			g = 0.9f;
			b = 1f;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		TileFluentDrawManager.AddFluentPoint(this, i, j);
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing) 
	{
		FurnitureUtils.LanternFluentDraw(screenPosition, pos, spriteBatch, tileDrawing);
	}
}
`,
    Candle: (name, space, itemSpace) => `using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	private Asset<Texture2D> flameTexture;

	public override void SetStaticDefaults()
	{
		Main.tileTable[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.InteractibleByNPCs[Type] = true;
		TileID.Sets.IsValidSpawnPoint[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Candles };
		TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
		TileObjectData.addTile(Type);

		if (!Main.dedServ)
		{
			if (!Main.dedServ)
				flameTexture = ModContent.Request<Texture2D>("");
		}

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.1f;
		g = 0.9f;
		b = 1f;
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 1);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 54)
		{
			int frequency = 20;
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)) && Main.rand.NextBool(frequency))
			{
				Rectangle dustBox = Utils.CenteredRectangle(new Vector2(i * 16 + 8, j * 16 + 4), new Vector2(16, 16));
				int numForDust = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, ModContent.DustType<Dusts.BlueToPurpleSpark>(), 0f, 0f, 254, default, Main.rand.NextFloat(0.95f, 1.75f));
				Dust obj = Main.dust[numForDust];
				obj.velocity *= 0.4f;
				Main.dust[numForDust].velocity.Y -= 0.4f;
			}
		}
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;

		ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i); // Don't remove any casts.
		var color = new Color(55, 5, 255, 0);
		int width = 20;
		int height = 20;
		var tile = Main.tile[i, j];
		int frameX = tile.TileFrameX;
		int frameY = tile.TileFrameY;
		color.A = 40;
		for (int k = 0; k < 7; k++)
		{
			float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
			float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

			spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + yy + k * 0.2f) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
		}
		color = new Color(22, 22, 22, 0);
		for (int k = 0; k < 7; k++)
		{
			float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
			float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

			spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + yy + 3 - k * 0.3f) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
		}
	}

	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
	{
		offsetY = 2;
	}
}
`,
    Candelabra: (name, space, itemSpace) => `using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	private Asset<Texture2D> flameTexture;

	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileTable[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
		TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
		TileID.Sets.IsValidSpawnPoint[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Candelabras };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2); // this style already takes care of direction for us
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
		TileObjectData.addTile(Type);
		if (!Main.dedServ)
			flameTexture = ModContent.Request<Texture2D>("");

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.1f;
		g = 0.9f;
		b = 1f;
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 2, 2);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 54)
		{
			int frequency = 20;
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)) && Main.rand.NextBool(frequency))
			{
				Rectangle dustBox = Utils.CenteredRectangle(new Vector2(i * 16 + 8, j * 16 + 4), new Vector2(16, 16));
				int numForDust = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, ModContent.DustType<Dusts.BlueToPurpleSpark>(), 0f, 0f, 254, default, Main.rand.NextFloat(0.95f, 1.75f));
				Dust obj = Main.dust[numForDust];
				obj.velocity *= 0.4f;
				Main.dust[numForDust].velocity.Y -= 0.4f;
			}
		}
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;

		ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i); // Don't remove any casts.
		var color = new Color(55, 5, 255, 0);
		int width = 20;
		int height = 20;
		var tile = Main.tile[i, j];
		int frameX = tile.TileFrameX;
		int frameY = tile.TileFrameY;
		color.A = 40;
		for (int k = 0; k < 7; k++)
		{
			float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
			float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

			spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + yy + k * 0.2f) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
		}
		color = new Color(22, 22, 22, 0);
		for (int k = 0; k < 7; k++)
		{
			float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
			float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

			spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + yy + 3 - k * 0.3f) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
		}
	}

	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
	{
		offsetY = 2;
	}
}
`,
    Bathtub: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair); // Beds count as chairs for the purpose of suitable room creation

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Bathtubs };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); // this style already takes care of direction for us
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
}
`,
    Bookcase: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileTable[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Bookcases };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
		TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
}
`,
    Lamp: (name, space, itemSpace) => `using Terraria.DataStructures;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileTable[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
		TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
		TileID.Sets.IsValidSpawnPoint[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Lamps };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX); // this style already takes care of direction for us
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 18)
		{
			r = 0.1f;
			g = 0.9f;
			b = 1f;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 3);
	}
}
`,
    Piano: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileTable[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Pianos };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
}
`,
    Sofa: (name, space, itemSpace) => `using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace ${space};

public class ${name} : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Facilitates calling ModifySittingTargetInfo for NPCs
		TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Facilitates calling ModifySittingTargetInfo for Players
		TileID.Sets.DisableSmartCursor[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

		DustType = ModContent.DustType<>();
		AdjTiles = new int[] { TileID.Benches };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
		// The following 3 lines are needed if you decide to add more styles and stack them vertically
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleHorizontal = true;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.addAlternate(1); // Facing right will use the second texture style
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
	{
		return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
	}

	public override bool RightClick(int i, int j)
	{
		return FurnitureUtils.SofaRightClick(i, j);
	}

	public override void MouseOver(int i, int j)
	{
		FurnitureUtils.SofaMouseOver<Items.Furnitures.GlowWoodSofa>(i, j);
	}
}
`
};

const itemTemplate = (name, space, type, tileSpace) => {
    return `using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;

namespace ${space};

public class ${name} : ${type === 'Workbench' ? 'WorkBench' : type}Item
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<${tileSpace}.${
        type === 'Door' ? name + 'Closed' : name
    }>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}
`;
};

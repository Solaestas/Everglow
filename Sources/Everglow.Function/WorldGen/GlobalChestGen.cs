using Everglow.Commons.Weapons.StabbingSwords;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace Everglow.Commons;

public class GlobalChestGen : ModSystem
{
	public override void PostWorldGen()
	{
		bool placed = false;
		bool placed2 = false;
		for (int offX = -36; offX < 36; offX += 72)
		{
			for (int offY = -36; offY < 12; offY++)
			{
				if (!placed)
					placed = TrySpellbookChest(Main.spawnTileX + offX, Main.spawnTileY + offY, 36);
				else break;
				if (!placed2)
					placed2 = TryBayonetChest(Main.spawnTileX + offX + 3, Main.spawnTileY + offY, 36);
				else break;
			}
		}
	}
	private bool TrySpellbookChest(int startX, int startY, int rangeX = 16)
	{
		int[] legalTile = new int[]
		{
				TileID.Stone,
				TileID.Grass,
				TileID.Dirt,
				TileID.SnowBlock,
				TileID.IceBlock,
				TileID.ClayBlock,
				TileID.Mud,
				TileID.JungleGrass,
				TileID.Sand
		};
		bool canPlace = true;
		int dir = -1;
		if (startY < 4 || startY > Main.maxTilesY - 1)
			return false;
		for (int x = startX; dir > 0 ? x <= startX + rangeX : x >= startX - rangeX; x += dir)
		{
			if (x < 0 || x > Main.maxTilesX - 4)
				continue;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Tile tile = Framing.GetTileSafely(x + i, startY - j);
					if (WorldGen.SolidOrSlopedTile(tile))
					{
						canPlace = false;
						break;
					}
				}
			}
			if (canPlace)
			{
				for (int i = 0; i < 4; i++)
				{
					Tile tile = Framing.GetTileSafely(x + i, startY + 1);
					if (!WorldGen.SolidTile(tile) || !legalTile.Contains(tile.TileType))
					{
						canPlace = false;
						break;
					}
				}
			}
			if (canPlace)
			{
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						WorldGen.KillTile(x + i, startY - j);
					}
				}
				WorldGen.PlaceTile(x, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 1, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 2, startY + 1, TileID.MeteoriteBrick, forced: true);
				//WorldGen.PlaceTile(x + 3, startY + 1, TileID.MeteoriteBrick, forced: true);
				int c = WorldGen.PlaceChest(x + 1, startY, style: 49);
				Chest chest = Main.chest[c];
				if (chest != null)
				{
					ModContent.TryFind("Everglow/CrystalSkull", out ModItem crystalSkull);
					ModContent.TryFind("Everglow/DreamWeaver", out ModItem dreamWeaver);
					ModContent.TryFind("Everglow/FireFeatherMagic", out ModItem fireFeatherMagic);
					ModContent.TryFind("Everglow/FreezeFeatherMagic", out ModItem freezeFeatherMagic);
					ModContent.TryFind("Everglow/BoneFeatherMagic", out ModItem boneFeatherMagic);
					chest.name = "Spellbook Demo";
					chest.item[0].SetDefaults(crystalSkull.Type);
					chest.item[1].SetDefaults(ItemID.WaterBolt);
					chest.item[2].SetDefaults(ItemID.DemonScythe);
					chest.item[3].SetDefaults(ItemID.BookofSkulls);
					chest.item[4].SetDefaults(ItemID.CrystalStorm);
					chest.item[5].SetDefaults(ItemID.CursedFlames);
					chest.item[6].SetDefaults(ItemID.GoldenShower);
					chest.item[7].SetDefaults(ItemID.MagnetSphere);
					chest.item[8].SetDefaults(ItemID.RazorbladeTyphoon);
					chest.item[9].SetDefaults(ItemID.LunarFlareBook);
					chest.item[10].SetDefaults(dreamWeaver.Type);
					chest.item[11].SetDefaults(fireFeatherMagic.Type);
					chest.item[12].SetDefaults(freezeFeatherMagic.Type);
					chest.item[13].SetDefaults(boneFeatherMagic.Type);
				}
				return true;
			}
			if (x <= startX - rangeX)
			{
				dir = 1;
				x = startX;
			}
		}
		return false;
	}
	private bool TryBayonetChest(int startX, int startY, int rangeX = 16)
	{
		int[] legalTile = new int[]
		{
				TileID.Stone,
				TileID.Grass,
				TileID.Dirt,
				TileID.SnowBlock,
				TileID.IceBlock,
				TileID.ClayBlock,
				TileID.Mud,
				TileID.JungleGrass,
				TileID.Sand
		};
		bool canPlace = true;
		int dir = -1;
		if (startY < 4 || startY > Main.maxTilesY - 1)
			return false;
		for (int x = startX; dir > 0 ? x <= startX + rangeX : x >= startX - rangeX; x += dir)
		{
			if (x < 0 || x > Main.maxTilesX - 4)
				continue;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Tile tile = Framing.GetTileSafely(x + i, startY - j);
					if (WorldGen.SolidOrSlopedTile(tile))
					{
						canPlace = false;
						break;
					}
				}
			}
			if (canPlace)
			{
				for (int i = 0; i < 4; i++)
				{
					Tile tile = Framing.GetTileSafely(x + i, startY + 1);
					if (!WorldGen.SolidTile(tile) || !legalTile.Contains(tile.TileType))
					{
						canPlace = false;
						break;
					}
				}
			}
			if (canPlace)
			{
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						WorldGen.KillTile(x + i, startY - j);
					}
				}
				WorldGen.PlaceTile(x, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 1, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 2, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 3, startY + 1, TileID.MeteoriteBrick, forced: true);
				int c = WorldGen.PlaceChest(x + 1, startY, style: 49);
				Chest chest = Main.chest[c];
				if (chest != null)
				{
					// We use TryFind since we cannot access all of the Myth content from here
					//ModContent.TryFind(eternalResolvePath + "MagicWeaponsReplace/Items/CrystalSkull", out ModItem crystalSkull);
					//ModContent.TryFind(eternalResolvePath + "TheFirefly/Items/Weapons/DreamWeaver", out ModItem dreamWeaver);
					ModContent.TryFind("Everglow/BloodGoldBayonet", out ModItem bloodGoldBayonet);
					ModContent.TryFind("Everglow/BlossomThorn", out ModItem blossomThorn);
					ModContent.TryFind("Everglow/BlueRibbonBayonet", out ModItem blueRibbonBayonet);
					ModContent.TryFind("Everglow/CopperBayonet", out ModItem copperBayonet);
					ModContent.TryFind("Everglow/CrutchBayonet", out ModItem crutchBayonet);
					ModContent.TryFind("Everglow/CurseFlameStabbingSword", out ModItem curseFlameBayonet);
					ModContent.TryFind("Everglow/DreamStar", out ModItem dreamStarBayonet);
					ModContent.TryFind("Everglow/EnchantedBayonet", out ModItem enchantedBayonet);
					ModContent.TryFind("Everglow/EternalNight", out ModItem eternalNight);
					ModContent.TryFind("Everglow/GoldenStabbingSword", out ModItem goldenBayonet);
					ModContent.TryFind("Everglow/HolyBayonet", out ModItem holyBayonet);
					ModContent.TryFind("Everglow/IchorBayonet", out ModItem ichorBayonet);
					ModContent.TryFind("Everglow/IronStabbingSword", out ModItem ironBayonet);
					ModContent.TryFind("Everglow/LeadStabbingSword", out ModItem leadBayonet);
					ModContent.TryFind("Everglow/MechanicMosquito", out ModItem mechanicMosquitoBayonet);
					ModContent.TryFind("Everglow/PlatinumStabbingSword", out ModItem platinumBayonet);
					ModContent.TryFind("Everglow/PrisonFireBayonet", out ModItem prisonFireBayonet);
					ModContent.TryFind("Everglow/RedRibbonBayonet", out ModItem redRibbonBayonet);
					ModContent.TryFind("Everglow/RottenGoldBayonet", out ModItem rottenGoldBayonet);
					ModContent.TryFind("Everglow/SilverStabbingSword", out ModItem silverBayonet);
					ModContent.TryFind("Everglow/SwordfishBeak", out ModItem swordfishBeak);
					ModContent.TryFind("Everglow/TinStabbingSword", out ModItem tinBayonet);
					ModContent.TryFind("Everglow/TungstenStabbingSword", out ModItem tungstenBayonet);
					ModContent.TryFind("Everglow/VegetationBayonet", out ModItem vegetationBayonet);
					ModContent.TryFind("Everglow/VertebralSpur", out ModItem vertebralSpurBayonet);
					ModContent.TryFind("Everglow/YoenLeZed", out ModItem yoenLeZedTheDestructionOfMeteoriteThunderBayonet);
					ModContent.TryFind("Everglow/PineStab", out ModItem pineTreeBayonet);
					chest.name = "Bayonet Demo";
					chest.item[0].SetDefaults(bloodGoldBayonet.Type);
					chest.item[1].SetDefaults(blossomThorn.Type);
					chest.item[2].SetDefaults(blueRibbonBayonet.Type);
					chest.item[3].SetDefaults(copperBayonet.Type);
					chest.item[4].SetDefaults(crutchBayonet.Type);
					chest.item[5].SetDefaults(curseFlameBayonet.Type);
					chest.item[6].SetDefaults(dreamStarBayonet.Type);
					chest.item[7].SetDefaults(enchantedBayonet.Type);
					chest.item[8].SetDefaults(eternalNight.Type);
					chest.item[9].SetDefaults(goldenBayonet.Type);
					chest.item[10].SetDefaults(holyBayonet.Type);
					chest.item[11].SetDefaults(ichorBayonet.Type);
					chest.item[12].SetDefaults(ironBayonet.Type);
					chest.item[13].SetDefaults(leadBayonet.Type);
					chest.item[14].SetDefaults(mechanicMosquitoBayonet.Type);
					chest.item[15].SetDefaults(platinumBayonet.Type);
					chest.item[16].SetDefaults(prisonFireBayonet.Type);
					chest.item[17].SetDefaults(redRibbonBayonet.Type);
					chest.item[18].SetDefaults(rottenGoldBayonet.Type);
					chest.item[19].SetDefaults(silverBayonet.Type);
					chest.item[20].SetDefaults(swordfishBeak.Type);
					chest.item[21].SetDefaults(tinBayonet.Type);
					chest.item[22].SetDefaults(tungstenBayonet.Type);
					chest.item[23].SetDefaults(vegetationBayonet.Type);
					chest.item[24].SetDefaults(vertebralSpurBayonet.Type);
					chest.item[25].SetDefaults(yoenLeZedTheDestructionOfMeteoriteThunderBayonet.Type);
					chest.item[26].SetDefaults(pineTreeBayonet.Type);
				}
				return true;
			}
			if (x <= startX - rangeX)
			{
				dir = 1;
				x = startX;
			}
		}
		return false;
	}
}
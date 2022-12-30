using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace
{
    public class MagicGen : ModSystem
    {
        public override void PostWorldGen()
        {
            bool placed = false;
            for (int offX = -36; offX < 37; offX += 72)
            {
                for (int offY = -48; offY < 16; offY++)
                {
                    if (!placed)
                    {
                        placed = TrySpellbookChest(Main.spawnTileX + offX, Main.spawnTileY + offY, 48);
                    }
                    else
                    {
                        break;
                    }
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
            if (startY < 3 || startY > Main.maxTilesY - 1)
            {
                return false;
            }
            for (int x = startX; dir > 0 ? (x <= startX + rangeX) : (x >= startX - rangeX); x += dir)
            {
                if (x < 0 || x > Main.maxTilesX - 2)
                {
                    continue;
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
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
                    for (int i = 0; i < 2; i++)
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
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            WorldGen.KillTile(x + i, startY - j);
                        }
                    }
                    WorldGen.PlaceTile(x, startY + 1, TileID.MeteoriteBrick, forced: true);
                    WorldGen.PlaceTile(x + 1, startY + 1, TileID.MeteoriteBrick, forced: true);
                    int c = WorldGen.PlaceChest(x, startY, style: 49);
                    Chest chest = Main.chest[c];
                    if (chest != null)
                    {
                        chest.name = "Spellbook Demo";
                        chest.item[0].SetDefaults(ModContent.ItemType<CrystalSkull>());
                        chest.item[1].SetDefaults(ItemID.WaterBolt);
                        chest.item[2].SetDefaults(ItemID.DemonScythe);
                        chest.item[3].SetDefaults(ItemID.BookofSkulls);
                        chest.item[4].SetDefaults(ItemID.CrystalStorm);
                        chest.item[5].SetDefaults(ItemID.CursedFlames);
                        chest.item[6].SetDefaults(ItemID.GoldenShower);
                        chest.item[7].SetDefaults(ItemID.MagnetSphere);
                        chest.item[8].SetDefaults(ItemID.RazorbladeTyphoon);
                        chest.item[9].SetDefaults(ItemID.LunarFlareBook);
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
}

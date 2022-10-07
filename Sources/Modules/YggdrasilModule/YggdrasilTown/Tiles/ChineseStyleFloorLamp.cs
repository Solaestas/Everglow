using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
<<<<<<<< HEAD:Sources/Modules/YggdrasilModule/YggdrasilTown/Tiles/StreetLantern.cs
    public class StreetLantern : ModTile
========
    public class ChineseStyleFloorLamp : ModTile
>>>>>>>> omnielement/Yggdrasil:Sources/Modules/YggdrasilModule/YggdrasilTown/Tiles/ChineseStyleFloorLamp.cs
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 48;
            TileObjectData.addTile(Type);
            DustType = DustID.DynastyWood;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(151, 31, 32), modTranslation);
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.tile[i, j].TileFrameX < 40 && Main.tile[i, j].TileFrameY < 40)
            {
                Lighting.AddLight(new Vector2(i * 16, j * 16), new Vector3(1f, 0.8f, 0.5f));
            }
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
<<<<<<<< HEAD:Sources/Modules/YggdrasilModule/YggdrasilTown/Tiles/StreetLantern.cs
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.StreetLantern>());
========
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.ChineseStyleFloorLamp>());
>>>>>>>> omnielement/Yggdrasil:Sources/Modules/YggdrasilModule/YggdrasilTown/Tiles/ChineseStyleFloorLamp.cs
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
        }
        public override void HitWire(int i, int j)
        {
            int k = i;
            for (int l = j; l < j + 6; l++)
            {
                if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == Type)
                {
                    if (Main.tile[k, l].TileFrameX < 40)
                    {
                        Main.tile[k, l].TileFrameX += 48;
                    }
                    else
                    {
                        Main.tile[k, l].TileFrameX -= 48;
                    }
                    if (Wiring.running)
                    {
                        Wiring.SkipWire(k, l);
                    }
                }
                else
                {
                    break;
                }
            }
            for (int l = j - 1; l > j - 6; l--)
            {
                if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == Type)
                {
                    if (Main.tile[k, l].TileFrameX < 40)
                    {
                        Main.tile[k, l].TileFrameX += 48;
                    }
                    else
                    {
                        Main.tile[k, l].TileFrameX -= 48;
                    }
                    if (Wiring.running)
                    {
                        Wiring.SkipWire(k, l);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}

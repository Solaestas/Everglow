using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ObjectData;

namespace MythMod.OceanMod.Tiles
{
    public class SparseBarnacle3 : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                36,
            };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateWidth = 36;
            TileObjectData.addTile((int)base.Type);
            DustType = 7;
            ItemDrop = ModContent.ItemType<OceanMod.Items.Barnacle>();
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("Barnacle");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "藤壶");
            base.AddMapEntry(new Color(108, 108, 78), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 48, 48, ModContent.ItemType<OceanMod.Items.Barnacle>(), 1, false, 0, false, false);
        }
        public override void RandomUpdate(int i, int j)
        {
            int ty = Main.rand.Next(4);
            int Typ = 0;
            if (ty == 0)
            {
                Typ = ModContent.TileType<OceanMod.Tiles.Barnacle1>();
            }
            if (ty == 1)
            {
                Typ = ModContent.TileType<OceanMod.Tiles.Barnacle2>();
            }
            if (ty == 2)
            {
                Typ = ModContent.TileType<OceanMod.Tiles.Barnacle3>();
            }
            if (ty == 3)
            {
                Typ = ModContent.TileType<OceanMod.Tiles.Barnacle4>();
            }
            Main.tile[i, j].TileType = (ushort)Typ;
        }
    }
}

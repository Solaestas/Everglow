using Terraria.Audio;
using Terraria.Localization;
using Terraria.ObjectData;
using Everglow.Sources.Modules.YggdrasilModule.Common;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
    public class CyanVineOre : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 30000; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = false;
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 5;

            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 22 };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(195, 217, 229));
            DustType = DustID.Silver;
            AdjTiles = new int[] { Type };
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            SoundEngine.PlaySound(SoundID.NPCHit4, new Vector2(i * 16, j * 16));
            int Times = Main.rand.Next(5, 9);
            for (int d = 0; d < Times; d++)
            {
                Item.NewItem(null, i * 16 + Main.rand.Next(72), j * 16 + Main.rand.Next(64), 16, 16, ModContent.ItemType<Items.CyanVineOre>());
            }
            for (int f = 0; f < 13; f++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d);
                Gore.NewGore(null, new Vector2(i * 16, j * 16) + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineOre" + f.ToString()).Type, 1f);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.Silver, vF.X, vF.Y);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.WoodFurniture, vF.X, vF.Y);
            }
        }
    }
}

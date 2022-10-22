using Terraria.Audio;
using Terraria.Localization;
using Terraria.ObjectData;
using Everglow.Sources.Modules.YggdrasilModule.Common;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles.CyanVine
{
    public class CyanVineOreLarge : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;

            MineResist = 24f;
            HitSound = SoundID.NPCHit4;
            MinPick = 40;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(84, 130, 154));
            DustType = ModContent.DustType<Dusts.CyanVine>();
            AdjTiles = new int[] { Type };
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail)
            {
                return;
            }
            var ThisTile = Main.tile[i, j];
            int X0 = i - ThisTile.TileFrameX / 18 + 1;
            int Y0 = j - ThisTile.TileFrameY / 18 + 1;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var tile = Main.tile[X0 + x, Y0 + y];
                    if (tile.TileFrameX == x * 18 && tile.TileFrameY == y * 18)
                    {
                        if (tile.TileType == ModContent.TileType<CyanVineOreTile>() && tile.HasTile)
                        {
                            tile.HasTile = false;
                        }
                    }
                }
            }
            SoundEngine.PlaySound(SoundID.NPCHit4, new Vector2(i * 16, j * 16));
            int Times = Main.rand.Next(14, 21);
            if(ThisTile.TileFrameX > 108)
            {
                Times = Main.rand.Next(7, 10);
            }    
            for (int d = 0; d < Times; d++)
            {
                Item.NewItem(null, i * 16 + Main.rand.Next(94) - 16, j * 16 + Main.rand.Next(64) - 48, 16, 16, ModContent.ItemType<Items.CyanVineOre>());
            }
            Times = 13;
            if (ThisTile.TileFrameX > 108)
            {
                Times = 9;
            }
            for (int f = 0; f < Times; f++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d);
                Gore.NewGore(null, new Vector2(i * 16 + Main.rand.Next(94) - 16, j * 16 + Main.rand.Next(64) - 48) + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineOre" + Main.rand.Next(13).ToString()).Type, 1f);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16 + Main.rand.Next(94) - 16, j * 16 + Main.rand.Next(64) - 48) + vF, 0, 0, ModContent.DustType<Dusts.CyanVine>(), vF.X, vF.Y);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16 + Main.rand.Next(94) - 16, j * 16 + Main.rand.Next(64) - 48) + vF, 0, 0, DustID.WoodFurniture, vF.X, vF.Y);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return false;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            if (tile.TileFrameX % 108 == 36 && tile.TileFrameY == 54)
            {
                for (int x = -2; x < 4; x++)
                {
                    for (int y = -5; y < 1; y++)
                    {
                        Color cTile = Lighting.GetColor(i + x, j + y);
                        Texture2D tex = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/CyanVine/CyanVineOreLarge");
                        spriteBatch.Draw(tex, new Vector2((i + x) * 16, (j + y) * 16) - Main.screenPosition + zero, new Rectangle(x * 18 + tile.TileFrameX, y * 18 + tile.TileFrameY, 16, 18), cTile, 0, new Vector2(0), 1, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}

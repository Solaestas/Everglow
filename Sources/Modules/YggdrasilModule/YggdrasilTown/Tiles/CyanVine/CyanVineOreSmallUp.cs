using Terraria.Audio;
using Terraria.Localization;
using Terraria.ObjectData;
using Everglow.Sources.Modules.YggdrasilModule.Common;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles.CyanVine
{
    public class CyanVineOreSmallUp : ModTile
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
            DustType = DustID.Silver;
            AdjTiles = new int[] { Type };
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail)
            {
                return;
            }
            var ThisTile = Main.tile[i, j];
            int X0 = i - ThisTile.TileFrameX / 18;
            int Y0 = j - ThisTile.TileFrameY / 18;
            for (int x = 0; x < 3; x++)
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
            int Times = Main.rand.Next(3, 6);
            for (int d = 0; d < Times; d++)
            {
                Item.NewItem(null, i * 16 + Main.rand.Next(94) - 16, j * 16 + Main.rand.Next(64) - 48, 16, 16, ModContent.ItemType<Items.CyanVineOre>());
            }
            for (int f = 0; f < 8; f++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d);
                Gore.NewGore(null, new Vector2(i * 16 + Main.rand.Next(34) - 16, j * 16 + Main.rand.Next(38) - 32) + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineOre" + f.ToString()).Type, 1f);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16 + Main.rand.Next(34) - 16, j * 16 + Main.rand.Next(38) - 32) + vF, 0, 0, DustID.Silver, vF.X, vF.Y);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            if(tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        Color cTile = Lighting.GetColor(i + x, j + y);
                        Texture2D tex = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/CyanVine/CyanVineOreSmallUp");
                        if (y == 0)
                        {
                            spriteBatch.Draw(tex, new Vector2((i + x) * 16, (j + y) * 16 - 2) - Main.screenPosition + zero, new Rectangle(x * 18 + tile.TileFrameX, y * 18 + tile.TileFrameY + 2, 16, 18), cTile, 0, new Vector2(0), 1, SpriteEffects.None, 0);
                            continue;
                        }
                        spriteBatch.Draw(tex, new Vector2((i + x) * 16, (j + y) * 16 - 4) - Main.screenPosition + zero, new Rectangle(x * 18 + tile.TileFrameX, y * 18 + tile.TileFrameY, 16, 18), cTile, 0, new Vector2(0), 1, SpriteEffects.None, 0);
                    }
                }
            }
            else
            {
                return false;
            }
            
            return false;
        }
    }
}

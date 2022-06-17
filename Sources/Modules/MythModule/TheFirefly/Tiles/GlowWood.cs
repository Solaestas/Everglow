using Terraria.Localization;
using Terraria.ObjectData;
using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class GlowWood : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(51, 26, 58));
            DustType = ModContent.DustType<Bosses.CorruptMoth.Dusts.MothBlue2>();
            AdjTiles = new int[] { Type };
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("Wood");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "木");
            base.AddMapEntry(new Color(155, 173, 183), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int Times = Main.rand.Next(5, 9);
            for (int d = 0; d < Times; d++)
            {
                Item.NewItem(null, i * 16 + Main.rand.Next(72), j * 16 + Main.rand.Next(64), 16, 16, ModContent.ItemType<Items.GlowWood>());
            }
            /*for (int f = 0; f < 13; f++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d);
                Gore.NewGore(null, new Vector2(i * 16, j * 16) + vF, vF, ModContent.Find<ModGore>("MythMod/CyanVineOre" + f.ToString()).Type, 1f);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.Silver, vF.X, vF.Y);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.WoodFurniture, vF.X, vF.Y);
            }*/
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            /*short num = (short)(Main.rand.Next(0, 4));
            Main.tile[i, j - 3].TileFrameX = (short)(num * 84);
            if (Main.tile[i - 1, j - 3].TileFrameX != 7)
            {
                Main.tile[i - 1, j - 3].TileFrameX = 7;
            }*/
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0)
            {
                Point point = new Point(i, j);
                if (tile != null && tile.HasTile)
                {
                    Texture2D value = MythContent.QuickTexture("TheFirefly/Tiles/GlowWood");
                    Texture2D valueG = MythContent.QuickTexture("TheFirefly/Tiles/GlowWoodGlow");
                    Vector2 value2 = point.ToWorldCoordinates(8f, 8f);
                    Color color = Lighting.GetColor(point.X, point.Y);
                    SpriteEffects effects = SpriteEffects.None;
                    Vector2 HalfSize = value.Size() / 2f;
                    Main.spriteBatch.Draw(value, value2 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, 0, value.Width, value.Height), color, 0f, HalfSize, 1f, effects, 0f);
                    Main.spriteBatch.Draw(valueG, value2 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, 0, value.Width, value.Height), new Color(1f, 1f, 1f,0), 0f, HalfSize, 1f, effects, 0f);
                }
            }
            return false;
        }
    }
}

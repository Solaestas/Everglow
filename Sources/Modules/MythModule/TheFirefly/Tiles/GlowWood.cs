using Terraria.Audio;
using Terraria.Localization;
using Terraria.ObjectData;

namespace MythMod.OceanMod.Tiles.Tree1
{
    public class CyanVineOre : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 300; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
            //HitSound = 4;
            AddMapEntry(new Color(155, 173, 183));
            DustType = DustID.Silver;
            AdjTiles = new int[] { Type };
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("Cyan Vine");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "青缎矿脉");
            base.AddMapEntry(new Color(155, 173, 183), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            //SoundEngine.PlaySound(SoundID.NPCHit4, i * 16, j * 16);
            int Times = Main.rand.Next(5, 9);
            for (int d = 0; d < Times; d++)
            {
                Item.NewItem(null, i * 16 + Main.rand.Next(72), j * 16 + Main.rand.Next(64), 16, 16, ModContent.ItemType<Items.Ores.CyanVineOre>());
            }
            for (int f = 0; f < 13; f++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d);
                Gore.NewGore(null, new Vector2(i * 16, j * 16) + vF, vF, ModContent.Find<ModGore>("MythMod/CyanVineOre" + f.ToString()).Type, 1f);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.Silver, vF.X, vF.Y);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.WoodFurniture, vF.X, vF.Y);
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 4));
            Main.tile[i, j - 3].TileFrameX = (short)(num * 84);
            if (Main.tile[i - 1, j - 3].TileFrameX != 7)
            {
                Main.tile[i - 1, j - 3].TileFrameX = 7;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX % 84 == 0 && tile.TileFrameY == 0)
            {
                Point point = new Point(i, j);
                if (tile != null && tile.HasTile)
                {
                    Texture2D value = ModContent.Request<Texture2D>("MythMod/OceanMod/Tiles/Tree1/CyanVineOre").Value;
                    Vector2 value2 = point.ToWorldCoordinates(24f, 64f);
                    Color color = Lighting.GetColor(point.X, point.Y);
                    SpriteEffects effects = SpriteEffects.None;
                    Main.spriteBatch.Draw(value, value2 - Main.screenPosition + new Vector2(192, 166), new Rectangle(tile.TileFrameX, 0, 84, 72), color, 0f, new Vector2(42, 36), 1f, effects, 0f);
                    Vector2 v0 = value2 - Main.screenPosition + new Vector2(0, -16);
                    Vector2 v1 = Main.LocalPlayer.Center - Main.screenPosition;
                    if ((v1 - v0).Length() < 120)
                    {
                        if (Math.Abs(Main.MouseScreen.X - v0.X) < 38 && Math.Abs(Main.MouseScreen.Y - v0.Y) < 32)
                        {
                            Texture2D t2 = ModContent.Request<Texture2D>("MythMod/OceanMod/Tiles/Tree1/CyanVineOreOutline").Value;
                            Main.spriteBatch.Draw(t2, value2 - Main.screenPosition + new Vector2(192, 166), new Rectangle(tile.TileFrameX, 0, 84, 72), new Color(255, 255, 255, 0), 0f, new Vector2(42, 36), 1f, effects, 0f);
                        }
                    }
                }
            }
            return false;
        }
    }
}

using Terraria.Localization;
using Terraria.ObjectData;

namespace MythMod.OceanMod.Tiles.Town
{
    public class LanternCrystal : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
                                          //Main.tileShine[Type] = 300; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(168, 0, 28));

            DustType = DustID.Gold;
            AdjTiles = new int[] { Type };
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("Royal City Pylon");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "皇城晶塔");
            base.AddMapEntry(new Color(114, 92, 82), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<OceanMod.Items.Town.LanternCrystal>());
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        int FraY = 0;
        int DFY = 0;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX != 18 || !(tile.TileFrameY == 54 || tile.TileFrameX == 126))
            {
                return;
            }
            DFY++;
            if (DFY % 8 == 2)
            {
                DFY = 0;
                if (FraY < 7)
                {
                    FraY++;
                }
                else
                {
                    FraY = 0;
                }
            }
            for (int x = 0; x < 2; x++)
            {
                Point point = new Point(i, j);

                if (tile != null && tile.HasTile)
                {
                    Texture2D value = ModContent.Request<Texture2D>("MythMod/OceanMod/Tiles/Town/RedLanternCrystal").Value;
                    Texture2D valueB = ModContent.Request<Texture2D>("MythMod/OceanMod/Tiles/Town/RedLanternCrystalBound").Value;
                    int frameY = (int)(tile.TileFrameX / 54);
                    bool flag = tile.TileFrameY / 72 != 0;
                    int horizontalFrames = 1;
                    int verticalFrames = 27;
                    Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, 0, frameY, 0, 0);
                    Vector2 origin = rectangle.Size() / 2f;
                    Vector2 value2 = point.ToWorldCoordinates(24f, 64f);
                    float num3 = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 5f));
                    Vector2 value3 = value2 + new Vector2(0f, -40f) + new Vector2(0f, num3 * 4f);
                    Color color = Lighting.GetColor(point.X, point.Y);
                    SpriteEffects effects = flag ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Main.spriteBatch.Draw(value, value3 - Main.screenPosition + new Vector2(176, 114), new Rectangle(0, 46 * FraY, 28, 44), color, 0f, origin, 1f, effects, 0f);
                    if (Main.SmartCursorIsUsed)
                    {
                        Vector2 v1 = new Vector2(i * 16, j * 16) - Main.LocalPlayer.Center;
                        if (v1.Length() < 100)
                        {
                            Main.spriteBatch.Draw(valueB, value3 - Main.screenPosition + new Vector2(176, 114), new Rectangle(0, 46 * FraY, 28, 44), new Color(45, 45, 45), 0f, origin, 1f, effects, 0f);
                            Vector2 v2 = new Vector2(i * 16, j * 16) - Main.MouseWorld;
                            if (v1.Length() < 20)
                            {
                                Main.spriteBatch.Draw(valueB, value3 - Main.screenPosition + new Vector2(176, 114), new Rectangle(0, 46 * FraY, 28, 44), Color.Gold, 0f, origin, 1f, effects, 0f);
                            }
                        }
                    }
                    //Lighting.AddLight(i, j, 0.5f, 0, 0);
                    float scale = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 2f)) * 0.3f + 0.7f;
                    Color color2 = color;
                    color2.A = 0;
                    color2 = color2 * 0.1f * scale;
                    for (float num4 = 0f; num4 < 1f; num4 += 0.166666672f)
                    {
                        Main.spriteBatch.Draw(value, value3 - Main.screenPosition + (6.28318548f * num4).ToRotationVector2() * (6f + num3 * 2f) + new Vector2(176, 114), new Rectangle(0, 46 * FraY, 28, 44), color2, 0f, origin, 1f, effects, 0f);
                    }
                }
            }
            //base.PostDraw(i, j, spriteBatch);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Typeless.TransportCircle>()] < 1)
            {
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Typeless.TransportCircle>(), 0, 0, player.whoAmI, 0);
            }
            return true;
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<OceanMod.Items.Town.LanternCrystal>();
        }
    }
}
//

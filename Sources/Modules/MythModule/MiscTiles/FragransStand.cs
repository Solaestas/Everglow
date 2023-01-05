using Everglow.Sources.Commons.Function.FeatureFlags;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.MiscTiles
{
    public class FragransStand : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 20 };
            TileObjectData.addTile(Type);

            DustType = DustID.Ash;
            // TODO: Actual Localization Needed
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("Moon Fragrans"); //Translated to "Moon Fragrans" currently
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "天外之月"); //Translated to "天外之月" currently
            base.AddMapEntry(new Color(44, 12, 53), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<MiscItems.FragransStand>());
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        Vector2[] vCl = new Vector2[4];
        float[] velo = new float[4];
        float[] Col = new float[4];
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
                zero = Vector2.Zero;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX != 18 || tile.TileFrameY != 36)
            {
                return;
            }
            int num = 11;
            int num2 = 12;
            for (int x = 0; x < 2; x++)
            {
                Point point = new Point(i, j);

                if (tile != null && tile.HasTile)
                {
                    Texture2D value = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Typeless/Moon").Value;
                    int frameY = (int)(tile.TileFrameX / 54);
                    bool flag = tile.TileFrameY / 72 != 0;
                    int horizontalFrames = 1;
                    int verticalFrames = 27;
                    Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, 0, frameY, 0, 0);
                    Vector2 origin = rectangle.Size() / 2f;
                    Vector2 value2 = point.ToWorldCoordinates(24f, 64f);
                    float num3 = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 5f));
                    Vector2 value3 = value2 + new Vector2(0f, -40f) + new Vector2(0f, num3 * 4f);
                    Color color = new Color(255, 255, 255, 0);
                    SpriteEffects effects = flag ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    Main.spriteBatch.Draw(value, value3 - Main.screenPosition + zero, null, color, 0f, origin, 1f, effects, 0f);
                    float scale = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 2f)) * 0.3f + 0.7f;
                    Color color2 = color;
                    color2.A = 0;
                    color2 = color2 * 0.1f * scale;
                    for (float num4 = 0f; num4 < 1f; num4 += 0.166666672f)
                    {
                        Main.spriteBatch.Draw(value, value3 - Main.screenPosition + (6.28318548f * num4).ToRotationVector2() * (6f + num3 * 2f) + zero, null, color2, 0f, origin, 1f, effects, 0f);
                    }
                    for (int z = 0; z < 4; z++)
                    {
                        if (vCl[z] == Vector2.Zero)
                        {
                            vCl[z] = new Vector2(Main.rand.Next(-20, 15), Main.rand.Next(1, 40));
                            velo[z] = Main.rand.NextFloat(0.15f, 0.3f);
                            Col[z] = 0;
                        }
                        if (vCl[z].X > 8)
                        {
                            Col[z] -= 0.05f;
                        }
                        else
                        {
                            Col[z] += 0.05f;
                        }
                        if (Col[z] <= 0)
                        {
                            vCl[z] = new Vector2(Main.rand.Next(-20, -10), Main.rand.Next(1, 40));
                            velo[z] = Main.rand.NextFloat(0.15f, 0.3f);
                            Col[z] = 0.05f;
                        }
                        vCl[z].X += velo[z];
                        //TODO: Fix Object Reference Error
                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Typeless/Cloud" + z.ToString()).Value, value3 - Main.screenPosition + zero + vCl[z], null, new Color((int)(255 * Col[z]), (int)(255 * Col[z]), (int)(255 * Col[z]), 0), 0f, origin, 2f, effects, 0f);
                    }
                }
            }
            Player player = Main.LocalPlayer;
            int numbSta = 0;
            for (int x = 0; x < 58; x++)
            {
                if (player.inventory[x].type == ModContent.ItemType<MiscItems.Materials.FragransSoul>() && player.inventory[x].stack > 0)
                {
                    numbSta += player.inventory[x].stack;
                }
            }
            Vector2 vL = Main.MouseWorld - new Vector2(i * 16, j * 16);
            if (Over > 0 && vL.Length() < 60)
            {
                Over--;
                // TODO: Actual Localization Needed
                string Tex1 = "Right Click will consume 120 fragrans soul to start a random prize\n       Now you have ";
                string Tex2 = " fragrans soul";
                if (Language.ActiveCulture.Name == "zh-Hans")
                {
                    Tex1 = "右键消耗300金桂之魂启动一次抽奖\n         当前拥有";
                    Tex2 = "金桂之魂";
                }
                Point point = new Point(i, j);
                Vector2 value2 = point.ToWorldCoordinates(24f, 64f);
                float num3 = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 5f));
                Vector2 value3 = value2 + new Vector2(0f, -40f) + new Vector2(0f, num3 * 4f);
                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Tex1 + numbSta + Tex2, value3 - Main.screenPosition + zero, Color.Wheat, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            }
            else
            {
                Over = 0;
            }
        }
        int Over = 0;
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Over = 2;
            base.MouseOver(i, j);
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<MiscItems.Materials.FragransSoul>();

        }
        int NoP = 0;
        int NoO = 0;
        int addP = 0;
        int addO = 0;
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            int numbSta = 0;
            for (int x = 0; x < 58; x++)
            {
                if (player.inventory[x].type == ModContent.ItemType<MiscItems.Materials.FragransSoul>())
                {
                    numbSta += player.inventory[x].stack;
                    if (EverglowConfig.DebugMode) { Main.NewText("Consumed Fragrans Soul"); }
                }
            }
            if (numbSta < 120)
            {
                string tex = "Not Enough Fragrans Souls";
                if (Language.ActiveCulture.Name == "zh-Hans")
                {
                    tex = "金桂之魂不足";
                }
                CombatText.NewText(new Rectangle((int)i * 16 - 10, (int)j * 16 - 10, 20, 20), Color.Wheat, tex);
                return false;
            }
            int numbDe = 120;
            if (numbSta >= 120)
            {
                for (int x = 0; x < 58; x++)
                {
                    if (player.inventory[x].type == ModContent.ItemType<MiscItems.Materials.FragransSoul>())
                    {
                        if (player.inventory[x].stack >= numbDe)
                        {
                            player.inventory[x].stack -= numbDe;
                            break;
                        }
                        else
                        {
                            numbDe -= player.inventory[x].stack;
                            player.inventory[x].stack = 0;
                        }
                        numbSta += player.inventory[x].stack;
                    }
                }
            }
            int Value = Main.rand.Next(10000);
            Vector2 v = new Vector2(0, 1800).RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f));
            addP = (int)(NoP * NoP * NoP * NoP * 1.5 - 3000);
            if (addP < 0)
            {
                addP = 0;
            }
            addO = (int)(NoO * NoO * NoO * NoO / 10000f - 5000);
            if (addP < 0)
            {
                addP = 0;
            }
            if (addO < 0)
            {
                addO = 0;
            }
            if (Value < 800 + addP + addO)
            {
                if (Value < 60 + addO)
                {
                    Projectile.NewProjectile(null, new Vector2(i * 16f, j * 16f) - v, v / 100f, ModContent.ProjectileType<MiscProjectiles.Typeless.OrangeStar>(), 0, 0, Main.LocalPlayer.whoAmI, i * 16f, j * 16f);
                    NoO = 0;
                }
                else
                {
                    Projectile.NewProjectile(null, new Vector2(i * 16f, j * 16f) - v, v / 100f, ModContent.ProjectileType<MiscProjectiles.Typeless.PurpleStar>(), 0, 0, Main.LocalPlayer.whoAmI, i * 16f, j * 16f);
                    NoO++;
                    NoP = 0;
                }
            }
            else
            {
                Projectile.NewProjectile(null, new Vector2(i * 16f, j * 16f) - v, v / 100f, ModContent.ProjectileType<MiscProjectiles.Typeless.BlueStar>(), 0, 0, Main.LocalPlayer.whoAmI, i * 16f, j * 16f);
                NoP++;
                NoO++;
            }
            return base.RightClick(i, j);
        }
    }
}

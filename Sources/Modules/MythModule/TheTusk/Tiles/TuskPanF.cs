using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Tiles
{
    public class TuskPanF : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16
            };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.addTile(Type);
            DustType = 4;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "");
            base.AddMapEntry(new Color(0, 0, 0, 0), modTranslation);
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public int TpTime = 0;
        public static int[] PlayerTpTime = new int[255];
        private int Col = 0;
        public override void PostDraw(int i, int j, SpriteBatch sb)
        {
            Player player = Main.LocalPlayer;
            if ((player.Center - new Vector2(i * 16, j * 16 - 72)).Length() < 80)
            {
                if (!Main.gamePaused)
                {
                    TpTime += 3;
                }
                Col = 100;
                //if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscProjectiles.TransportCircle>()] < 1)
                //{
                    Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.TuskTranF>(), 0, 0, player.whoAmI, 0);
                //}
                for (int f = 0; f < Main.projectile.Length; f++)
                {
                    if (Main.projectile[f].owner == player.whoAmI)
                    {
                        if (Main.projectile[f].type == ModContent.ProjectileType<MiscProjectiles.TuskTranF>())
                        {
                            Main.projectile[f].ai[0] = TpTime;
                        }
                    }
                }
            }
            else
            {
                if (Col > 0)
                {
                    Col -= 5;
                }
                else
                {
                    Col = 0;
                    TpTime = 0;
                }//Projectile.ai[1] == 7
                for (int f = 0; f < Main.projectile.Length; f++)
                {
                    if (Main.projectile[f].owner == player.whoAmI)
                    {
                        if (Main.projectile[f].type == ModContent.ProjectileType<MiscProjectiles.TuskTranF>())
                        {
                            Main.projectile[f].ai[1] = 7;
                        }
                    }
                }
            }
            if (TpTime >= 120)
            {
                for (int a = TpH; a < 0; a++)
                {
                    if (Main.tile[(int)(player.position.X / 16f), (int)(player.position.Y / 16f) + a].HasTile)
                    {
                        for (int z = 0; z < 120; z++)
                        {
                            Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
                            Vector2 vF2 = new Vector2(0, Main.rand.NextFloat(0, 15f)).RotatedByRandom(6.28);
                            Dust.NewDust(player.Center + vF2, 0, 0, 183, vF.X, vF.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 2.1f));
                        }
                        player.position += new Vector2(0, a * 16 - 64);
                        for (int z = 0; z < 120; z++)
                        {
                            Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28);
                            Vector2 vF2 = new Vector2(0, Main.rand.NextFloat(0, 15f)).RotatedByRandom(6.28);
                            Dust.NewDust(player.Center + vF2, 0, 0, 183, vF.X, vF.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 2.1f));
                        }
                        Col = 0;
                        TpTime = 0;
                        break;
                    }
                }
            }
            TileI = i;
            TileJ = j;
            PlayerTpTime[player.whoAmI] = TpTime;
        }

        private int TpH = -200;
        public static void DrawAll(SpriteBatch sb)
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Texture2D Tdoor = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Tusk/CosmicFlame").Value;
            Texture2D Tdoor2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Tusk/CosmicVort").Value;
            Texture2D Tdoor3 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Tusk/CosmicPerlin").Value;
            Vector2 Correction = new Vector2(-186f, -260f);
            Main.spriteBatch.Draw(Tdoor, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 30f, new Vector2(56), 65f / 45f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tdoor, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(100, 100, 100, 0), -(float)Main.time / 20f, new Vector2(56), 65f / 45f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tdoor, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 15f, new Vector2(56), 65f / 50f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tdoor2, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 30f, new Vector2(56), 65 / 45f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tdoor3, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), -(float)Main.time / 20f, new Vector2(56), 65f / 45f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tdoor3, new Vector2(TileI * 16, TileJ * 16) + Correction - Main.screenPosition + zero, null, new Color(255, 255, 255, 0), (float)Main.time / 15f, new Vector2(56), 65 / 45f, SpriteEffects.None, 0f);
        }
        public static float TileI = 0;
        public static float TileJ = 0;
    }
}

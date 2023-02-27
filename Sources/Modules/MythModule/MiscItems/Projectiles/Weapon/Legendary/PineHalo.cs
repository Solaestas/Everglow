using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Localization;


namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class PineHalo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pine Halo");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "青松法环");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
        }
        int addi = 0;
        int Energy = 0;
        int Energy2 = 2;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            addi++;
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            HasForst = player.ownedProjectileCounts[ModContent.ProjectileType<PineVortex>()] > 0;
            Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0).RotatedBy(Projectile.ai[0] / 4d) * (8f - Projectile.ai[0] * 4);
            if (Main.mouseLeft)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - Math.PI / 2d));
                Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 0.25);

                Projectile.timeLeft = 5 + Energy;
                if (Energy < 120)
                {
                    Energy += 6;
                }
                else
                {
                    Energy = 120;
                }
                Projectile.timeLeft = 20;
                if (HasForst)
                {
                    if (Energy2 < 140)
                    {
                        Energy2 += 7;
                    }
                    else
                    {
                        Energy2 = 140;
                    }
                }
                else
                {
                    if (Energy2 > 0)
                    {
                        Energy2 -= 7;
                    }
                    else
                    {
                        Energy2 = 0;
                    }
                }
            }
            else
            {
                Energy = Math.Min(Projectile.timeLeft * 6, Energy);
                Energy2 = Math.Min(Projectile.timeLeft * 7, Energy2);
            }
        }
        Vector3[] CirclePoint = new Vector3[120];
        float Rad = 0;
        float Rad2 = 0;
        Vector2[] Circle2D = new Vector2[120];
        float Cy2 = -37.5f;
        float cirpro = 0;
        Vector2 DrawAdd = Vector2.Zero;
        double HaloRot = 0;
        Vector2 Halo1 = Vector2.Zero;
        bool HasForst = false;
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Texture2D TexMain = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Legendary/FrozenStormPine").Value;
            Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
            SpriteEffects se = SpriteEffects.None;
            float AdRot = 0;
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            Vector2 v1 = Vector2.Normalize(v0) * 40f * (float)(1 + Math.Sin(addi / 20d) / 6d);
            if (!Main.gamePaused)
            {
                Halo1 = v1;
            }

            if (Projectile.Center.X < player.Center.X)
            {
                se = SpriteEffects.FlipVertically;
                player.direction = -1;
                AdRot = -(float)(Math.PI * 0.5);
            }
            else
            {
                player.direction = 1;
            }
            if (!Main.gamePaused)
            {
                DrawAdd = Vector2.Normalize(v0) * 26f;
            }

            Cy2 = 0;//三维Y向校正量
            Rad = (float)Energy * 0.75f;//半径
            Rad2 = (float)Energy2 / 100f;//半径2
            cirpro += 0.5f;
            for (int d = 0; d < 120; d++)
            {
                Circle2D[d] = new Vector2(30, 0).RotatedBy(d * Math.PI / 60d);//2D平面圆
                CirclePoint[d] = new Vector3(Circle2D[d].X, -15, 50 + Circle2D[d].Y);//向3维投影
            }
            for (int d = 0; d < 120; d++)
            {
                Circle2D[d] = new Vector2(CirclePoint[d].X / (float)CirclePoint[d].Z, CirclePoint[d].Y / (float)CirclePoint[d].Z + 0.3f/*二维Y向校正量*/) * Rad * (float)(1 + Math.Sin(addi / 31d + 5) / 7d);//落回2D
            }
            //背景层
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Vector2 Vbase = player.Center - Main.screenPosition;

            List<Vertex2D> Vx4 = new List<Vertex2D>();
            if (!Main.gamePaused)
            {
                HaloRot = Math.Atan2(v0.Y, v0.X) + Math.PI / 2d;
            }

            for (int h = 0; h < 120; h++)
            {
                if (CirclePoint[h].Z < 50)
                {
                    Vx4.Add(new Vertex2D(Vbase + Circle2D[(h) % 120].RotatedBy(HaloRot) + Halo1, Color.White, new Vector3(((h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx4.Add(new Vertex2D(Vbase + Circle2D[(h + 1) % 120].RotatedBy(HaloRot) + Halo1, Color.White, new Vector3(((0.999f + h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx4.Add(new Vertex2D(Vbase + new Vector2(0, -0.3f * Rad * (float)(1 + Math.Sin(addi / 31d + 5) / 7d)).RotatedBy(HaloRot) + Halo1, Color.White, new Vector3(((0.5f + h + cirpro) / 30f) % 1f, 1, 0)));
                }
            }

            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/GreenHalo").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx4.ToArray(), 0, Vx4.Count / 3);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            List<Vertex2D> Vx5 = new List<Vertex2D>();
            if (!Main.gamePaused)
            {
                HaloRot = Math.Atan2(v0.Y, v0.X) + Math.PI / 2d;
            }

            for (int h = 0; h < 120; h++)
            {
                if (CirclePoint[h].Z < 50)
                {
                    Vx5.Add(new Vertex2D(Vbase + Circle2D[(h) % 120].RotatedBy(HaloRot) * 2 * Rad2 + Halo1 * 1.35f, new Color(0, 125, 255, 0), new Vector3(((h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx5.Add(new Vertex2D(Vbase + Circle2D[(h + 1) % 120].RotatedBy(HaloRot) * 2 * Rad2 + Halo1 * 1.35f, new Color(0, 125, 255, 0), new Vector3(((0.999f + h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx5.Add(new Vertex2D(Vbase + new Vector2(0, -0.3f * Rad * (float)(1 + Math.Sin(addi / 31d + 5) / 7d)).RotatedBy(HaloRot) * 2 * Rad2 + Halo1 * 1.35f, new Color(0, 125, 255, 0), new Vector3(((0.5f + h + cirpro) / 30f) % 1f, 1, 0)));
                }
            }

            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/GoldLine").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx5.ToArray(), 0, Vx5.Count / 3);

            //绘制法杖
            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition + DrawAdd, null, drawColor, Projectile.rotation + AdRot, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);


            //前景层
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            List<Vertex2D> Vx3 = new List<Vertex2D>();
            for (int h = 0; h < 120; h++)
            {
                if (CirclePoint[h].Z >= 50)
                {
                    Vx3.Add(new Vertex2D(Vbase + Circle2D[(h) % 120].RotatedBy(HaloRot) + Halo1, Color.White, new Vector3(((h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new Vertex2D(Vbase + Circle2D[(h + 1) % 120].RotatedBy(HaloRot) + Halo1, Color.White, new Vector3(((0.999f + h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new Vertex2D(Vbase + new Vector2(0, -0.3f * Rad * (float)(1 + Math.Sin(addi / 31d + 5) / 7d)).RotatedBy(HaloRot) + Halo1, Color.White, new Vector3(((0.5f + h + cirpro) / 30f) % 1f, 1, 0)));
                }
            }

            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/GreenHalo").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx3.ToArray(), 0, Vx3.Count / 3);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            List<Vertex2D> Vx6 = new List<Vertex2D>();
            if (!Main.gamePaused)
            {
                HaloRot = Math.Atan2(v0.Y, v0.X) + Math.PI / 2d;
            }

            for (int h = 0; h < 120; h++)
            {
                if (CirclePoint[h].Z >= 50)
                {
                    Vx6.Add(new Vertex2D(Vbase + Circle2D[(h) % 120].RotatedBy(HaloRot) * 2 * Rad2 + Halo1 * 1.35f, new Color(0, 125, 255, 0), new Vector3(((h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx6.Add(new Vertex2D(Vbase + Circle2D[(h + 1) % 120].RotatedBy(HaloRot) * 2 * Rad2 + Halo1 * 1.35f, new Color(0, 125, 255, 0), new Vector3(((0.999f + h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx6.Add(new Vertex2D(Vbase + new Vector2(0, -0.3f * Rad * (float)(1 + Math.Sin(addi / 31d + 5) / 7d)).RotatedBy(HaloRot) * 2 * Rad2 + Halo1 * 1.35f, new Color(0, 125, 255, 0), new Vector3(((0.5f + h + cirpro) / 30f) % 1f, 1, 0)));
                }
            }

            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/GoldLine").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx6.ToArray(), 0, Vx6.Count / 3);
        }
    }
}

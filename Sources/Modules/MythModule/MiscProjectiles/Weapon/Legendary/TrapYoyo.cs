using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.TheTusk.Projectiles;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class TrapYoyo : ModProjectile
    {
        private int[] Np = new int[5];
        private bool X = true;
        private float Adding = 0;
        private float R = 48;
        private float Omega = 0.04f;
        private int Ad2 = 0;
        private int CryzyTime = 0;
        private bool CTime = false;
        private float Vmax = 8f;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(547);
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 360f;
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TrapYoyoLight");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "机关球");
        }
        public override void AI()
        {
            if (CTime)
            {
                ProjectileExtras.YoyoAI(Projectile.whoAmI, 240f, 360f, 16f);
            }
            else
            {
                ProjectileExtras.YoyoAI(Projectile.whoAmI, 240f, 360f, 14f);
            }
            if (X)
            {
                for (int i = 0; i < 5; i++)
                {
                    Np[i] = -1;
                }
                X = false;
            }
            Adding += Omega;
            Ad2 += 1;
            int Cp = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Np[i] != -1 && Main.projectile[Np[i]].type == ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.TrapYoyoLight>() && Main.projectile[Np[i]].active)
                {
                    Cp += 1;
                }
                else
                {
                    Np[i] = -1;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                if (Np[i] != -1)
                {
                    if (Main.projectile[Np[i]].type == ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.TrapYoyoLight>())
                    {
                        if (Main.projectile[Np[i]].active)
                        {
                            //Main.projectile[Np[i]].position = Projectile.position + new Vector2(0, 48).RotatedBy(MathHelper.TwoPi * i / 5d + Adding);
                            Vector2 poPosi = Projectile.position + new Vector2(0, R).RotatedBy(MathHelper.TwoPi * i / 5d + Adding);
                            Vector2 Pcen = Main.projectile[Np[i]].position;
                            Vector2 PaV = Projectile.position + new Vector2(0, R).RotatedBy(MathHelper.TwoPi * i / 5d + Adding) - Main.projectile[Np[i]].position;
                            if (PaV.Length() > Vmax)
                            {
                                PaV = PaV / PaV.Length() * Vmax;
                            }
                            Main.projectile[Np[i]].velocity = PaV;
                            if (CryzyTime <= 0)
                            {
                                Main.projectile[Np[i]].timeLeft = 60;
                            }
                        }
                        else
                        {
                            Np[i] = -1;
                        }
                    }
                }
            }
            if (Ad2 % 90 == 0 && CryzyTime <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Np[i] == -1)
                    {
                        Np[i] = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.TrapYoyoLight>(), Projectile.damage * 2, 0.2f, Main.myPlayer, 0f, 0f);
                        break;
                    }
                }
            }
            if (CryzyTime > 0)
            {
                CryzyTime--;
                Omega += 0.006f;
                if (R < 25)
                {
                    Vmax += 0.5f;
                }
                if (R > 0)
                {
                    R -= 1;
                    if (R == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                    }
                }
                else
                {
                    CTime = true;
                }
                if (CryzyTime == 0)
                {
                    R = 48;
                    Omega = 0.04f;
                    CTime = false;
                    Cp = 0;
                    Vmax = 8f;
                }
            }
            else
            {
                CryzyTime = 0;
            }
            if (Cp == 5 && CryzyTime <= 0)
            {
                if (Main.mouseRight)
                {
                    SoundEngine.PlaySound(SoundID.Item24, Projectile.Center);
                    CryzyTime = 200;
                }
            }
            if (CTime)
            {
                if (Ad2 % 2 == 0)
                {
                    Vector2 v = new Vector2(0, Main.rand.NextFloat(3f, 9f)).RotatedByRandom(MathHelper.TwoPi);
                    v += Projectile.velocity;
                    Player p = Main.player[Projectile.owner];
                    int num2 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center - v, v, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.AuDust>(), Projectile.damage / 3, 0.2f, Main.myPlayer, 5 + p.GetCritChance(DamageClass.Melee), 0f);
                    Main.projectile[num2].timeLeft = 80;
                }
                Vector2 v2 = new Vector2(0, Main.rand.NextFloat(1.5f, 4f)).RotatedByRandom(MathHelper.TwoPi);
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 87, 0f, 0f, 100, default(Color), 1.6f);
                Main.dust[num].velocity *= v2 + Projectile.velocity;
                Main.dust[num].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (CTime)
            {
                SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
                for (int i = 0; i < 8; i++)
                {
                    Vector2 v = new Vector2(0, Main.rand.NextFloat(5f, 35f)).RotatedByRandom(MathHelper.TwoPi);
                    int num2 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.AuDust>(), Projectile.damage / 3, 0.2f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num2].timeLeft = 200;
                }
                for (int i = 0; i < 25; i++)
                {
                    Vector2 v = new Vector2(0, Main.rand.NextFloat(1.5f, 7f)).RotatedByRandom(MathHelper.TwoPi);
                    int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 87, 0f, 0f, 100, default(Color), 1.6f);
                    Main.dust[num].velocity *= v;
                    Main.dust[num].noGravity = true;
                }
            }
        }
        private int DelOme = 0;
        private Effect ef;
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/TrapYoyoGlow").Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
            if (CTime)
            {
                Texture2D texture2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/TrapYoyoLight").Value;
                for (int i = 0; i < 3; i++)
                {
                    Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/Trail").Value;
            double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
            double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
            int width = (int)(120 - omega * 480);
            if (width <= 10)
            {
                width = 10;
            }
            if (width <= 30)
            {
                DelOme = width;
            }
            if (width > DelOme)
            {
                width = DelOme;
            }
            width /= 2;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(8, 8), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(8, 8), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }
            List<Vertex2D> triangleList = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Projectile.velocity, Color.White, new Vector3(0, 0.5f, 1));
                triangleList.Add(bars[1]);
                triangleList.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapYellow").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                ef.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}

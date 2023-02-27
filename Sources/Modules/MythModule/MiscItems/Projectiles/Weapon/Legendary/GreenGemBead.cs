using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class GreenGemBead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gem Bead");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "宝石珠");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 3600;
            Projectile.hostile = false;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }
        public override void AI()
        {
            if (Tokill < 0)
            {
                for (float v = 0; v < Projectile.velocity.Length(); v += 1f)
                {
                    int r = Dust.NewDust(Projectile.Center - Vector2.Normalize(Projectile.velocity) * v - new Vector2(4), 0, 0, 89, 0, 0, 200, new Color(DrawC / 3f, DrawC / 3f, DrawC / 3f, 0), Math.Clamp(DrawC + 0.1f, 0, 0.5f));
                    Main.dust[r].velocity.X = 0;
                    Main.dust[r].velocity.Y = 0;
                    Main.dust[r].noGravity = true;
                    if (Main.rand.NextBool(8))
                    {
                        int r2 = Dust.NewDust(Projectile.Center - Vector2.Normalize(Projectile.velocity) * v - new Vector2(4), 0, 0, ModContent.DustType<MiscItems.Dusts.GreenEffect2>(), 0, 0, 200, new Color(DrawC / 3f, DrawC / 3f, DrawC / 3f, 0), DrawC + 0.4f);
                        Main.dust[r2].velocity.X = 0;
                        Main.dust[r2].velocity.Y = 0;
                        Main.dust[r2].noGravity = true;
                    }
                }
            }
            if (Tokill >= 0 && Tokill <= 2)
            {
                Projectile.Kill();
            }
            Player player = Main.player[Projectile.owner];
            if (Tokill <= 15 && Tokill > 0)
            {
                Projectile.Center = vp;
                Projectile.velocity = Projectile.oldVelocity;
            }
            Tokill--;
            Projectile.velocity.Y += 0.07f;
        }
        Vector2 vp = Vector2.Zero;
        private bool Nul = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Projectile.oldVelocity;
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center);
            vp = Projectile.Center;
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) + Projectile.velocity, Projectile.width, Projectile.height, 89, 0f, 0f, 100, default(Color), DrawC + 0.5f);
                Main.dust[num].velocity *= 1.1f;
                Main.dust[num].noGravity = true;
            }
            for (int j = 0; j < 18; j++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2f, 7f)).RotatedByRandom(6.28) * DrawC;
                int ds = Projectile.NewProjectile(null, Projectile.Center + v * 3f + Projectile.velocity, v, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.BrokenGem>(), Projectile.damage / 4, 1, Main.myPlayer, 4);
                Main.projectile[ds].scale = DrawC;
            }
            for (int j = 0; j < 30; j++)
            {
                Vector2 v0 = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(Math.PI * 2);
                int num22 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2) + Projectile.velocity, 2, 2, ModContent.DustType<MiscItems.Dusts.GreenEffect2>(), v0.X, v0.Y, 0, default(Color), DrawC + 0.5f);
            }
            //Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + Projectile.velocity, Vector2.Zero, ModContent.ProjectileType<Projectiles.Ranged.Slingshots.SlingshotHitGreen>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, Math.Clamp(DrawC * 2,0,1f), 0f);
            Tokill = 15;
            Player player = Main.player[Projectile.owner];
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Nul = true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Tokill > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.velocity = Projectile.oldVelocity;
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center);
            vp = Projectile.Center;
            for (int i = 0; i < 10; i++)
            {
                int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) + Projectile.velocity, Projectile.width, Projectile.height, 89, 0f, 0f, 100, default(Color), DrawC + 0.5f);
                Main.dust[num].velocity *= 1.1f;
                Main.dust[num].noGravity = true;
            }
            for (int j = 0; j < 18; j++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2f, 7f)).RotatedByRandom(6.28) * DrawC;
                int ds = Projectile.NewProjectile(null, Projectile.Center + v * 3f + Projectile.velocity, v, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.BrokenGem>(), Projectile.damage / 4, 1, Main.myPlayer, 4);
                Main.projectile[ds].scale = DrawC;
            }
            for (int j = 0; j < 30; j++)
            {
                Vector2 v0 = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(Math.PI * 2);
                int num22 = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 8f)).RotatedByRandom(Math.PI * 2) + Projectile.velocity, 2, 2, ModContent.DustType<MiscItems.Dusts.GreenEffect2>(), v0.X, v0.Y, 0, default(Color), DrawC + 0.5f);
            }
            //Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + Projectile.velocity, Vector2.Zero, ModContent.ProjectileType<Projectiles.Ranged.Slingshots.SlingshotHitGreen>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, Math.Clamp(DrawC * 2, 0, 1f), 0f);
            Tokill = 15;
            Player player = Main.player[Projectile.owner];
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Nul = true;
        }
        int Tokill = -1;
        private Effect ef;
        int TrueL = 0;//真实的长度
        Vector2 oVel = Vector2.One;//记录速度
        public override void PostDraw(Color lightColor)
        {
            Projectile.ai[0] = Tokill;
            DrawC = Math.Clamp((Projectile.velocity.Length() - 10f) / 30f, 0, 1f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            TrueL = 1;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                TrueL++;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float width = 6;
                if (Projectile.timeLeft > 30)
                {
                    width = 6;
                }
                else
                {
                    width = Projectile.timeLeft / 5f;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                if (normalDir.Length() < 0.2f)
                {
                    normalDir = Projectile.velocity / Projectile.velocity.Length();
                }
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var color = Color.Lerp(new Color(DrawC, DrawC, DrawC, 0), new Color(0, 0, 0, 0), factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }
            List<Vertex2D> triangleList = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                if (Projectile.velocity.Length() > 0.05f)
                {
                    oVel = Projectile.velocity;
                }
                Vector2 va = Projectile.velocity * 1.5f;
                if (Tokill <= 44 && Tokill > 0)
                {
                    va = Projectile.velocity * 0.05f;
                }
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + va, new Color(DrawC, DrawC, DrawC, 0), new Vector3(0, 0.5f, 1));
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
                Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/EShootGreen").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        float DrawC = 0;
    }
}


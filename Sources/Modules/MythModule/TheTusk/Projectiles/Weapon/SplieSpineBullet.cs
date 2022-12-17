using Everglow.Sources.Modules.MythModule.TheTusk;
//using MythMod.Common.Players;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    public class SplieSpineBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SplieSpineBullet");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "脊骨母弹");
        }
        private float num = 0;
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = -1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        int Tokill = -1;
        public override void AI()
        {
            ka = 0.2f;
            if (Projectile.timeLeft < 60f)
            {
                ka = Projectile.timeLeft / 300f;
            }
            if (Tokill < 0)
            {
                for (int j = 0; j < 200; j++)
                {
                    if ((Main.npc[j].Center - (Projectile.Center + Projectile.velocity * 1.5f)).Length() < 120 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                    {
                        Vector2 v0 = Vector2.Normalize(Projectile.velocity);
                        Vector2 v1 = Vector2.Normalize(Main.npc[j].Center - Projectile.Center);
                        float CosAng = Vector2.Dot(v0, v1);//夹角余弦值大于0.707,即为45°
                        if (CosAng > 0.707)//爆
                        {
                            int NumProjectiles = 6;

                            for (int i = 0; i < NumProjectiles; i++)
                            {
                                Vector2 newVelocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
                                newVelocity *= 0.6f - Main.rand.NextFloat(0.2f);
                                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, newVelocity, (int)(Projectile.ai[1]), Projectile.damage, Projectile.knockBack, Projectile.owner);
                            }
                            /*int NumProjectiles2 = Main.rand.Next(2, 4);
                            for (int i = 0; i < NumProjectiles2; i++)
                            {
                                Vector2 newVelocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(13));
                                newVelocity *= 2.4f - Main.rand.NextFloat(0.3f);
                                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, newVelocity, ModContent.ProjectileType<Projectiles.Ranged.BloodyGun>(), (int)(Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner);
                            }*/
                            TuskModPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<TuskModPlayer>();
                            mplayer.Shake = 1;
                            for (int i = 0; i < 16; i++)
                            {
                                Vector2 v = new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi);
                                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 5f));
                            }
                            for (int i = 0; i < 16; i++)
                            {
                                Vector2 v = new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi);
                                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, v.X, v.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.3f));
                            }
                            for (int i = 0; i < 7; i++)
                            {
                                Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 3.2f));
                            }
                            Projectile.velocity = Projectile.oldVelocity;
                            Tokill = 45;//0.75s后消掉
                            Projectile.friendly = false;
                            Projectile.damage = 0;
                            Projectile.tileCollide = false;
                            Projectile.ignoreWater = true;
                            Projectile.aiStyle = -1;
                            break;
                        }
                    }
                }

            }
            if (Tokill >= 0 && Tokill <= 2)
            {
                Projectile.Kill();
            }
            if (Tokill > 0)
            {
                Tokill--;
            }
            if (Tokill <= 44 && Tokill > 0)
            {
                Projectile.position = Projectile.oldPosition;
                Projectile.velocity = Projectile.oldVelocity;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            TuskModPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<TuskModPlayer>();
            mplayer.Shake = 1;
            for (int i = 0; i < 16; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 5f));
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, v.X, v.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.3f));
            }
            for (int i = 0; i < 7; i++)
            {
                Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 3.2f));
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 20 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    Player player2 = Main.player[Projectile.owner];
                    player2.dpsDamage += (int)(Projectile.damage * (1 + Projectile.ai[0] / 100f) * 1f);
                }
            }
            Projectile.velocity = Projectile.oldVelocity;
            Tokill = 45;//0.75s后消掉
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
        }
        private NPC md = Main.npc[0];
        private Vector2 v = new Vector2(0, 0);
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TuskModPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<TuskModPlayer>();
            mplayer.Shake = 1;
            for (int i = 0; i < 16; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 5f));
            }
            for (int i = 0; i < 16; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, v.X, v.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.3f));
            }
            for (int i = 0; i < 7; i++)
            {
                Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(1, 3)).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 3.2f));
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 20 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    Player player2 = Main.player[Projectile.owner];
                    player2.dpsDamage += (int)(Projectile.damage * (1 + Projectile.ai[0] / 100f) * 1f);
                }
            }
            Projectile.velocity = Projectile.oldVelocity;
            Tokill = 45;//0.75s后消掉
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            return false;
        }
        public override void Kill(int timeLeft)
        {
        }
        public override void PostDraw(Color lightColor)
        {
        }
        int TrueL = 1;
        float ka = 0;
        Color c0 = new Color(0, 0, 0, 0);
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = TextureAssets.Projectile[Math.Clamp((int)(Projectile.ai[1]), 0, TextureAssets.Projectile.Length)].Value;
            Color[] Lig = new Color[t.Width * t.Height];
            t.GetData(Lig);
            c0 = Lig[(int)(t.Width * t.Height / 2f - 1)];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
            float width = 2 * Projectile.scale;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft / 30f * Projectile.scale;
            }
            TrueL = 0;
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
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = 1f;
                factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float Tr = c0.R / 300f;
                float Tg = c0.G / 300f;
                float Tb = c0.B / 300f;
                Lighting.AddLight(Projectile.oldPos[i], (float)(255 - Projectile.alpha) * Tr / 50f * ka * (1 - factor), (float)(255 - Projectile.alpha) * Tg / 50f * ka * (1 - factor), (float)(255 - Projectile.alpha) * Tb / 50f * ka * (1 - factor));
                bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(4f, 4f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(1, factor, w)));
                bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(4f, 4f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(0, factor, w)));
            }
            List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity), new Color(254, 254, 254, 0), new Vector3(0.5f, 0, 1));
                Vx.Add(bars[1]);
                Vx.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx.Add(bars[i]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 1]);

                    Vx.Add(bars[i + 1]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 3]);
                }
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            return false;
        }
    }
}

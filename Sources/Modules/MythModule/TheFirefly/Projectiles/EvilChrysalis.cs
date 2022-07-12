using MythMod.Common.Players;

namespace MythMod.Projectiles.Summon
{
    internal class EvilChrysalis : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.PI / 2d));
            if (Projectile.timeLeft == 75)
            {
                player.AddBuff(ModContent.BuffType<Buffs.CorruptMothBuff>(), 18000);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Bottom + new Vector2(Main.rand.NextFloat(-60, 60), -5), new Vector2(0, Main.rand.NextFloat(-8, -2)), ModContent.ProjectileType<Projectiles.Summon.CorruptMoth>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Projectile.damage, Main.rand.NextFloat(0, 200f));
                for (int f = 0; f < 12; f++)
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.Bottom + new Vector2(Main.rand.NextFloat((f - 5.5f) * 10, (f - 4.5f) * 10), Main.rand.NextFloat(-5, 15)), new Vector2(0, Main.rand.NextFloat(-12, -4)), ModContent.ProjectileType<Projectiles.Summon.DarkEffect>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(7f, 15f), 0);

                    for (int z = 0; z < 4; z++)
                    {
                        int ds = Dust.NewDust(player.Bottom + new Vector2(Main.rand.NextFloat(-60, 60), Main.rand.NextFloat(-5, 15)), 0, 0, ModContent.DustType<Dusts.MothBlue2>(), 0, Main.rand.NextFloat(-8, -4), 0, default(Color), Main.rand.NextFloat(0.6f, 1.8f));
                        Main.dust[ds].velocity = new Vector2(0, Main.rand.NextFloat(-8, -4));
                        int es = Dust.NewDust(player.Bottom + new Vector2(Main.rand.NextFloat(-60, 60), Main.rand.NextFloat(-5, 15)), 0, 0, 191, 0, Main.rand.NextFloat(-8, -4), 0, default(Color), Main.rand.NextFloat(0.3f, 1.0f));
                        Main.dust[es].velocity = new Vector2(0, Main.rand.NextFloat(-8, -4));
                    }
                }
            }
            if (Projectile.timeLeft == 78)
            {
                MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
                mplayer.Shake = 1;
                float CritC = player.GetCritChance(DamageClass.Summon) + player.GetCritChance(DamageClass.Generic);
                for (int j = 0; j < 200; j++)
                {
                    if ((Main.npc[j].Center - Projectile.Center).Length() < 60 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f) * 4.0f), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < CritC);
                        player.addDPS((int)(Projectile.damage * (1 + CritC / 100f)));
                    }
                }
            }
        }

        private float Dy = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis").Value;
            Texture2D tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalisG").Value;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            Color c0 = Lighting.GetColor((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f));
            SpriteEffects sp = SpriteEffects.None;
            if (player.direction == -1)
            {
                sp = SpriteEffects.FlipHorizontally;
            }

            if (Projectile.timeLeft >= 75)
            {
                Dy += 0.5f;
            }
            else
            {
                Dy = 0;
            }
            if (Projectile.timeLeft < 72)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis1").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis1G").Value;
            }
            if (Projectile.timeLeft < 66)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis2").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis2G").Value;
            }
            if (Projectile.timeLeft < 60)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis3").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis3G").Value;
            }
            if (Projectile.timeLeft < 54)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalisG").Value;
            }
            if (Projectile.timeLeft < 48)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis2").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis2G").Value;
            }
            if (Projectile.timeLeft < 42)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalisG").Value;
            }
            if (Projectile.timeLeft < 36)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis3").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis3G").Value;
            }
            if (Projectile.timeLeft < 30)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalisG").Value;
            }
            if (Projectile.timeLeft < 24)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis4").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis4G").Value;
            }
            if (Projectile.timeLeft < 18)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalisG").Value;
            }
            if (Projectile.timeLeft < 12)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis5").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis5G").Value;
            }
            if (Projectile.timeLeft < 6)
            {
                t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalis").Value;
                tG = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/EvilChrysalisG").Value;
            }
            Main.spriteBatch.Draw(t, player.Center + new Vector2(20 * player.direction, -5 - Dy) - Main.screenPosition, null, c0, (float)(-0.25 * Math.PI * player.direction), drawOrigin, 1, sp, 0);
            Main.spriteBatch.Draw(tG, player.Center + new Vector2(20 * player.direction, -5 - Dy) - Main.screenPosition, null, new Color(255, 255, 255, 0), (float)(-0.25 * Math.PI * player.direction), drawOrigin, 1, sp, 0);
            return false;
        }
        public override void Kill(int timeLeft)
        {
        }

        private Vector3[] CirclePoint = new Vector3[120];
        private float Rad = 0;
        private Vector2[] Circle2D = new Vector2[120];
        private float Cy = -37.5f;
        private float Cy2 = -37.5f;
        private float cirpro = 0;
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft < 75)
            {
                Cy = -15 - Projectile.timeLeft / 4f;
                Cy2 = 27.5f - Projectile.timeLeft;
                Rad = (float)Math.Sin(Projectile.timeLeft / 75d * Math.PI) * 90f;
            }
            cirpro += 0.5f;
            if (Projectile.timeLeft < 75)
            {
                for (int d = 0; d < 120; d++)
                {
                    Circle2D[d] = new Vector2(30, 0).RotatedBy(d * Math.PI / 60d);
                    CirclePoint[d] = new Vector3(Circle2D[d].X, -15, 50 + Circle2D[d].Y);
                }
                for (int d = 0; d < 120; d++)
                {
                    Circle2D[d] = new Vector2(CirclePoint[d].X / CirclePoint[d].Z, CirclePoint[d].Y / CirclePoint[d].Z) * Rad;
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Vector2 Vbase = player.Center - Main.screenPosition;

                List<VertexBase.CustomVertexInfo> Vx3 = new List<VertexBase.CustomVertexInfo>();
                for (int h = 0; h < 120; h++)
                {
                    Vx3.Add(new VertexBase.CustomVertexInfo(Vbase + Circle2D[(h) % 120] - new Vector2(0, Cy2), Color.White, new Vector3(((h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new VertexBase.CustomVertexInfo(Vbase + Circle2D[(h + 1) % 120] - new Vector2(0, Cy2), Color.White, new Vector3(((0.999f + h + cirpro) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new VertexBase.CustomVertexInfo(Vbase + new Vector2(0, -0.3f * Rad) - new Vector2(0, Cy2), Color.White, new Vector3(((0.5f + h + cirpro) / 30f) % 1f, 1, 0)));
                }

                Texture2D t = ModContent.Request<Texture2D>("MythMod/Projectiles/Summon/BlackHalo").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx3.ToArray(), 0, Vx3.Count / 3);
            }
        }
    }
}

using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Buffs;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    internal class DarkFanFly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 250;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.scale = 1.5f;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FanHit>(), 0, 0,player.whoAmI, Math.Max(Projectile.velocity.Length() * 0.2f, 0.4f));
            int[] array = Projectile.localNPCImmunity;
            bool flag = (!Projectile.usesLocalNPCImmunity && !Projectile.usesIDStaticNPCImmunity) || (Projectile.usesLocalNPCImmunity && array[target.whoAmI] == 0) || (Projectile.usesIDStaticNPCImmunity && Projectile.IsNPCIndexImmuneToProjectileType(Projectile.type, target.whoAmI));
            if (target.active && !target.dontTakeDamage && flag && (target.aiStyle != 112 || target.ai[2] <= 1f))
            {
                if (target.active)
                {
                    Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
                }
            }
            int Count = (int)Projectile.ai[0];
            if(Projectile.ai[0] > 3 + player.maxMinions / 5)
            {
                Count = 3 + player.maxMinions / 5;
                Projectile.ai[0] -= 3 + player.maxMinions / 5;
            }
            else
            {
                Count = (int)Projectile.ai[0];
            }
            for (int g = 0; g < Count; g++)
            {
                Vector2 va = new Vector2(0, Main.rand.NextFloat(9f, 11f)).RotatedByRandom(Math.PI * 2);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center + va, va, ModContent.ProjectileType<Projectiles.GlowingButterfly>(), Projectile.damage / 3, Projectile.knockBack, player.whoAmI, player.GetCritChance(DamageClass.Summon) + 8, 0f);
            }
            for (int i = 0; i < 18; i++)
            {
                Dust.NewDust(target.Center, 0, 0, 113, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color), 0.6f);
            }
            for (int i = 0; i < 6; i++)
            {
                int num90 = Dust.NewDust(target.Center - new Vector2(8), 0, 0, 226, 0f, 0f, 100, Color.Blue, Main.rand.NextFloat(0.7f, 1.2f));
                Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(5f, 10f)).RotatedByRandom(6.283);
                Main.dust[num90].noGravity = true;
            }
            target.AddBuff(ModContent.BuffType<Buffs.OnMoth>(), 300);
            int MaxS = -1;
            for (int p = 0; p < 58; p++)
            {
                if (player.inventory[p].type == player.HeldItem.type)
                {
                    MaxS += 1;
                }
            }
            if (MaxS > 5)
            {
                MaxS = 5;
            }
            MothBuffTarget mothBuffTarget = target.GetGlobalNPC<MothBuffTarget>();
            if (mothBuffTarget.MothStack < 5 + MaxS * 0)
            {
                mothBuffTarget.MothStack += 1;
            }
            else
            {
                mothBuffTarget.MothStack = 5 + MaxS * 0;
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 v0 = player.Center - Projectile.Center;
            Vector2 v1 = Vector2.Normalize(v0) * 120f;
            Vector2 v2 = player.Center + v1 - Projectile.Center;
            Vector2 v3 = Vector2.Normalize(v2) * 0.48f;
            Projectile.velocity += v3;
            if (Projectile.timeLeft < 180 && Projectile.timeLeft > 60)
            {
                if (v0.Length() < 48)
                {
                    Projectile.timeLeft = 60;
                }
            }
            Projectile.velocity *= 0.99f;
            for(int x = 58;x >= 0;x--)
            {
                OldVelocity[x + 1] = OldVelocity[x];
            }
            OldVelocity[0] = Projectile.velocity;

            for (int x = 58; x >= 0; x--)
            {
                OldScale[x + 1] = OldScale[x];
            }
            OldScale[0] = Projectile.scale;
            if (Projectile.timeLeft < 100)
            {
                Projectile.tileCollide = false;
            }
            if(Projectile.timeLeft < 60)
            {
                Projectile.timeLeft -= 4;
            }

        }
        public override void Kill(int timeLeft)
        {
        }

        private Vector2 PosRot0 = new Vector2(-16, -100);
        private Vector2 PosRot1 = new Vector2(100, 0);
        private Vector2 PosRot2 = new Vector2(-100, 100);
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        Vector2[] OldVelocity = new Vector2[60];
        float[] OldScale = new float[60];
        public override void PostDraw(Color lightColor)
        {
            float width = 12;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft / 5f;
                Projectile.scale = Projectile.timeLeft / 40f;
            }
            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                TrueL++;
            }
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                float NLength = normalDir.Length();
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                float delta = 0;
                if(i >= 2)
                {
                    var OldnormalDir = Projectile.oldPos[i - 2] - Projectile.oldPos[i - 1];
                    OldnormalDir = Vector2.Normalize(new Vector2(-OldnormalDir.Y, OldnormalDir.X));
                    float sinTheta = Vector3.Cross(new Vector3(normalDir, 0), new Vector3(OldnormalDir, 0)).Z;
                    float cosTheta = Vector2.Dot(normalDir, OldnormalDir);
                    int signS = Math.Sign(sinTheta);
                    if (NLength < width * sinTheta * signS)
                    {
                        delta = -(1 - cosTheta * NLength / width) * signS;
                    }
                }
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                var UpValue = ((delta > 0 ? delta : 0) - 1);
                var DownValue = ((delta < 0 ? delta : 0) + 1);

                Vector2 v3 = new Vector2(0.707f, -0.707f).RotatedBy((Projectile.timeLeft+ i) / 10f);
                double rot = Math.Atan2(OldVelocity[i].Y, OldVelocity[i].X);
                v3 = new Vector2(v3.X, v3.Y / 2f).RotatedBy(rot);
                bars.Add(new Vertex2D(Projectile.oldPos[i] + v3 * 45 * OldScale[i]  + new Vector2(29) - Main.screenPosition, new Color(254, 254, 254, 255), new Vector3(factor, 0, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + v3 * 45 * OldScale[i] + width * normalDir * 2 + new Vector2(29) - Main.screenPosition, new Color(254, 254, 254, 255), new Vector3(factor, 1, w)));
            }
            Texture2D t = MythContent.QuickTexture("TheFirefly/Projectiles/heatmapShade");
            if(bars.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }


            Texture2D Tex = MythContent.QuickTexture("TheFirefly/Projectiles/DarkFanFly");
            Texture2D TexG = MythContent.QuickTexture("TheFirefly/Projectiles/DarkFanFlyGlow");
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
            List<Vertex2D> Vx = new List<Vertex2D>();
            Vector2 v0 = PosRot0.RotatedBy(Projectile.timeLeft / 10f);
            v0 = new Vector2(v0.X, v0.Y / 2f).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            Vector2 v1 = PosRot1.RotatedBy(Projectile.timeLeft / 10f);
            v1 = new Vector2(v1.X, v1.Y / 2f).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            Vector2 v2 = PosRot2.RotatedBy(Projectile.timeLeft / 10f);
            v2 = new Vector2(v2.X, v2.Y / 2f).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            Vx.Add(new Vertex2D(Projectile.Center + v0 * Projectile.scale - Main.screenPosition, color, new Vector3(84f / 200f, 0f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v1 * Projectile.scale - Main.screenPosition, color, new Vector3(200f / 200f, 100f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v2 * Projectile.scale - Main.screenPosition, color, new Vector3(0f / 200f, 200f / 200f, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = Tex;//
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);

            Vx.Add(new Vertex2D(Projectile.Center + v0 * Projectile.scale - Main.screenPosition, new Color(255, 255, 255, 0), new Vector3(84f / 200f, 0f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v1 * Projectile.scale - Main.screenPosition, new Color(255, 255, 255, 0), new Vector3(200f / 200f, 100f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v2 * Projectile.scale - Main.screenPosition, new Color(255, 255, 255, 0), new Vector3(0f / 200f, 200f / 200f, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = TexG;//
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.velocity, Vector2.Zero, ModContent.ProjectileType<FanHit>(), Projectile.damage, 0,player.whoAmI, Math.Max(Projectile.velocity.Length() * 0.2f,0.4f));
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.98f;
            for (int i = 0; i < 18; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default, 0.6f);
            }
            for (int i = 0; i < 6; i++)
            {
                int index = Dust.NewDust(Projectile.position - new Vector2(8), Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Blue, Main.rand.NextFloat(0.7f, 1.2f));
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(5f, 10f)).RotatedByRandom(6.283);
                Main.dust[index].noGravity = true;
            }
            return false;
        }
    }
}

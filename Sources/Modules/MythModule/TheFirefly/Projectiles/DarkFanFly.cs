using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
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
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            int[] array = Projectile.localNPCImmunity;
            bool flag = (!Projectile.usesLocalNPCImmunity && !Projectile.usesIDStaticNPCImmunity) || (Projectile.usesLocalNPCImmunity && array[target.whoAmI] == 0) || (Projectile.usesIDStaticNPCImmunity && Projectile.IsNPCIndexImmuneToProjectileType(Projectile.type, target.whoAmI));
            if (target.active && !target.dontTakeDamage && flag && (target.aiStyle != 112 || target.ai[2] <= 1f))
            {
                if (target.active)
                {
                    Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
                }
            }
            for (int g = 0; g < 10; g++)
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
            if (Projectile.timeLeft < 180)
            {
                if (v0.Length() < 48)
                {
                    Projectile.Kill();
                }
            }
            Projectile.velocity *= 0.99f;
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
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
            Vx.Add(new Vertex2D(Projectile.Center + v0 - Main.screenPosition, color, new Vector3(84f / 200f, 0f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v1 - Main.screenPosition, color, new Vector3(200f / 200f, 100f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v2 - Main.screenPosition, color, new Vector3(0f / 200f, 200f / 200f, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = Tex;//
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);

            Vx.Add(new Vertex2D(Projectile.Center + v0 - Main.screenPosition, new Color(255, 255, 255, 0), new Vector3(84f / 200f, 0f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v1 - Main.screenPosition, new Color(255, 255, 255, 0), new Vector3(200f / 200f, 100f / 200f, 0)));
            Vx.Add(new Vertex2D(Projectile.Center + v2 - Main.screenPosition, new Color(255, 255, 255, 0), new Vector3(0f / 200f, 200f / 200f, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = TexG;//
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
    }
}
